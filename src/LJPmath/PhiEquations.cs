using System;
using System.Collections.Generic;
using System.Text;

namespace LJPmath
{
    class PhiEquations : IEquationSystem
    {
        private List<Ion> list;
        private int ionCount;
        private double[] sigma;
        private double temperatureC;

        public PhiEquations(List<Ion> list, double temperatureC)
        {
            this.list = list;
            ionCount = list.Count;

            this.temperatureC = temperatureC;

            // determine smallest nonzero cL of all ions
            double smallestCL = Double.PositiveInfinity;
            for (int j = 0; j < ionCount; j++)
            {
                Ion ion = list[j];
                bool ionHasCL = (ion.cL > 0);
                if (ionHasCL)
                {
                    double thisCl = Math.Abs(ion.cL);
                    smallestCL = Math.Min(smallestCL, thisCl);
                }
            }

            // set sigmas to cLs (unless cL is zero, then use smallest nonzero cL)
            sigma = new double[ionCount];
            for (int j = 0; j < ionCount; j++)
            {
                Ion ion = list[j];
                bool ionHasCL = (ion.cL > 0);
                if (ionHasCL)
                    sigma[j] = 0.01 * Math.Abs(ion.cL);
                else
                    sigma[j] = 0.01 * smallestCL;
            }
        }

        public void equations(double[] x, double[] f)
        {
            Calculate.Ljp(list, x, f, temperatureC);
            for (int j = 0; j < ionCount - 2; j++)
            {
                Ion ion = list[j];
                f[j] = (f[j] - ion.cL) / sigma[j];
            }
        }

        public int number()
        {
            return list.Count - 2;
        }
    }
}
