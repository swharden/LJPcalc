using NUnit.Framework;
using LJPcalc.Core;

namespace LJPcalc.Test;

[TestFixture]
class IonTableTest
{
    [Test]
    public void Test_NoFile_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new IonTable("badFileName.md"));
    }

    [Test]
    public void Test_EmptyConstructor_IsFine()
    {
        var ionTable = new IonTable();
        Console.WriteLine(ionTable);
    }

    [Test]
    public void Test_ConstructorFilename_IsFine()
    {
        var ionTable = new IonTable("IonTable.md");
        Console.WriteLine(ionTable);
    }

    [Test]
    public void Test_Lookup_KnownIon()
    {
        var ionTable = new IonTable();
        var ion = ionTable.Lookup("glutamate");
        Console.WriteLine(ion);
        Assert.AreEqual(-1, ion.charge);
    }

    [Test]
    public void Test_Lookup_IsCaseInsensitive()
    {
        var ionTable = new IonTable();
        var ion = ionTable.Lookup("gLuTaMaTe");
        Console.WriteLine(ion);
        Assert.AreEqual(-1, ion.charge);
    }

    [Test]
    public void Test_Lookup_ByDescription()
    {
        var ionTable = new IonTable();
        var ion = ionTable.Lookup("Glutamate");
        Console.WriteLine(ion);
        Assert.AreEqual(-1, ion.charge);
    }

    [Test]
    public void Test_Lookup_FailsWithoutCrashing()
    {
        var ionTable = new IonTable();
        var ion = ionTable.Lookup("adfasdfasdfasdf");
        Console.WriteLine(ion);
        Assert.AreEqual(0, ion.charge);
    }

    struct JljpIon
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

        List<JljpIon> jljpIons = new List<JljpIon>
        {
            new JljpIon() { name = "Ag", charge = 1, mu = 401656000000 },
            new JljpIon() { name = "Al", charge = 3, mu = 131939000000 },
            new JljpIon() { name = "Au(CN)2", charge = -1, mu = 324439000000 },
            new JljpIon() { name = "Au(CN)4", charge = -1, mu = 233596000000 },
            new JljpIon() { name = "Ba", charge = 2, mu = 206343000000 },
            new JljpIon() { name = "B(C6H5)4", charge = -1, mu = 136265000000 },
            new JljpIon() { name = "Be", charge = 2, mu = 145998000000 },
            new JljpIon() { name = "Br3", charge = -1, mu = 279018000000 },
            new JljpIon() { name = "Br", charge = -1, mu = 506774000000 },
            new JljpIon() { name = "BrO3", charge = -1, mu = 361425000000 },
            new JljpIon() { name = "Ca", charge = 2, mu = 192944000000 },
            new JljpIon() { name = "Cd", charge = 2, mu = 175197000000 },
            new JljpIon() { name = "Ce", charge = 3, mu = 150972000000 },
            new JljpIon() { name = "Cl", charge = -1, mu = 495159000000 },
            new JljpIon() { name = "ClO2", charge = -1, mu = 337417000000 },
            new JljpIon() { name = "ClO3", charge = -1, mu = 419176000000 },
            new JljpIon() { name = "ClO4", charge = -1, mu = 436695000000 },
            new JljpIon() { name = "CN", charge = -1, mu = 506125000000 },
            new JljpIon() { name = "CNO", charge = -1, mu = 419176000000 },
            new JljpIon() { name = "CO3", charge = -2, mu = 224836000000 },
            new JljpIon() { name = "Co", charge = 2, mu = 178442000000 },
            new JljpIon() { name = "Co(CN)6", charge = -3, mu = 213914000000 },
            new JljpIon() { name = "Co(NH3)6", charge = 3, mu = 220402000000 },
            new JljpIon() { name = "Cr", charge = 3, mu = 144916000000 },
            new JljpIon() { name = "CrO4", charge = -2, mu = 275773000000 },
            new JljpIon() { name = "Cs", charge = 1, mu = 500934000000 },
            new JljpIon() { name = "Cu", charge = 2, mu = 173900000000 },
            //new JljpIon() { name = "D", charge = 1, mu = 1621550000000 }, // REF DOESNT HAV 25C
            new JljpIon() { name = "Dy", charge = 3, mu = 141888000000 },
            new JljpIon() { name = "Er", charge = 3, mu = 142537000000 },
            new JljpIon() { name = "Eu", charge = 3, mu = 146647000000 },
            new JljpIon() { name = "F", charge = -1, mu = 359479000000 },
            new JljpIon() { name = "FeII", charge = 2, mu = 175197000000 },
            new JljpIon() { name = "FeIII", charge = 3, mu = 147079000000 },
            new JljpIon() { name = "[Fe(CN)6]III", charge = -3, mu = 218240000000 },
            new JljpIon() { name = "[Fe(CN)6]IIII", charge = -4, mu = 179091000000 },
            new JljpIon() { name = "Gd", charge = 3, mu = 145565000000 },
            new JljpIon() { name = "H2AsO4", charge = -1, mu = 220619000000 },
            new JljpIon() { name = "H2PO2", charge = -1, mu = 298484000000 },
            // new JljpIon() { name = "H2PO4", charge = -1, mu = 233596000000 }, // INACCURATE?
            new JljpIon() { name = "H2SbO4", charge = -1, mu = 201152000000 },
            new JljpIon() { name = "H", charge = 1, mu = 2268800000000 },
            new JljpIon() { name = "HCO3", charge = -1, mu = 288751000000 },
            new JljpIon() { name = "HF2", charge = -1, mu = 486659000000 },
            //new JljpIon() { name = "Hg", charge = 2, mu = 222565000000 }, // INACCURATE?
            new JljpIon() { name = "Ho", charge = 3, mu = 143402000000 },
            //new JljpIon() { name = "HPO4", charge = -2, mu = 184930000000 }, // INACCURATE?
            new JljpIon() { name = "HS", charge = -1, mu = 421771000000 },
            //new JljpIon() { name = "HSO3", charge = -1, mu = 376350000000 }, // INACCURATE?
            //new JljpIon() { name = "HSO4", charge = -1, mu = 337417000000 }, // INACCURATE?
            new JljpIon() { name = "I", charge = -1, mu = 498339000000 },
            new JljpIon() { name = "IO3", charge = -1, mu = 262796000000 },
            new JljpIon() { name = "IO4", charge = -1, mu = 353639000000 },
            new JljpIon() { name = "K", charge = 1, mu = 476796000000 },
            new JljpIon() { name = "La", charge = 3, mu = 150756000000 },
            new JljpIon() { name = "Li", charge = 1, mu = 250857000000 },
            new JljpIon() { name = "Mg", charge = 2, mu = 171953000000 },
            new JljpIon() { name = "Mn", charge = 2, mu = 173575000000 },
            new JljpIon() { name = "MnO4", charge = -1, mu = 397763000000 },
            new JljpIon() { name = "MoO4", charge = -2, mu = 241707000000 },
            new JljpIon() { name = "N2H5", charge = 1, mu = 382838000000 },
            new JljpIon() { name = "N3", charge = -1, mu = 447726000000 },
            new JljpIon() { name = "Na", charge = 1, mu = 324958000000 },
            new JljpIon() { name = "N(CN)2", charge = -1, mu = 353639000000 },
            new JljpIon() { name = "Nd", charge = 3, mu = 150107000000 },
            new JljpIon() { name = "NH2SO3", charge = -1, mu = 313408000000 },
            new JljpIon() { name = "NH4", charge = 1, mu = 476926000000 },
            new JljpIon() { name = "Ni", charge = 2, mu = 160922000000 },
            new JljpIon() { name = "NO2", charge = -1, mu = 465895000000 },
            new JljpIon() { name = "NO3", charge = -1, mu = 463429000000 },
            new JljpIon() { name = "OCN(cyanate)", charge = -1, mu = 419176000000 },
            new JljpIon() { name = "OD", charge = -1, mu = 772166000000 },
            new JljpIon() { name = "OH", charge = -1, mu = 1284780000000 },
            new JljpIon() { name = "Pb", charge = 2, mu = 230352000000 },
            new JljpIon() { name = "PF6", charge = -1, mu = 369212000000 },
            new JljpIon() { name = "PO3F", charge = -2, mu = 205370000000 },
            //new JljpIon() { name = "PO4", charge = -3, mu = 200720000000 }, // INACCURATE?
            new JljpIon() { name = "Pr", charge = 3, mu = 150324000000 },
            new JljpIon() { name = "Ra", charge = 2, mu = 216725000000 },
            new JljpIon() { name = "Rb", charge = 1, mu = 504828000000 },
            new JljpIon() { name = "ReO4", charge = -1, mu = 356234000000 },
            new JljpIon() { name = "Sb(OH)6", charge = -1, mu = 206992000000 },
            new JljpIon() { name = "Sc", charge = 3, mu = 139942000000 },
            new JljpIon() { name = "SCN(thiocyanate)", charge = -1, mu = 428260000000 },
            new JljpIon() { name = "SeCN", charge = -1, mu = 419825000000 },
            new JljpIon() { name = "SeO4", charge = -2, mu = 245601000000 },
            new JljpIon() { name = "Sm", charge = 3, mu = 148161000000 },
            //new JljpIon() { name = "SO3", charge = -2, mu = 233596000000 }, // INACCURATE?
            new JljpIon() { name = "SO4", charge = -2, mu = 259551000000 },
            new JljpIon() { name = "Sr", charge = 2, mu = 192717000000 },
            new JljpIon() { name = "Tl", charge = 1, mu = 484712000000 },
            new JljpIon() { name = "Tm", charge = 3, mu = 141456000000 },
            new JljpIon() { name = "UO2", charge = 2, mu = 103821000000 },
            new JljpIon() { name = "WO4", charge = -2, mu = 223863000000 },
            new JljpIon() { name = "Yb", charge = 3, mu = 141888000000 },
            new JljpIon() { name = "Y", charge = 3, mu = 134102000000 },
            new JljpIon() { name = "Zn", charge = 2, mu = 171304000000 }
        };

        var ionTable = new IonTable();

        foreach (var jljpIon in jljpIons)
        {
            if (ionTable.Contains(jljpIon.name))
            {
                var ion = ionTable.Lookup(jljpIon.name);
                Assert.That(ion.charge == jljpIon.charge);

                double muDifference = Math.Abs(ion.mu - jljpIon.mu);
                double muDifferencePercent = Math.Round(muDifference / ion.mu * 100.0, 3);
                double allowableDifferencePercent = 10;
                Assert.That(muDifferencePercent < allowableDifferencePercent);
            }
            else
            {
                Console.WriteLine($"JLJP contains ion not in LJPcalc table: {jljpIon.name}");
            }
        }
    }
}
