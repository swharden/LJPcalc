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

        private readonly double conc1_M;
        private readonly double conc2_M;
        private readonly int charge;
        private readonly double mu;
        private readonly string symbol;

        public Ion(string symbol, double conc1_M, double conc2_M, double? charge = null, double? mu = null, bool attemptLookup = true)
        {
            this.conc1_M = conc1_M;
            this.conc2_M = conc2_M;
            this.symbol = symbol;

            if (attemptLookup)
            {
                var knownIon = IonTable.Lookup(symbol);
                if (charge is null)
                    charge = knownIon.charge;
                if (mu is null)
                    mu = knownIon.mu;
            }

            this.charge = (int)charge;
            this.mu = (double)mu;
        }

        public override string ToString()
        {
            return ($"Ion {symbol}: charge = {charge}, mu = {mu}, c1 = {conc1_M} M, c2 = {conc2_M} M");
        }
    }
}
