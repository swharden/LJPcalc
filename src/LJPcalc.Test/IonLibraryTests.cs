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

        for (int i = 0; i < KnownGoodIons.Length; i++)
        {
            Assert.That(KnownGoodIons[i].ToString(), Is.EqualTo(LookedUpIons[i].ToString()));
            Assert.That(KnownGoodIons[i].Charge, Is.EqualTo(LookedUpIons[i].Charge));
            Assert.That(KnownGoodIons[i].Conductivity, Is.EqualTo(LookedUpIons[i].Conductivity));
            Assert.That(KnownGoodIons[i].CL, Is.EqualTo(LookedUpIons[i].CL));
            Assert.That(KnownGoodIons[i].C0, Is.EqualTo(LookedUpIons[i].C0));
        }
    }

    [Test]
    public void Test_Library_HasNoDuplicates()
    {
        HashSet<string> duplicatedIons = new();
        HashSet<string> seenIons = new();
        foreach (Ion ion in IonLibrary.KnownIons)
        {
            string nameWithCharge = ion.NameWithCharge;
            if (seenIons.Contains(nameWithCharge))
            {
                duplicatedIons.Add(nameWithCharge);
                Console.WriteLine($"DUPLICATE: {nameWithCharge}");
            }
            else
            {
                seenIons.Add(nameWithCharge);
            }
        }

        Assert.That(seenIons, Is.Not.Empty);
        Assert.That(duplicatedIons, Is.Empty);
    }

    [Test]
    public void Test_Library_Duplicate_MobilitiesAreClose()
    {
        Ion[] ions = IonLibrary.GetKnownIons(removeDuplicates: false);

        HashSet<string> duplicateNames = new();
        HashSet<string> seenNames = new();
        foreach (Ion ion in ions)
        {
            string name = ion.NameWithCharge;
            if (seenNames.Contains(name))
            {
                duplicateNames.Add(name);
            }
            else
            {
                seenNames.Add(name);
            }
        }

        Assert.That(seenNames, Is.Not.Empty);
        Assert.That(duplicateNames, Is.Not.Empty);

        foreach (string name in duplicateNames)
        {
            Ion[] duplicateIons = ions.Where(x => x.NameWithCharge == name).ToArray();
            Assert.That(duplicateIons, Is.Not.Empty);

            foreach (Ion ion in duplicateIons)
            {
                Assert.That(duplicateIons.First().Conductivity, Is.EqualTo(ion.Conductivity).Within(7).Percent, name);
            }
        }
    }

    [Test]
    public void Test_KnownIonSets_AreBalanced()
    {
        Core.KnownIonSets.IKnownIonSet[] knowSets = Core.KnownIonSets.KnownSets.GetAll();
        Assert.That(knowSets, Is.Not.Null);
        Assert.That(knowSets, Is.Not.Empty);

        foreach (var knownSet in knowSets)
        {
            Ion[] ions = IonLibrary.Lookup(knownSet.Ions);
            Assert.That(ions.Where(x => x.Charge == 0), Is.Empty);

            double totalLeftCharge = ions.Select(x => x.C0 * x.Charge).Sum();
            double totalLeftConcenration = ions.Select(x => x.C0).Sum();
            Assert.That(totalLeftCharge, Is.EqualTo(0).Within(.05 * totalLeftConcenration), knownSet.Name);

            double totalRightCharge = ions.Select(x => x.CL * x.Charge).Sum();
            double totalRightConcentration = ions.Select(x => x.CL).Sum();
            Assert.That(totalRightCharge, Is.EqualTo(0).Within(.05 * totalRightConcentration), knownSet.Name);
        }
    }
}