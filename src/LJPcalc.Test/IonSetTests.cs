using LJPcalc.Core;
using LJPcalc.Core.KnownIonSets;
using NUnit.Framework;

namespace LJPcalc.Test;

internal class IonSetTests
{
    [Test]
    public void Test_IonSet_TextExport()
    {
        IKnownIonSet knownSet = new Owen2013();
        Ion[] ions1 = IonLibrary.Lookup(knownSet.Ions);
        double temperature1 = knownSet.Temperature_C;

        string text1 = IonSet.ToText(ions1, temperature1);
        Console.WriteLine(text1);

        (Ion[] ions2, double temperature2) = IonSet.FromText(text1);
        string text2 = IonSet.ToText(ions2, temperature2);

        Assert.That(temperature1, Is.EqualTo(temperature2));
        Assert.That(text1, Is.EqualTo(text2));
    }
}
