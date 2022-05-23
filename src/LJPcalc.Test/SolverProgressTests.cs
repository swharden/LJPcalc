﻿using LJPcalc.Core;
using LJPcalc.Core.Solver;
using NUnit.Framework;

namespace LJPcalc.Test;

internal class SolverProgressTests
{
    [Test]
    public void Test_Calculator_LimitIterations()
    {
        // This ion set in this order is known to be very difficult to solve

        Ion[] ions = {
            new Ion("Zn", 9, .0284),
            new Ion("K", 0, 3),
            new Ion("Cl", 18, 3.062),
            new Ion("Mg", 5, 3),
            new Ion("Ag", 1, 1),
        };

        ions = IonLibrary.Lookup(ions);

        LjpCalculator calcWithoutSorting = new(ions, autoSort: false);
        calcWithoutSorting.IterateRepeatedly(1.0, 123);
        Console.WriteLine(calcWithoutSorting);
        Assert.That(calcWithoutSorting.Iterations, Is.EqualTo(123));

        LjpCalculator calcWithSorting = new(ions, autoSort: true);
        calcWithSorting.IterateRepeatedly(1.0, 123);
        Console.WriteLine(calcWithSorting);
        Assert.That(calcWithSorting.Iterations, Is.LessThan(calcWithoutSorting.Iterations));
    }

    [Test]
    public void Test_Calculator_ManualIterations()
    {
        // This ion set in this order is known to be very difficult to solve

        Ion[] ions = {
            new Ion("Zn", 9, .0284),
            new Ion("K", 0, 3),
            new Ion("Cl", 18, 3.062),
            new Ion("Mg", 5, 3),
            new Ion("Ag", 1, 1),
        };

        ions = IonLibrary.Lookup(ions);

        LjpCalculator calc = new(ions);
        for (int i = 0; i < 100; i++)
        {
            calc.Iterate();
            Console.WriteLine(calc.GetLJP());
        }
    }
}
