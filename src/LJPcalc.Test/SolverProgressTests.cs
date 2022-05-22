using LJPcalc.Core;
using LJPcalc.Core.Solver;
using NUnit.Framework;

namespace LJPcalc.Test;

internal class SolverProgressTests
{
    [Test]
    public void Test_Solver_Updates()
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

        LjpCalculationOptions options = new()
        {
            AutoSort = false,
            MaximumIterations = 123,
            ThrowIfIterationLimitExceeded = false,
        };

        options.IterationFinished += Options_IterationFinished;

        LjpResult result = Calculate.Ljp(ions, options);
        Console.WriteLine(result.GetSummary());

        options.AutoSort = true;
        result = Calculate.Ljp(ions, options);
        Console.WriteLine(result.GetSummary());
    }

    private void Options_IterationFinished(object? sender, EventArgs e)
    {
        EquationSolver solver = (EquationSolver)sender!;
        Console.WriteLine($"Iteration {solver.Iterations} M={solver.M:N2}");
    }
}
