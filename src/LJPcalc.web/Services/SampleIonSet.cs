using LJPcalc.web.InputModels;
using LJPmath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJPcalc.web.Services
{
    public class SampleIonSet
    {
        public List<UserIon> Ions;
        public string Title;
        public string Description;

        private readonly IonLookup KnownIons = new IonLookup();

        public SampleIonSet PotassiumGluconate => new SampleIonSet()
        {
            Title = "K-Glu",
            Description = "Potassium Gluconate internal solution based on [ref?]",
            Ions = new List<UserIon>
            {
                UserIon.FromIon(KnownIons.GetIon("Ca", 0, 2.4)),
                UserIon.FromIon(KnownIons.GetIon("Cl", 2, 133.8)),
                UserIon.FromIon(KnownIons.GetIon("Gluconate", 125, 0)),
                UserIon.FromIon(KnownIons.GetIon("H2PO4", 0, 1.2)),
                UserIon.FromIon(KnownIons.GetIon("HCO3", 0, 25)),
                UserIon.FromIon(KnownIons.GetIon("HEPES", 10, 0)),
                UserIon.FromIon(KnownIons.GetIon("K", 125, 3)),
                UserIon.FromIon(KnownIons.GetIon("Mg", 1, 1.5)),
                UserIon.FromIon(KnownIons.GetIon("SO4", 0, 1.5)),
                UserIon.FromIon(KnownIons.GetIon("Na", 4.25, 152.2)),
            }
        };
    }
}
