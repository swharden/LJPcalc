using System;
using System.Collections.Generic;
using System.Text;

namespace LJPmath
{
    public class Ion
    {
        public string name { get; private set; } = "?";
        public int charge { get; private set; } = 0;
        public double conductivity { get; private set; } = 0;
        public double mu { get; private set; } = 0;
        public double muE11 { get { return mu / 1e11; } }
        public double cdadc { get; } = 1.0;

        public double c0 { get; set; } = 0;
        public double cL { get; set; } = 0;
        public double cDiff { get { return Math.Abs(cL - c0); } }

        public double phi { get; set; } = 0;

        public string chargeWithSign { get { return (charge > 0) ? "+" + charge.ToString() : charge.ToString(); } }
        public string nameWithCharge { get { return $"{name} ({chargeWithSign})"; } }
        public bool isValid { get { return (charge != 0 && mu >= 0 && conductivity > 0 && name != null && name != ""); } }

        public Ion()
        {

        }

        public Ion(Ion ion)
        {
            this.name = ion.name;
            this.charge = ion.charge;
            this.conductivity = ion.conductivity;
            this.mu = ion.mu;
            this.c0 = ion.c0;
            this.cL = ion.cL;
            this.phi = ion.phi;
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

        public Ion(Ion ion, double c0, double cL)
        {
            name = ion.name;
            charge = ion.charge;
            conductivity = ion.conductivity;
            mu = ion.mu;
            this.c0 = c0;
            this.cL = cL;
        }

        public Ion(String name, int charge, double conductivity, double c0, double cL)
        {
            this.name = name;
            this.charge = charge;
            this.conductivity = conductivity;
            mu = (charge == 0) ? 0 : conductivity / (Constants.Nav * Math.Pow(Constants.e, 2) * Math.Abs(charge));
            this.c0 = c0;
            this.cL = cL;
        }

        public override string ToString()
        {
            return $"Ion {nameWithCharge}: mu={mu:0.000E+0}, c0={c0:0.000}, cL={cL:0.000}";
        }
    }
}
