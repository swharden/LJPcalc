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
    /// Determine scaled CL (f) for the given phis (x).
    /// </summary>
    /// <param name="x">phis</param>
    /// <param name="f">scaled difference between expected CL and actual CL given phis (x)</param>
    public void Calculate(double[] x, double[] f)
    {
        LJPcalc.Core.Calculate.SolveForLJP(Ions, startingPhis: x, CLs: f, TemperatureC);

        // Sigma is a scaling factor later used to scale the difference between 
        // the expected CL and the CL determined using the custom phis (x).
        double smallestAbsoluteNonZeroCL = Ions.Select(ion => Math.Abs(ion.CL))
                                               .Where(c => c > 0)
                                               .Min();

        for (int i = 0; i < EquationCount; i++)
        {
            double newCL = f[i];
            double originalCL = Ions[i].CL;
            double differenceCL = newCL - originalCL;
            double absoluteCL = Math.Max(Math.Abs(Ions[i].CL), smallestAbsoluteNonZeroCL);
            f[i] = 100 * differenceCL / absoluteCL;
        }
    }
}
