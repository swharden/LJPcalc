using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LJPmath
{
    public static class Calculate
    {
        public static Result NernstPlank(List<Ion> ionList)
        {

            Debug.WriteLine($"calculating LJP using the stationary Nernst-Planck equation for {ionList.Count} ions...");

            foreach (Ion ion in ionList)
                Debug.WriteLine(ion);

            // get starting phis for each ion
            double[] phis = new double[ionList.Count - 2];
            for (int j = 0; j < ionList.Count - 2; j++)
                phis[j] = ionList[j].conc2_M - ionList[j].conc1_M;

            // TODO: this is where I stopped working...

            // done
            var result = new Result();
            return result;
        }

        [Obsolete("Henderson method is not supported", true)]
        public static Result Henderson()
        {
            var result = new Result();
            return result;
        }
    }
}
