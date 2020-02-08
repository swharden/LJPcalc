using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LJPconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // reproduce the LJP in the JLJP screenshot
            var ionList = new List<LJPmath.Ion>
            {
                new LJPmath.Ion("Zn", 9, 0.0284),
                new LJPmath.Ion("K", 0, 3),             // X
                new LJPmath.Ion("Cl", 18, 3.0568)       // Last
            };

            // solve using the stationary Nernst-Planck equation
            var result = LJPmath.Calculate.NernstPlank(ionList);
            Debug.WriteLine(result);

            // compare the result
            double expected_mV = -20.823125;
            Debug.WriteLine($"Calculated {result.ljp_mV} mV (expected {expected_mV} mV)");
        }
    }
}
