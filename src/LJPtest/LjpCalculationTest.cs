using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using LJPmath;

namespace LJPtest
{
    class LjpCalculationTest
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            System.IO.File.Delete("IonTable.csv");
            System.IO.File.Copy("../../../../IonTable.csv", "./IonTable.csv");
        }

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

            double ljp_mV = Calculate.Ljp(ionSet) * 1000;
            Assert.AreEqual(-20.82388089, ljp_mV, 1e-6);
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

            double ljp_mV = Calculate.Ljp(ionSet) * 1000;
            Assert.AreEqual(-20.82388089, ljp_mV, 1e-6);
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

            //Zn.setCdadc("-1.0+2.0/3.0*(atan((Zn-5.45776)*0.408978)*2.35004+3.0-atan(-5.45776*0.408978)*2.35004)");
            //K.setCdadc("1.0");
            //Cl.setCdadc("1.0/2.0+1.0/6.0*(atan((Zn-5.45776)*0.408978)*2.35004+3.0-atan(-5.45776*0.408978)*2.35004)");
            // if custom cdadcs are used the LJP will be -16.65

            var ionSet = new List<Ion> { Zn, K, Cl };
            var ionTable = new IonTable();
            ionSet = ionTable.Lookup(ionSet);
            double ljp_mV = Calculate.Ljp(ionSet) * 1000;
            Assert.AreEqual(-20.82388089, ljp_mV, 1e-6);
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

            double ljp_mV = Calculate.Ljp(ionSet) * 1000;
            Assert.AreEqual(-4.3, ljp_mV, 0.5);
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

            double ljp_mV = Calculate.Ljp(ionSet) * 1000;
            Assert.AreEqual(-4.3, ljp_mV, 0.5);
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

            double ljp_mV = Calculate.Ljp(ionSet) * 1000;
            Assert.AreEqual(-4.9, ljp_mV, 0.5);
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

            double ljp_mV = Calculate.Ljp(ionSet) * 1000;
            Assert.AreEqual(+10.0, ljp_mV, 0.5);
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

            double ljp_mV = Calculate.Ljp(ionSet) * 1000;
            Assert.AreEqual(+0.6, ljp_mV, 0.5);
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

            double ljp_mV = Calculate.Ljp(ionSet) * 1000;
            Assert.AreEqual(+6.4, ljp_mV, 0.5);
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

            double ljp_mV = Calculate.Ljp(ionSet) * 1000;
            Assert.AreEqual(-8.2, ljp_mV, 0.5);
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

            double ljp_mV = Calculate.Ljp(ionSet) * 1000;
            Assert.AreEqual(+8.74, ljp_mV, 0.5);
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

            double ljp_mV = Calculate.Ljp(ionSet) * 1000;
            Assert.AreEqual(+8.71, ljp_mV, 0.5);
        }
    }
}
