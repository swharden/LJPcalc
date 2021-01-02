using LJPmath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJPcalc.web.Services
{
    public class SampleIonSet
    {
        public List<Ion> Ions;
        public string Title;
        public string Description;

        private readonly KnownIons KnownIons = new KnownIons();

        public SampleIonSet PotassiumGluconate => new SampleIonSet()
        {
            Title = "K-Glu",
            Description = "Potassium Gluconate internal solution based on [ref?]",
            Ions = new List<Ion>
            {
                KnownIons.GetIon("Ca", 0, 2.4),
                KnownIons.GetIon("Cl", 2, 133.8),
                KnownIons.GetIon("Gluconate", 125, 0),
                KnownIons.GetIon("H2PO4", 0, 1.2),
                KnownIons.GetIon("HCO3", 0, 25),
                KnownIons.GetIon("HEPES", 10, 0),
                KnownIons.GetIon("K", 125, 3),
                KnownIons.GetIon("Mg", 1, 1.5),
                KnownIons.GetIon("SO4", 0, 1.5),
                KnownIons.GetIon("Na", 4.25, 152.2),
            }
        };
    }
}
