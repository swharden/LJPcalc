using System;
using System.Collections.Generic;
using System.Text;

namespace LJPmath
{
    public class Ion
    {
        // ion properies are immutable
        public readonly string name;
        public readonly int charge;
        public readonly double conductance;
        public readonly double mu;
        public const double cdadc = 1.0;

        // only concentration is mutable
        public double c0;
        public double cL;
        public double phi;

        public Ion()
        {

        }

        public Ion(String name)
        {
            this.name = name;
        }

        public Ion(String name, double c0, double cL)
        {
            this.name = name;
            this.c0 = c0;
            this.cL = cL;
        }

        public Ion(String name, int charge, double conductance, double c0, double cL)
        {
            this.name = name;
            this.charge = charge;
            this.conductance = conductance;
            mu = conductance / (Constants.Nav * Math.Pow(Constants.e, 2) * Math.Abs(charge));
            this.c0 = c0;
            this.cL = cL;
        }

        public override string ToString()
        {
            return $"Ion {name}: mu={mu:0.000E+0}, phi={phi}, c0={c0}, cL={cL}";
        }
    }
}
