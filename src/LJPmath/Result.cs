using System;

namespace LJPmath
{
    public class Result
    {
        public double ljp_V = 0;
        public double ljp_mV { get { return ljp_V * 1000; } }
    }
}
