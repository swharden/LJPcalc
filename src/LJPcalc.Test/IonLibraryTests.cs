using NUnit.Framework;
using LJPcalc.Core;

namespace LJPcalc.Test;

public class IonLibraryTests
{
    [Test]
    public void Test_IonLibrary_HasItems()
    {
        Assert.That(IonLibrary.KnownIons, Is.Not.Null);
        Assert.That(IonLibrary.KnownIons, Is.Not.Empty);
    }

    [Test]
    public void Test_IonLibrary_LookupIndividual()
    {

        Ion[] KnownGoodIons =
        {
             new("Zn", 2, 52.8, 9, 0.0284),
             new("K", 1, 73.5, 0, 3),
             new("Cl", -1, 76.31, 18, 3.0568),
        };

        foreach (Ion knownIon in KnownGoodIons)
        {
            Ion lookedUpIon = IonLibrary.Lookup(knownIon.Name);
            Assert.That(knownIon.Conductivity, Is.EqualTo(lookedUpIon.Conductivity));
        }
    }

    [Test]
    public void Test_IonLibrary_LookupGroup()
    {

        Ion[] KnownGoodIons =
        {
             new("Zn", 2, 52.8, 9, 0.0284),
             new("K", 1, 73.5, 0, 3),
             new("Cl", -1, 76.31, 18, 3.0568),
        };

        Ion[] LookedUpIons = IonLibrary.Lookup(KnownGoodIons);

        for (int i=0; i<KnownGoodIons.Length; i++)
        {
            Assert.That(KnownGoodIons[i].ToString(), Is.EqualTo(LookedUpIons[i].ToString()));
            Assert.That(KnownGoodIons[i].Charge, Is.EqualTo(LookedUpIons[i].Charge));
            Assert.That(KnownGoodIons[i].Conductivity, Is.EqualTo(LookedUpIons[i].Conductivity));
            Assert.That(KnownGoodIons[i].CL, Is.EqualTo(LookedUpIons[i].CL));
            Assert.That(KnownGoodIons[i].C0, Is.EqualTo(LookedUpIons[i].C0));
        }
    }
}
