using NUnit.Framework;
using LJPcalc.Core;

namespace LJPcalc.Test;

class IonSortingTests
{
    [Test]
    public void Test_BadOrder_FixedByAutoSort001()
    {
        /* Known to crash due to poor ion order https://github.com/swharden/LJPcalc/issues/9 */
        // fixed by autoSort prior to calculation

        var ionSet = new Ion[] {
                new Ion("Zn", 9, .0284),
                new Ion("K", 0, 3),
                new Ion("Cl", 18, 3.062),
                new Ion("Mg", 5, 3),
                new Ion("Ag", 1, 1),
            };

        ionSet = IonLibrary.Lookup(ionSet);

        // it does not solve in this order
        Assert.Throws<OperationCanceledException>(() => Calculate.Ljp(ionSet, autoSort: false, throwIfTimeout: true));

        // but it does solve if allowed to auto-sort
        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(-13.5).Within(0.5));
    }

    [Test]
    public void Test_BadOrder_FixedByAutoSort002()
    {
        /* Known to crash due to poor ion order https://github.com/swharden/LJPcalc/issues/9 */
        // fixed by autoSort prior to calculation

        var ionSet = new Ion[] {
                new Ion("K", 145, 2.8),
                new Ion("Na", 13, 145),
                new Ion("Cl", 10, 148.8),
                new Ion("Gluconate", 145, 0),
                new Ion("Mg", 1, 2),
                new Ion("Ca", 0, 1),
                new Ion("HEPES", 5, 5),
            };

        ionSet = IonLibrary.Lookup(ionSet);

        // it does not solve in this order
        Assert.Throws<OperationCanceledException>(() => Calculate.Ljp(ionSet, autoSort: false, throwIfTimeout: true));

        // but it does solve if allowed to auto-sort
        var ljp = Calculate.Ljp(ionSet);
        Assert.That(ljp.mV, Is.EqualTo(16.3).Within(0.5));
    }
}
