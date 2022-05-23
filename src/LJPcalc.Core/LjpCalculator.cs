using System.Diagnostics;
using LJPcalc.Core.Solver;

namespace LJPcalc.Core;

public class LjpCalculator
{
    public readonly Ion[] Ions;

    public readonly double TemperatureC;
    public int Iterations => Solver.Iterations;

    private readonly Stopwatch Stopwatch = new();

    public TimeSpan Elapased => Stopwatch.Elapsed;

    private readonly EquationSolver Solver;

    private readonly PhiEquationSystem PhiEquations;
    public double BestSolutionMaxError => Solver.BestSolution.MaxAbsoluteError;
    private double[] BestSolutionPhis => Solver.BestSolution.Inputs;
    private double[] BestSolutionCLs => Solver.BestSolution.Outputs;

    public LjpCalculator(Ion[] ions, double temperature = 25, bool autoSort = true)
    {
        if (ions is null)
            throw new ArgumentNullException(nameof(ions));

        if (ions.Length == 1)
            throw new ArgumentException("LJP calculation requires at least two ions");

        foreach (Ion ion in ions)
        {
            if (ion.Charge == 0)
                throw new ArgumentException($"ion charge cannot not be zero: {ion}");

            if (ion.Conductivity == 0)
                throw new ArgumentException($"ion conductivity cannot not be zero: {ion}");

            if (ion.Mu == 0)
                throw new ArgumentException($"ion mobility cannot not be zero: {ion}");
        }

        if (ions[ions.Length - 2].C0 == ions[ions.Length - 2].CL)
            throw new InvalidOperationException("second from last ion concentrations cannot be equal");

        // clone all ions so we don't accidentally mutate any values passed in
        ions = ions.Select(x => x.Clone()).ToArray();

        if (autoSort)
            ions = IonSorting.PreCalculationSort(ions);

        Ions = ions;
        TemperatureC = temperature;
        PhiEquations = new PhiEquationSystem(ions, temperature);

        double[] initialPhis = ions.Take(ions.Length - 2).Select(x => x.CL - x.C0).ToArray();
        Solver = new EquationSolver(PhiEquations, initialPhis);
    }

    public override string ToString()
    {
        if (BestSolutionMaxError > 1)
            return $"LJP calculator after {Iterations} iterations (max error: {BestSolutionMaxError:N2}%)";
        else
            return $"LJP calculator after {Iterations} iterations (max error: {BestSolutionMaxError:#.##E+0}%)";
    }

    public static string GetVersion()
    {
        Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!;
        return $"{version.Major}.{version.Minor}.{version.Build}";
    }

    /// <summary>
    /// Perform a single iteration
    /// </summary>
    public void Iterate()
    {
        Stopwatch.Start();
        Solver.Iterate();
        Stopwatch.Stop();
    }

    /// <summary>
    /// Iterate the solver until the error is below the target or the iteration count exceeds the limit
    /// </summary>
    public void IterateRepeatedly(double targetError = 1.0, int maxIterations = 5000)
    {
        while (Solver.BestSolution.MaxAbsoluteError > targetError && Iterations < maxIterations)
        {
            Iterate();
        }
    }

    /// <summary>
    /// Calculate LJP using the best available solution
    /// </summary>
    public LjpResult GetLJP()
    {
        Ion[] ions = Ions.Select(x => x.Clone()).ToArray();
        Ion lastIon = ions[Ions.Length - 1];
        Ion secondFromLastIon = ions[Ions.Length - 2];

        // update ions based on new phis and CLs (all ions except the last two)
        for (int j = 0; j < BestSolutionPhis.Length; j++)
        {
            ions[j].Phi = BestSolutionPhis[j];
            ions[j].CL = BestSolutionCLs[j];
        }

        // second from last ion phi is its concentration difference
        secondFromLastIon.Phi = secondFromLastIon.CL - secondFromLastIon.C0;

        // last ion's phi is calculated using all the phis before it
        lastIon.Phi = -ions.Take(ions.Length - 1).Sum(x => x.Phi * x.Charge) / lastIon.Charge;

        // last ion's cL is calculated from all the cLs before it
        lastIon.CL = -ions.Take(ions.Length - 1).Sum(x => x.CL * x.Charge) / lastIon.Charge;

        LjpSolution sol = LjpSolver.CalculateLjp(Ions, BestSolutionPhis, TemperatureC);

        return new LjpResult(Ions, TemperatureC, sol.Millivolts, Elapased, Iterations, BestSolutionMaxError);
    }
}
