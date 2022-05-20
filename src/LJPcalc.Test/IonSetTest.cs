using NUnit.Framework;
using LJPcalc.Core;

namespace LJPcalc.Test;

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
        string filePath = System.IO.Path.GetFullPath(System.IO.Path.GetTempFileName() + "_testIonSet.md");
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
            Assert.AreEqual(ions[i].Name, ionSet1.ions[i].Name);
            Assert.AreEqual(ions[i].Charge, ionSet1.ions[i].Charge);
            Assert.AreEqual(ions[i].Conductivity, ionSet1.ions[i].Conductivity, 1e-10);
            Assert.AreEqual(ions[i].C0, ionSet1.ions[i].C0);
            Assert.AreEqual(ions[i].CL, ionSet1.ions[i].CL);

            Assert.AreEqual(ions[i].Name, ionSet2.ions[i].Name);
            Assert.AreEqual(ions[i].Charge, ionSet2.ions[i].Charge);
            Assert.AreEqual(ions[i].Conductivity, ionSet2.ions[i].Conductivity, 1e-10);
            Assert.AreEqual(ions[i].C0, ionSet2.ions[i].C0);
            Assert.AreEqual(ions[i].CL, ionSet2.ions[i].CL);
        }
    }
}
