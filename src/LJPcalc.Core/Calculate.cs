using LJPcalc.Core.Solver;

namespace LJPcalc.Core;

public class Calculate
{
    public static LjpResult Ljp(Ion[] ions, double temperatureC = 25)
    {
        LjpCalculationOptions defaultOptions = new() { TemperatureC = temperatureC };
        return Ljp(ions, defaultOptions);
    }

    public static LjpResult Ljp(Ion[] ions, LjpCalculationOptions options)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        Ion[] ionsInput = ions.Select(x => x.Clone()).ToArray();
        ions = ions.Select(x => x.Clone()).ToArray();

        if (ions.Any(x => x.Charge == 0))
            throw new ArgumentException("ion charge cannot be zero");

        if (ions.Any(x => x.Mu == 0))
            throw new ArgumentException("ion mu cannot be zero");

        if (options.AutoSort)
            ions = PreCalculationIonListSort(ions);

        Ion secondFromLastIon = ions[ions.Length - 2];
        Ion LastIon = ions[ions.Length - 1];

        if (secondFromLastIon.C0 == secondFromLastIon.CL)
            throw new InvalidOperationException("second from last ion concentrations cannot be equal");

        // determine flux for all ions
        double[] phis = ions.Take(ions.Length - 2).Select(x => x.CL - x.C0).ToArray();
        int iterations = 0;

        // use the fancy solver to optimize phis when there are more than 2 ions
        if (ions.Length > 2)
        {
            IEquation equations = new PhiEquationSystem(ions, options.TemperatureC);

            EquationSolver solver = new(equations, phis)
            {
                MaximumIterations = options.MaximumIterations,
                ThrowIfIterationLimitExceeded = options.ThrowIfIterationLimitExceeded
            };
            solver.IterationFinished += (object? sender, EventArgs args) => options.OnIterationFinished(sender, args);

            phis = solver.Solve();

            iterations = solver.Iterations;
        }

        LjpSolution ljpSolution = LjpSolver.CalculateLjp(ions, phis, options.TemperatureC);

        // update ions based on new phis and CLs (all ions except the last two)
        for (int j = 0; j < phis.Length; j++)
        {
            ions[j].Phi = phis[j];
            ions[j].CL = ljpSolution.CLs[j];
        }

        // second from last ion phi is concentration difference
        secondFromLastIon.Phi = secondFromLastIon.CL - secondFromLastIon.C0;

        // last ion's phi is calculated from all the phis before it
        LastIon.Phi = -ions.Take(ions.Length - 1).Sum(x => x.Phi * x.Charge) / LastIon.Charge;

        // last ion's cL is calculated from all the cLs before it
        LastIon.CL = -ions.Take(ions.Length - 1).Sum(x => x.CL * x.Charge) / LastIon.Charge;

        // load new details into the result
        LjpResult result = new(ionsInput, options.TemperatureC, ljpSolution.Millivolts, sw.Elapsed, iterations);

        return result;
    }

    /// <summary>
    /// This strategy places the ion with the largest difference between C0 and CL last
    /// and the remaining ion with the largest CL second to last.
    /// </summary>
    private static Ion[] PreCalculationIonListSort(Ion[] inputIons)
    {
        List<Ion> ionList = inputIons.ToList();

        // absolute largest cL should be last
        Ion ionWithLargestCL = ionList.OrderBy(x => x.CL).Last();
        ionList.Remove(ionWithLargestCL);

        // largest diff should be second from last
        Ion ionWithLargestCDiff = ionList.OrderBy(x => Math.Abs(x.CL - x.C0)).Last();
        ionList.Remove(ionWithLargestCDiff);

        // place the removed ions back in the proper order
        ionList.Add(ionWithLargestCDiff);
        ionList.Add(ionWithLargestCL);
        return ionList.ToArray();
    }
}
