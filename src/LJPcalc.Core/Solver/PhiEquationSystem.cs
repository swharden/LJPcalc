﻿namespace LJPcalc.Core.Solver;

class PhiEquationSystem : IEquationSystem
{
    public int EquationCount { get; private set; }
    private readonly double TemperatureC;
    private readonly Ion[] Ions;

    public PhiEquationSystem(Ion[] ions, double temperatureC)
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
    public double[] Calculate(double[] phis)
    {
        double[] originalCLs = Enumerable.Range(0, Ions.Length)
            .Select(x => Ions[x].CL)
            .ToArray();

        (_, double[] ljpCLs) = Core.Calculate.SolveForLJP(Ions, phis, TemperatureC);

        double[] differences = Enumerable.Range(0, Ions.Length)
            .Select(i => ljpCLs[i] - originalCLs[i])
            .ToArray();

        /* Next we must normalize differences to a base value. Typically this is the absolute value of the original CL.
         * 
         * However, if original CL is extremely small or zero, normalizing to it will produce a huge or undefined error.
         * 
         * JLJP originally solved this by instead normalizing to the smallest non-zero original CL. I found this problematic
         * still because some original CLs may be extremely small, so calculated error remains very large.
         * The solution to normalize to a percentage of total CL seems to work.
         */
        double onePercentTotalCL = Ions.Select(x => x.CL).Sum() * .01;

        double[] divisors = Enumerable.Range(0, Ions.Length)
            .Select(i => Math.Max(originalCLs[i], onePercentTotalCL))
            .ToArray();

        double[] percentErrorCL = Enumerable.Range(0, EquationCount)
            .Select(i => 100 * differences[i] / divisors[i])
            .ToArray();

        return percentErrorCL;
    }
}
