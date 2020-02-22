using LJPmath;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LJPtest
{
    public class IonSetTest
    {
        [Test]
        public void Test_SaveAndLoad_ValuesAreIdentical()
        {
            // create an ion set
            var ions = new List<Ion>
            {
                new Ion("Na", +1, 50.11E-4, 11, 22),
                new Ion("Cl", -1, 76.31E-4, 33, 44),
                new Ion("K", -1, 73.5E-4, 55, 66)
            };

            // save the ion set to a file
            string filePath = System.IO.Path.GetFullPath("testIonSet.md");
            IonSet ionSet1 = new IonSet(ions);
            ionSet1.Save(filePath);
            Console.WriteLine($"saved: {filePath}");

            // load the ion set from the file
            IonSet ionSet2 = new IonSet(filePath);

            // test that both ion sets are equal
            Assert.AreEqual(ions.Count, ionSet1.ions.Count);
            Assert.AreEqual(ions.Count, ionSet2.ions.Count);
            Assert.AreEqual(ionSet1.ions.Count, ionSet2.ions.Count);

            for (int i=0; i< ions.Count; i++)
            {
                Assert.AreEqual(ions[i].name, ionSet1.ions[i].name);
                Assert.AreEqual(ions[i].charge, ionSet1.ions[i].charge);
                Assert.AreEqual(ions[i].conductivity, ionSet1.ions[i].conductivity, 1e-10);
                Assert.AreEqual(ions[i].c0, ionSet1.ions[i].c0);
                Assert.AreEqual(ions[i].cL, ionSet1.ions[i].cL);

                Assert.AreEqual(ions[i].name, ionSet2.ions[i].name);
                Assert.AreEqual(ions[i].charge, ionSet2.ions[i].charge);
                Assert.AreEqual(ions[i].conductivity, ionSet2.ions[i].conductivity, 1e-10);
                Assert.AreEqual(ions[i].c0, ionSet2.ions[i].c0);
                Assert.AreEqual(ions[i].cL, ionSet2.ions[i].cL);
            }
        }
    }
}
