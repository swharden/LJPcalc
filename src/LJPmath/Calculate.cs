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
