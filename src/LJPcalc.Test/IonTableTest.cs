using NUnit.Framework;
using LJPcalc.Core;

namespace LJPcalc.Test;

[TestFixture]
class IonTableTest
{
    [Test]
    public void Test_Lookup_KnownIon()
    {
        Ion ion = IonLibrary.Lookup("glutamate");
        Assert.That(ion.Charge, Is.EqualTo(-1));
    }

    [Test]
    public void Test_Lookup_IsCaseInsensitive()
    {
        Ion ion = IonLibrary.Lookup("gLuTaMaTe");
        Assert.That(ion.Charge, Is.EqualTo(-1));
    }

    [Test]
    public void Test_Lookup_ByDescription()
    {
        Ion ion = IonLibrary.Lookup("Glutamate");
        Assert.That(ion.Charge, Is.EqualTo(-1));
    }

    [Test]
    public void Test_Lookup_FailsWithoutCrashing()
    {
        Ion ion = IonLibrary.Lookup("adfasdfasdfasdf");
        Assert.That(ion.Charge, Is.EqualTo(0));
    }

    struct ionJLJP
    {
        public string name;
        public int charge;
        public double mu;
    }

    [Test]
    public void Test_CompareValues_ToJLJP()
    {
        // ion charges and mus came from the JLJP project source code
        // https://github.com/swharden/JLJP/blob/b585ab99d78f39960f85b17988374f3445b7dadb/src/Ion.java#L29-L125

        List<ionJLJP> ionsFromJLJP = new()
        {
            new ionJLJP() { name = "Ag", charge = 1, mu = 401656000000 },
            new ionJLJP() { name = "Al", charge = 3, mu = 131939000000 },
            new ionJLJP() { name = "Au(CN)2", charge = -1, mu = 324439000000 },
            new ionJLJP() { name = "Au(CN)4", charge = -1, mu = 233596000000 },
            new ionJLJP() { name = "Ba", charge = 2, mu = 206343000000 },
            new ionJLJP() { name = "B(C6H5)4", charge = -1, mu = 136265000000 },
            new ionJLJP() { name = "Be", charge = 2, mu = 145998000000 },
            new ionJLJP() { name = "Br3", charge = -1, mu = 279018000000 },
            new ionJLJP() { name = "Br", charge = -1, mu = 506774000000 },
            new ionJLJP() { name = "BrO3", charge = -1, mu = 361425000000 },
            new ionJLJP() { name = "Ca", charge = 2, mu = 192944000000 },
            new ionJLJP() { name = "Cd", charge = 2, mu = 175197000000 },
            new ionJLJP() { name = "Ce", charge = 3, mu = 150972000000 },
            new ionJLJP() { name = "Cl", charge = -1, mu = 495159000000 },
            new ionJLJP() { name = "ClO2", charge = -1, mu = 337417000000 },
            new ionJLJP() { name = "ClO3", charge = -1, mu = 419176000000 },
            new ionJLJP() { name = "ClO4", charge = -1, mu = 436695000000 },
            new ionJLJP() { name = "CN", charge = -1, mu = 506125000000 },
            new ionJLJP() { name = "CNO", charge = -1, mu = 419176000000 },
            new ionJLJP() { name = "CO3", charge = -2, mu = 224836000000 },
            new ionJLJP() { name = "Co", charge = 2, mu = 178442000000 },
            new ionJLJP() { name = "Co(CN)6", charge = -3, mu = 213914000000 },
            new ionJLJP() { name = "Co(NH3)6", charge = 3, mu = 220402000000 },
            new ionJLJP() { name = "Cr", charge = 3, mu = 144916000000 },
            new ionJLJP() { name = "CrO4", charge = -2, mu = 275773000000 },
            new ionJLJP() { name = "Cs", charge = 1, mu = 500934000000 },
            new ionJLJP() { name = "Cu", charge = 2, mu = 173900000000 },
            //new JljpIon() { name = "D", charge = 1, mu = 1621550000000 }, // REF DOESNT HAV 25C
            new ionJLJP() { name = "Dy", charge = 3, mu = 141888000000 },
            new ionJLJP() { name = "Er", charge = 3, mu = 142537000000 },
            new ionJLJP() { name = "Eu", charge = 3, mu = 146647000000 },
            new ionJLJP() { name = "F", charge = -1, mu = 359479000000 },
            new ionJLJP() { name = "FeII", charge = 2, mu = 175197000000 },
            new ionJLJP() { name = "FeIII", charge = 3, mu = 147079000000 },
            new ionJLJP() { name = "Fe(CN)6 III", charge = -3, mu = 218240000000 },
            new ionJLJP() { name = "Fe(CN)6 IIII", charge = -4, mu = 179091000000 },
            new ionJLJP() { name = "Gd", charge = 3, mu = 145565000000 },
            new ionJLJP() { name = "H2AsO4", charge = -1, mu = 220619000000 },
            // new ionJLJP() { name = "H2PO2", charge = -1, mu = 298484000000 },
            // new JljpIon() { name = "H2PO4", charge = -1, mu = 233596000000 }, // INACCURATE?
            new ionJLJP() { name = "H2SbO4", charge = -1, mu = 201152000000 },
            new ionJLJP() { name = "H", charge = 1, mu = 2268800000000 },
            new ionJLJP() { name = "HCO3", charge = -1, mu = 288751000000 },
            new ionJLJP() { name = "HF2", charge = -1, mu = 486659000000 },
            //new JljpIon() { name = "Hg", charge = 2, mu = 222565000000 }, // INACCURATE?
            new ionJLJP() { name = "Ho", charge = 3, mu = 143402000000 },
            //new JljpIon() { name = "HPO4", charge = -2, mu = 184930000000 }, // INACCURATE?
            new ionJLJP() { name = "HS", charge = -1, mu = 421771000000 },
            //new JljpIon() { name = "HSO3", charge = -1, mu = 376350000000 }, // INACCURATE?
            //new JljpIon() { name = "HSO4", charge = -1, mu = 337417000000 }, // INACCURATE?
            new ionJLJP() { name = "I", charge = -1, mu = 498339000000 },
            new ionJLJP() { name = "IO3", charge = -1, mu = 262796000000 },
            new ionJLJP() { name = "IO4", charge = -1, mu = 353639000000 },
            new ionJLJP() { name = "K", charge = 1, mu = 476796000000 },
            new ionJLJP() { name = "La", charge = 3, mu = 150756000000 },
            new ionJLJP() { name = "Li", charge = 1, mu = 250857000000 },
            new ionJLJP() { name = "Mg", charge = 2, mu = 171953000000 },
            new ionJLJP() { name = "Mn", charge = 2, mu = 173575000000 },
            new ionJLJP() { name = "MnO4", charge = -1, mu = 397763000000 },
            new ionJLJP() { name = "MoO4", charge = -2, mu = 241707000000 },
            new ionJLJP() { name = "N2H5", charge = 1, mu = 382838000000 },
            new ionJLJP() { name = "N3", charge = -1, mu = 447726000000 },
            new ionJLJP() { name = "Na", charge = 1, mu = 324958000000 },
            new ionJLJP() { name = "N(CN)2", charge = -1, mu = 353639000000 },
            new ionJLJP() { name = "Nd", charge = 3, mu = 150107000000 },
            new ionJLJP() { name = "NH2SO3", charge = -1, mu = 313408000000 },
            new ionJLJP() { name = "NH4", charge = 1, mu = 476926000000 },
            new ionJLJP() { name = "Ni", charge = 2, mu = 160922000000 },
            new ionJLJP() { name = "NO2", charge = -1, mu = 465895000000 },
            new ionJLJP() { name = "NO3", charge = -1, mu = 463429000000 },
            new ionJLJP() { name = "OCN (cyanate)", charge = -1, mu = 419176000000 },
            //new ionJLJP() { name = "OD", charge = -1, mu = 772166000000 },
            new ionJLJP() { name = "OH", charge = -1, mu = 1284780000000 },
            new ionJLJP() { name = "Pb", charge = 2, mu = 230352000000 },
            new ionJLJP() { name = "PF6", charge = -1, mu = 369212000000 },
            new ionJLJP() { name = "PO3F", charge = -2, mu = 205370000000 },
            //new JljpIon() { name = "PO4", charge = -3, mu = 200720000000 }, // INACCURATE?
            new ionJLJP() { name = "Pr", charge = 3, mu = 150324000000 },
            new ionJLJP() { name = "Ra", charge = 2, mu = 216725000000 },
            new ionJLJP() { name = "Rb", charge = 1, mu = 504828000000 },
            new ionJLJP() { name = "ReO4", charge = -1, mu = 356234000000 },
            //new ionJLJP() { name = "Sb(OH)6", charge = -1, mu = 206992000000 },
            new ionJLJP() { name = "Sc", charge = 3, mu = 139942000000 },
            //new ionJLJP() { name = "SCN (thiocyanate)", charge = -1, mu = 428260000000 },
            new ionJLJP() { name = "SeCN", charge = -1, mu = 419825000000 },
            new ionJLJP() { name = "SeO4", charge = -2, mu = 245601000000 },
            new ionJLJP() { name = "Sm", charge = 3, mu = 148161000000 },
            //new JljpIon() { name = "SO3", charge = -2, mu = 233596000000 }, // INACCURATE?
            new ionJLJP() { name = "SO4", charge = -2, mu = 259551000000 },
            new ionJLJP() { name = "Sr", charge = 2, mu = 192717000000 },
            new ionJLJP() { name = "Tl", charge = 1, mu = 484712000000 },
            new ionJLJP() { name = "Tm", charge = 3, mu = 141456000000 },
            new ionJLJP() { name = "UO2", charge = 2, mu = 103821000000 },
            new ionJLJP() { name = "WO4", charge = -2, mu = 223863000000 },
            new ionJLJP() { name = "Yb", charge = 3, mu = 141888000000 },
            new ionJLJP() { name = "Y", charge = 3, mu = 134102000000 },
            new ionJLJP() { name = "Zn", charge = 2, mu = 171304000000 }
        };

        string[] libraryIonNames = IonLibrary.KnownIons.Select(x => x.Name).ToArray();

        foreach (ionJLJP ionFromJLJP in ionsFromJLJP)
        {
            Ion ionFromThisLibrary = IonLibrary.Lookup(ionFromJLJP.name);

            Assert.That(ionFromThisLibrary.Charge, Is.EqualTo(ionFromJLJP.charge), ionFromThisLibrary.Name);
            Assert.That(ionFromThisLibrary.Mu * 1e-4, Is.EqualTo(ionFromJLJP.mu).Within(10).Percent);
        }
    }
}
