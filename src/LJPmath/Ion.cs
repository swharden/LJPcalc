using System;
using System.Collections.Generic;
using System.Text;

namespace LJPmath
{
    public class Ion
    {
        private const double Nav = 6.02214076e23; // Avogadro's constant (no units)
        private double particleCount1 { get { return conc1_M * Nav; } }
        private double particleCount2 { get { return conc2_M * Nav; } }

        public readonly double conc1_M;
        public readonly double conc2_M;
        public readonly int charge;
        public readonly double mu;
        public readonly string name;

        public Ion(string name, double conc1_M = 0, double conc2_M = 0, double? charge = null, double? mu = null, bool attemptLookup = true)
        {
            this.conc1_M = conc1_M;
            this.conc2_M = conc2_M;
            this.name = name;

            if (attemptLookup)
            {
                var knownIon = IonTable.Lookup(name);
                if (charge is null)
                    charge = knownIon.charge;
                if (mu is null)
                    mu = knownIon.mu;
            }

            if (charge != null)
                this.charge = (int)charge;

            if (mu != null)
                this.mu = (double)mu;

        }

        public override string ToString()
        {
            return ($"Ion {name}: charge = {charge}, mu = {mu}, c1 = {conc1_M} M, c2 = {conc2_M} M");
        }
    }
}
