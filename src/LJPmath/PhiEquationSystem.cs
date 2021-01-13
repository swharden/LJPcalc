using System;
using System.Collections.Generic;
using System.Text;

namespace LJPmath
{
    class PhiEquationSystem : IEquationSystem
    {
        public int Count { get; private set; }
        private readonly double TemperatureC;

        private readonly List<Ion> Ions;
        private readonly int IonCount;
        private readonly double[] Sigmas;

        public PhiEquationSystem(List<Ion> ions, double temperatureC)
        {
            Ions = ions;
            TemperatureC = temperatureC;
            IonCount = ions.Count;
            Count = Ions.Count - 2;

            // determine smallest nonzero cL of all ions
            double smallestCL = Double.PositiveInfinity;
            for (int j = 0; j < IonCount; j++)
            {
                Ion ion = ions[j];
                bool ionHasCL = (ion.cL > 0);
                if (ionHasCL)
                {
                    double thisCl = Math.Abs(ion.cL);
                    smallestCL = Math.Min(smallestCL, thisCl);
                }
            }

            // set sigmas to cLs (unless cL is zero, then use smallest nonzero cL)
            Sigmas = new double[IonCount];
            for (int j = 0; j < IonCount; j++)
            {
                Ion ion = ions[j];
                bool ionHasCL = (ion.cL > 0);
                if (ionHasCL)
                    Sigmas[j] = 0.01 * Math.Abs(ion.cL);
                else
                    Sigmas[j] = 0.01 * smallestCL;
            }
        }

        public void Equations(double[] x, double[] f)
        {
            Calculate.SolveForLJP(Ions, startingPhis: x, CLs: f, TemperatureC);

            for (int j = 0; j < IonCount - 2; j++)
                f[j] = (f[j] - Ions[j].cL) / Sigmas[j];
        }
    }
}
