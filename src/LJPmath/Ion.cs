using System;
using System.Collections.Generic;
using System.Text;

namespace LJPmath
{
    public class Ion
    {
        private const double Nav = 6.02214076e23; // Avogadro's constant (no units)

        public Ion(string symbol, double conc1_M, double conc2_M)
        {
            double particleCount1 = conc1_M * Nav;
            double particleCount2 = conc2_M * Nav;
        }
    }
}
