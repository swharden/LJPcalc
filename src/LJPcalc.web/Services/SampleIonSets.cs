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

        public static SampleIonSet PotassiumGluconate => new SampleIonSet()
        {
            Title = "K-Glu",
            Description = "Potassium Gluconate internal solution based on [ref?]",
            Ions = new List<Ion>
            {
                IonTable.GetIon("Ca", 0, 2.4),
                IonTable.GetIon("Cl", 2, 133.8),
                IonTable.GetIon("Gluconate", 125, 0),
                IonTable.GetIon("H2PO4", 0, 1.2),
                IonTable.GetIon("HCO3", 0, 25),
                IonTable.GetIon("HEPES", 10, 0),
                IonTable.GetIon("K", 125, 3),
                IonTable.GetIon("Mg", 1, 1.5),
                IonTable.GetIon("SO4", 0, 1.5),
                IonTable.GetIon("Na", 4.25, 152.2),
            }
        };
    }
}
