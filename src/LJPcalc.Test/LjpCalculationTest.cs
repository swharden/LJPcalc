using NUnit.Framework;
using LJPcalc.Core;

namespace LJPcalc.Test;

class LjpCalculationTest
{
    [Test]
    public void Test_LjpCalculationMatches_ExampleFromScreenshot()
    {
        /* Test came from screenshot on original JLJP website */

        var ionSet = new Ion[]
        {
            new Ion("Zn", 9, 0.0284),
            new Ion("K", 0, 3),
            new Ion("Cl", 18, 3.0568)
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet, autoSort: false);
        Assert.That(ljp.mV, Is.EqualTo(-20.79558643).Within(1e-6));
    }

    [Test]
    public void Test_LjpMathThrowsExceptionIf_ChargeOrMuIsZero()
    {
        // here the ions aren't looked up from a table so charge and mu is 0
        var ionSet = new Ion[]
        {
            new Ion("Zn", 9, 0.0284),
            new Ion("K", 0, 3),
            new Ion("Cl", 18, 3.0568)
        };

        Assert.Throws<ArgumentException>(() => Calculate.Ljp(ionSet));
    }

    [Test]
    public void Test_LjpMathIsAccurateWhen_ChargeAndConductanceAreSetManually()
    {
        // here the ions aren't looked up from a table but charge and mu are set manually
        var ionSet = new Ion[]
        {
            new Ion("Zn", 2, 52.8, 9, 0.0284),
            new Ion("K", 1, 73.5, 0, 3),
            new Ion("Cl",-1, 76.31, 18, 3.0568)
        };

        var ljp = Calculate.Ljp(ionSet, autoSort: false);
        Assert.That(ljp.mV, Is.EqualTo(-20.79558643).Within(1e-6));
    }

    private double MolsPerCubicMeter(double mM)
    {
        return mM * 1000 * Constants.Nav;
    }

    [Test]
    public void Test_LjpCalculationMatches_ExampleFromSourceCode()
    {
        /* From JLJP: https://github.com/swharden/JLJP/blob/2.0.0/src/Example.java */

        Ion Zn = new Ion("Zn", MolsPerCubicMeter(9), MolsPerCubicMeter(0.0284));
        Ion K = new Ion("K", MolsPerCubicMeter(0), MolsPerCubicMeter(3));
        Ion Cl = new Ion("Cl", MolsPerCubicMeter(18), MolsPerCubicMeter(3.0568));

        var ionSet = new Ion[] { Zn, K, Cl };
        ionSet = IonLibrary.Lookup(ionSet);
        var ljp = Calculate.Ljp(ionSet, autoSort: false);

        Assert.That(ljp.mV, Is.EqualTo(-20.79558643).Within(1e-6));
    }

    [Test]
    public void Test_LjpCalculationMatches_NgAndBarry001()
    {

        /* LJP for this test came from Ng and Barry (1994) Table 2 */

        // 50 mM NaCl : 50 mM KCl
        var ionSet = new Ion[]
        {
            new Ion("Na", 50, 0),
            new Ion("K", 0, 50),
            new Ion("Cl", 50, 50)
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(-4.3).Within(0.5));
    }

    [Test]
    public void Test_LjpCalculationMatches_NgAndBarry002()
    {

        /* LJP for this test came from Ng and Barry (1994) Table 2 */

        // 150 mM NaCl : 150 mM KCl
        // Na (150), Cl (150) : K (150) Cl (150)
        var ionSet = new Ion[]
        {
            new Ion("Na", 150, 0),
            new Ion("K", 0, 150),
            new Ion("Cl", 150, 150)
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(-4.3).Within(0.5));
    }

    [Test]
    public void Test_LjpCalculationMatches_NgAndBarry003()
    {

        /* LJP for this test came from Ng and Barry (1994) Table 2 */

        // 50 NaCl : 50 CsCl
        // Na (50), Cl (50) : Cs (50) Cl (50)
        var ionSet = new Ion[]
        {
            new Ion("Na", 50, 0),
            new Ion("Cs", 0, 50),
            new Ion("Cl", 50, 50)
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(-4.9).Within(0.5));
    }

    [Test]
    public void Test_LjpCalculationMatches_NgAndBarry004()
    {

        /* LJP for this test came from Ng and Barry (1994) Table 2 */

        // 100 NaCl : 100 MgCl2
        // Na (100), Cl (100) : Mg (100) Cl (100)
        var ionSet = new Ion[]
        {
            new Ion("Na", 100, 0),
            new Ion("Mg", 0, 100),
            new Ion("Cl", 100, 200)
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(+10.0).Within(0.5));
    }

    [Test]
    public void Test_LjpCalculationMatches_NgAndBarry005()
    {

        /* LJP for this test came from Ng and Barry (1994) Table 2 */

        // 100 CaCl2 : 100 MgCl2
        // Ca (100), Cl (200) : Mg (100) Cl (200)
        var ionSet = new Ion[]
        {
            new Ion("Ca", 100, 0),
            new Ion("Mg", 0, 100),
            new Ion("Cl", 200, 200)
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(+0.6).Within(0.5));
    }

    [Test]
    public void Test_LjpCalculationMatches_NgAndBarry006()
    {

        /* LJP for this test came from Ng and Barry (1994) Table 2 */

        // 100 KCl + 2 CaCl2 : 100 LiCl + 2 CaCl2
        // K (100), Cl (104), Ca (2) : Li (100), Cl (104), Ca (2)
        var ionSet = new Ion[]
        {
            new Ion("Ca", 2, 2),
            new Ion("K", 100, 0),
            new Ion("Li", 0, 100),
            new Ion("Cl", 104, 104)
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(+6.4).Within(0.5));
    }

    [Test]
    public void Test_LjpCalculationMatches_NgAndBarry007()
    {

        /* LJP for this test came from Ng and Barry (1994) Table 2 */

        // 50 CaCl2 + 50 MgCl2 : 100 LiCl
        // Ca (50), Cl (200), Mg (50) : Li (100), Cl (100)
        var ionSet = new Ion[]
        {
            new Ion("Ca", 50, 0),
            new Ion("Cl", 200, 100),
            new Ion("Mg", 50, 0),
            new Ion("Li", 0, 100)
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(-8.2).Within(0.5));
    }

    [Test]
    public void Test_LjpCalculationMatches_JPWin001()
    {
        // ion set shown in JPCalcWin manual (page 7)
        // https://tinyurl.com/wk7otn7

        var ionSet = new Ion[]
        {
            new Ion("Na", 10, 145),
            new Ion("Cl", 10, 145),
            new Ion("Cs", 135, 0),
            new Ion("F", 135, 0)
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(+8.74).Within(0.5));
    }

    [Test]
    public void Test_LjpCalculationMatches_JPWin002()
    {
        // ion set shown in JPCalcWin manual (page 10)
        // https://tinyurl.com/wk7otn7

        var ionSet = new Ion[]
        {
            new Ion("Cs", 145, 0),
            new Ion("Na", 0, 145),
            new Ion("F", 125, 0),
            new Ion("Cl", 20, 145)
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(+8.71).Within(0.5));
    }

    [Test]
    public void Test_LjpCalculationMatches_Harper001()
    {
        // ion set from Harper (1985) Table I
        // https://pubs.acs.org/doi/pdf/10.1021/j100255a022

        var ionSet = new Ion[]
        {
            new Ion("Na", 0.0995, 0.00499),
            new Ion("Cl", 0.0995, 0.00499)
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(-15.6).Within(0.5));
    }

    [Test]
    public void Test_LjpCalculationMatches_Harper002()
    {
        // ion set from Harper (1985) Table I
        // https://pubs.acs.org/doi/pdf/10.1021/j100255a022

        var ionSet = new Ion[]
        {
            new Ion("H", .1, .00345),
            new Ion("Cl", .1, .00345)
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(55.5).Within(0.5));
    }

    [Test]
    public void Test_LjpCalculationMatches_Harper003()
    {
        // ion set from Harper (1985) Table I
        // https://pubs.acs.org/doi/pdf/10.1021/j100255a022

        var ionSet = new Ion[]
        {
            new Ion("Ca", .29, .00545),
            new Ion("Cl", .29*2, .00545*2)
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(-35).Within(0.5));
    }

    [Test]
    public void Test_LjpCalculationMatches_Harper004()
    {
        // ion set from Harper (1985) Table I
        // https://pubs.acs.org/doi/pdf/10.1021/j100255a022

        var ionSet = new Ion[]
        {
            new Ion("Zn", .4, .0186),
            new Ion("SO4", .4, .0186),
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(-8.1).Within(0.5));
    }

    [Test]
    public void Test_DifficultSet_Owen2013()
    {
        // This ion set came from https://www.nature.com/articles/nature12330

        var ionSet = new Ion[]
        {
            new Ion("K", 50, 3),
            new Ion("Gluconate", 50, 0),
            new Ion("Cs", 70, 0),
            new Ion("HSO3", 70, 0),
            new Ion("TEA (TetraethylAmmonium)", 10, 10),
            new Ion("Cl", 12, 131.6),
            new Ion("Mg", 5, 1.3),
            new Ion("HEPES", 10, 0),
            new Ion("EGTA(2-)", .3, 0),
            new Ion("Tris", 10, 0),
            new Ion("ATP (Adenosine 5'-Triphosphate)", 4, 0),
            new Ion("Na", 0.3, 139.25),
            new Ion("HCO3", 0, 26),
            new Ion("H2PO4", 0, 1.25),
            new Ion("Ca", 0, 2),
            new Ion("4-AP (4-aminopyridine)", 0, 5),
        };

        ionSet = IonLibrary.Lookup(ionSet);

        var ljp = Calculate.Ljp(ionSet, temperatureC: 33);
        Assert.That(ljp.mV, Is.EqualTo(15.1 - 3.3).Within(0.5));
    }

    [Test]
    public void Test_KnownIonSets_LjpWithinExpectedRange()
    {
        Core.KnownIonSets.IKnownIonSet[] knowSets = Core.KnownIonSets.KnownSets.GetAll();
        Assert.That(knowSets, Is.Not.Null);
        Assert.That(knowSets, Is.Not.Empty);
        Console.WriteLine($"Checking {knowSets.Length} known ion sets...");

        foreach (var knownSet in knowSets)
        {
            Console.WriteLine(knownSet);
            Ion[] ions = IonLibrary.Lookup(knownSet.Ions);
            double ljp = Calculate.Ljp(ions, knownSet.Temperature_C).mV;
            Assert.That(ljp, Is.EqualTo(knownSet.Ljp_mV).Within(knownSet.Accuracy_mV));
        }
    }
}
