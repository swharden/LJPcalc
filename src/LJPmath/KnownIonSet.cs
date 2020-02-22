using System;
using System.Collections.Generic;
using System.Text;

namespace LJPmath
{
    public class KnownIonSet
    {
        public readonly string name;
        public readonly string details;
        public readonly double expectedLjp_mV;
        public readonly double expectedAccuracy_mV;
        public readonly double temperatureC;
        public readonly List<Ion> ions;

        public KnownIonSet(string name, string details, double expectedLjp_mV, double expectedAccuracy_mV, List<Ion> ions, double temperatureC, IonTable ionTable)
        {
            this.name = name;
            this.details = details;
            this.expectedLjp_mV = expectedLjp_mV;
            this.expectedAccuracy_mV = expectedAccuracy_mV;
            this.ions = ionTable.Lookup(ions);
            this.temperatureC = temperatureC;
        }
    }
}
