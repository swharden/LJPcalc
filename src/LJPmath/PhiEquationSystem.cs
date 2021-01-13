using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LJPmath
{
    class PhiEquationSystem : IEquationSystem
    {
        public int Count { get; private set; }
        private readonly double TemperatureC;

        private readonly List<Ion> Ions;
        private readonly int IonCount;

        public PhiEquationSystem(List<Ion> ions, double temperatureC)
        {
            Ions = ions;
            TemperatureC = temperatureC;
            IonCount = ions.Count;
            Count = Ions.Count - 2;
        }

        /// <summary>
        /// Determine CL (f) for the given phis (x)
        /// </summary>
        /// <param name="x">phis</param>
        /// <param name="f">scaled difference between expected CL and actual CL</param>
        public void Equations(double[] x, double[] f)
        {
            Calculate.SolveForLJP(Ions, startingPhis: x, CLs: f, TemperatureC);

            // Sigma is a scaling factor later used to scale equation error.
            // Ions with larger CLs have larger sigmas.
            double smallestAbsoluteNonZeroCL = Ions.Select(ion => Math.Abs(ion.cL))
                                                   .Where(c => c > 0)
                                                   .Min();

            for (int i = 0; i < IonCount - 2; i++)
            {
                double expectedCL = f[i];
                double actualCL = Ions[i].cL;
                double errorCL = expectedCL - actualCL;
                double sigma = Math.Max(Math.Abs(Ions[i].cL), smallestAbsoluteNonZeroCL) / 100;
                f[i] = errorCL / sigma;
            }
        }
    }
}
