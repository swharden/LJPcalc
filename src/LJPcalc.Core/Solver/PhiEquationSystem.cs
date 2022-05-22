namespace LJPcalc.Core.Solver;

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
    /// Determine scaled CL (f) for the given phis (x).
    /// The solution is found when CLs are suffeciently close to those defined in the ion table.
    /// A valid solution is a set of phis (xs) where for every x, f(x) is between -1 and 1.
    /// </summary>
    public double[] Calculate(double[] phis)
    {
        // solve with zero CL concentration
        (double ljp, double[] solvedCLs) = Core.Calculate.SolveForLJP(Ions, phis, TemperatureC);

        // Sigma is a scaling factor later used to scale the difference between 
        // the expected CL and the CL determined using the custom phis (x).
        double smallestAbsoluteNonZeroCL = Ions.Select(ion => Math.Abs(ion.CL))
                                               .Where(c => c > 0)
                                               .Min();

        // return scaled difference between expected CL and actual CL given phis (x)
        double[] scaledDifference = new double[EquationCount];
        for (int i = 0; i < EquationCount; i++)
        {
            double originalCL = Ions[i].CL;
            double differenceCL = solvedCLs[i] - originalCL;
            double absoluteCL = Math.Max(Math.Abs(originalCL), smallestAbsoluteNonZeroCL);
            scaledDifference[i] = 100 * differenceCL / absoluteCL;
        }

        return scaledDifference;
    }
}
