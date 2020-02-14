using System;
using System.Collections.Generic;
using System.Text;

namespace LJPmath
{
    public class Ion
    {
        // ion properies are immutable
        public readonly string name;
        public readonly string description;
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

        public Ion(String name, String description, int charge, double conductance)
        {
            this.name = name;
            this.description = description;
            this.charge = charge;
            this.conductance = conductance;
            mu = conductance / (Constants.Nav * Math.Pow(Constants.e, 2) * Math.Abs(charge));
        }

        public override string ToString()
        {
            string longName = (name == description) ? description : $"{description} ({name})";
            return $"Ion {longName}: mu={mu}, phi={phi}, c0={c0}, cL={cL}";
        }
    }
}
