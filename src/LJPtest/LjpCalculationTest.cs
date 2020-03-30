using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using LJPmath;

namespace LJPtest
{
    class LjpCalculationTest
    {
        [Test]
        public void Test_LjpCalculationMatches_ExampleFromScreenshot()
        {
            /* Test came from screenshot on original JLJP website */

            var ionSet = new List<Ion>
            {
                new Ion("Zn", 9, 0.0284),
                new Ion("K", 0, 3),
                new Ion("Cl", 18, 3.0568)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet, autoSort: false);
            Assert.AreEqual(-20.79558643, ljp.mV, 1e-6);
        }

        [Test]
        public void Test_LjpMathThrowsExceptionIf_ChargeOrMuIsZero()
        {
            // here the ions aren't looked up from a table so charge and mu is 0
            var ionSet = new List<Ion>
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
            var ionSet = new List<Ion>
            {
                new Ion("Zn", 2, 52.8, 9, 0.0284),
                new Ion("K", 1, 73.5, 0, 3),
                new Ion("Cl",-1, 76.31, 18, 3.0568)
            };

            var ljp = Calculate.Ljp(ionSet, autoSort: false);
            Assert.AreEqual(-20.79558643, ljp.mV, 1e-6);
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

            var ionSet = new List<Ion> { Zn, K, Cl };
            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);
            var ljp = Calculate.Ljp(ionSet, autoSort: false);

            Assert.AreEqual(-20.79558643, ljp.mV, 1e-6);
        }

        [Test]
        public void Test_LjpCalculationMatches_NgAndBarry001()
        {

            /* LJP for this test came from Ng and Barry (1994) Table 2 */

            // 50 mM NaCl : 50 mM KCl
            var ionSet = new List<Ion>
            {
                new Ion("Na", 50, 0),
                new Ion("K", 0, 50),
                new Ion("Cl", 50, 50)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(-4.3, ljp.mV, 0.5);
        }

        [Test]
        public void Test_LjpCalculationMatches_NgAndBarry002()
        {

            /* LJP for this test came from Ng and Barry (1994) Table 2 */

            // 150 mM NaCl : 150 mM KCl
            // Na (150), Cl (150) : K (150) Cl (150)
            var ionSet = new List<Ion>
            {
                new Ion("Na", 150, 0),
                new Ion("K", 0, 150),
                new Ion("Cl", 150, 150)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(-4.3, ljp.mV, 0.5);
        }

        [Test]
        public void Test_LjpCalculationMatches_NgAndBarry003()
        {

            /* LJP for this test came from Ng and Barry (1994) Table 2 */

            // 50 NaCl : 50 CsCl
            // Na (50), Cl (50) : Cs (50) Cl (50)
            var ionSet = new List<Ion>
            {
                new Ion("Na", 50, 0),
                new Ion("Cs", 0, 50),
                new Ion("Cl", 50, 50)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(-4.9, ljp.mV, 0.5);
        }

        [Test]
        public void Test_LjpCalculationMatches_NgAndBarry004()
        {

            /* LJP for this test came from Ng and Barry (1994) Table 2 */

            // 100 NaCl : 100 MgCl2
            // Na (100), Cl (100) : Mg (100) Cl (100)
            var ionSet = new List<Ion>
            {
                new Ion("Na", 100, 0),
                new Ion("Mg", 0, 100),
                new Ion("Cl", 100, 200)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(+10.0, ljp.mV, 0.5);
        }

        [Test]
        public void Test_LjpCalculationMatches_NgAndBarry005()
        {

            /* LJP for this test came from Ng and Barry (1994) Table 2 */

            // 100 CaCl2 : 100 MgCl2
            // Ca (100), Cl (200) : Mg (100) Cl (200)
            var ionSet = new List<Ion>
            {
                new Ion("Ca", 100, 0),
                new Ion("Mg", 0, 100),
                new Ion("Cl", 200, 200)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(+0.6, ljp.mV, 0.5);
        }

        [Test]
        public void Test_LjpCalculationMatches_NgAndBarry006()
        {

            /* LJP for this test came from Ng and Barry (1994) Table 2 */

            // 100 KCl + 2 CaCl2 : 100 LiCl + 2 CaCl2
            // K (100), Cl (104), Ca (2) : Li (100), Cl (104), Ca (2)
            var ionSet = new List<Ion>
            {
                new Ion("Ca", 2, 2),
                new Ion("K", 100, 0),
                new Ion("Li", 0, 100),
                new Ion("Cl", 104, 104)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(+6.4, ljp.mV, 0.5);
        }

        [Test]
        public void Test_LjpCalculationMatches_NgAndBarry007()
        {

            /* LJP for this test came from Ng and Barry (1994) Table 2 */

            // 50 CaCl2 + 50 MgCl2 : 100 LiCl
            // Ca (50), Cl (200), Mg (50) : Li (100), Cl (100)
            var ionSet = new List<Ion>
            {
                new Ion("Ca", 50, 0),
                new Ion("Cl", 200, 100),
                new Ion("Mg", 50, 0),
                new Ion("Li", 0, 100)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(-8.2, ljp.mV, 0.5);
        }

        [Test]
        public void Test_LjpCalculationMatches_JPWin001()
        {
            // ion set shown in JPCalcWin manual (page 7)
            // https://tinyurl.com/wk7otn7

            var ionSet = new List<Ion>
            {
                new Ion("Na", 10, 145),
                new Ion("Cl", 10, 145),
                new Ion("Cs", 135, 0),
                new Ion("F", 135, 0)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(+8.74, ljp.mV, 0.5);
        }

        [Test]
        public void Test_LjpCalculationMatches_JPWin002()
        {
            // ion set shown in JPCalcWin manual (page 10)
            // https://tinyurl.com/wk7otn7

            var ionSet = new List<Ion>
            {
                new Ion("Cs", 145, 0),
                new Ion("Na", 0, 145),
                new Ion("F", 125, 0),
                new Ion("Cl", 20, 145)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(+8.71, ljp.mV, 0.5);
        }

        [Test]
        public void Test_LjpCalculationMatches_Harper001()
        {
            // ion set from Harper (1985) Table I
            // https://pubs.acs.org/doi/pdf/10.1021/j100255a022

            var ionSet = new List<Ion>
            {
                new Ion("Na", 0.0995, 0.00499),
                new Ion("Cl", 0.0995, 0.00499)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(-15.6, ljp.mV, 0.5);
        }

        [Test]
        public void Test_LjpCalculationMatches_Harper002()
        {
            // ion set from Harper (1985) Table I
            // https://pubs.acs.org/doi/pdf/10.1021/j100255a022

            var ionSet = new List<Ion>
            {
                new Ion("H", .1, .00345),
                new Ion("Cl", .1, .00345)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(55.5, ljp.mV, 0.5);
        }

        [Test]
        public void Test_LjpCalculationMatches_Harper003()
        {
            // ion set from Harper (1985) Table I
            // https://pubs.acs.org/doi/pdf/10.1021/j100255a022

            var ionSet = new List<Ion>
            {
                new Ion("Ca", .29, .00545),
                new Ion("Cl", .29*2, .00545*2)
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(-35, ljp.mV, 0.5);
        }

        [Test]
        public void Test_LjpCalculationMatches_Harper004()
        {
            // ion set from Harper (1985) Table I
            // https://pubs.acs.org/doi/pdf/10.1021/j100255a022

            var ionSet = new List<Ion>
            {
                new Ion("Zn", .4, .0186),
                new Ion("SO4", .4, .0186),
            };

            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);

            var ljp = Calculate.Ljp(ionSet);
            Assert.AreEqual(-8.1, ljp.mV, 0.5);
        }

        [Test]
        public void Test_KnownIonSets_LjpWithinExpectedRange()
        {
            var ionTable = new IonTable();
            var knownSets = new KnownIonSets(ionTable);
            foreach (var ionSet in knownSets.ionSets)
            {
                Console.WriteLine($"Testing known ion set: {ionSet.name}");
                var ljp = Calculate.Ljp(ionSet.ions, ionSet.temperatureC);
                Assert.AreEqual(ionSet.expectedLjp_mV, ljp.mV, ionSet.expectedAccuracy_mV);
            }
        }
    }
}
