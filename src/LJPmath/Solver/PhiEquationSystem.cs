using System;
using System.Collections.Generic;
using System.Linq;

namespace LJPmath.Solver
{
    class PhiEquationSystem : Solver.IEquationSystem
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
            LJPmath.Calculate.SolveForLJP(Ions, startingPhis: x, CLs: f, TemperatureC);

            // Sigma is a scaling factor later used to scale the difference between 
            // the expected CL and the CL determined using the custom phis (x).
            double smallestAbsoluteNonZeroCL = Ions.Select(ion => Math.Abs(ion.cL))
                                                   .Where(c => c > 0)
                                                   .Min();

            for (int i = 0; i < EquationCount; i++)
            {
                double newCL = f[i];
                double originalCL = Ions[i].cL;
                double differenceCL = newCL - originalCL;
                double absoluteCL = Math.Max(Math.Abs(Ions[i].cL), smallestAbsoluteNonZeroCL);
                f[i] = 100 * differenceCL / absoluteCL;
            }
        }
    }
}
