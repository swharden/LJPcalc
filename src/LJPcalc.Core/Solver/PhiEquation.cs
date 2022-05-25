namespace LJPcalc.Core.Solver;

public class PhiEquation
{
    public int EquationCount { get; private set; }
    private readonly double TemperatureC;
    private readonly Ion[] Ions;

    public PhiEquation(Ion[] ions, double temperatureC)
    {
        Ions = ions;
        TemperatureC = temperatureC;
        EquationCount = Ions.Length - 2;
    }

    /// <summary>
    /// Solves for LJP (which calculates new CLs) and returns the percent error for each of the new CLs.
    /// A solution is found when all CLs used to calculate LJP are sufficiently close to those defined in the ion table.
    /// Typically a solution is one where all CL errors are less than 1%.
    /// </summary>
    public PhiEquationSolution Calculate(double[] phis)
    {
        double[] originalCLs = Enumerable.Range(0, Ions.Length)
            .Select(x => Ions[x].CL)
            .ToArray();

        (double ljpVolts, double[] solvedCLs) = LjpCalculator.CalculateLjp(Ions, phis, TemperatureC);

        double[] differences = Enumerable.Range(0, Ions.Length)
            .Select(i => solvedCLs[i] - originalCLs[i])
            .ToArray();

        // Next we must normalize differences to a base value. Typically this is the absolute value of the original CL.
        // However, if original CL is extremely small or zero, normalizing to it will produce a huge or undefined error. 

        // JLJP originally solved this by instead normalizing to the smallest non-zero original CL. I found this problematic
        // still because some original CLs may be extremely small, so calculated error remains very large.
        double smallestNonZeroCl = Ions.Select(x => x.CL).Where(CL => CL > 0).Min();

        // I prefer to normalize to a percentage of total CL
        double onePercentTotalCL = Ions.Select(x => x.CL).Sum() * .01;

        double[] divisors = Enumerable.Range(0, Ions.Length)
            .Select(i => Math.Max(originalCLs[i], onePercentTotalCL))
            .ToArray();

        double[] percentErrorCL = Enumerable.Range(0, EquationCount)
            .Select(i => 100 * differences[i] / divisors[i])
            .ToArray();

        return new PhiEquationSolution(phis, solvedCLs, percentErrorCL, ljpVolts);
    }
}
