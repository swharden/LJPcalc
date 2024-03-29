﻿namespace LJPcalc.Core.Solver;

public class PhiEquationSolver
{
    public readonly PhiEquation Equation;
    public readonly int EquationCount;
    private readonly Random Rand = new(0);
    private PhiEquationSolution[] Solutions;

    /// <summary>
    /// Values for the best solution found so far (the one with the smallest maximum error)
    /// </summary>
    public PhiEquationSolution BestSolution => Solutions[0];

    /// <summary>
    /// Number of times a proposed solution was tested
    /// </summary>
    public int Iterations { get; private set; }

    /// <summary>
    /// Functions which can use logic and/or randomness to generate new solutions
    /// which may be better than the existing ones using information about the solutions found so far.
    /// </summary>
    private readonly IStrategy[] Strategies;

    /// <summary>
    /// Vectorial equations in the form f(x) = 0
    /// </summary>
    public PhiEquationSolver(PhiEquation equation)
    {
        double[] initialPhis = equation.Ions.Take(equation.Ions.Length - 2).Select(x => x.CL - x.C0).ToArray();
        PhiEquationSolution initialSolution = equation.Calculate(initialPhis);

        Equation = equation;
        EquationCount = initialPhis.Length;
        Solutions = new PhiEquationSolution[] { initialSolution };
        Strategies = new IStrategy[]
        {
            new Strategies.ShiftByDelta(), // best strategy so use it most often
            new Strategies.NearBestSolution(),
            new Strategies.ShiftByDelta(),
            new Strategies.ConsideringMinMax(),
            new Strategies.ShiftByDelta(),
            new Strategies.TotallyRandom(),
        };
    }

    /// <summary>
    /// Take another step toward finding a potentially better solution.
    /// Calculate LJP, assess CL error, keep the new solution only if it's in the top N solutions.
    /// This function returns the solution generated by this iteration.
    /// </summary>
    public PhiEquationSolution Iterate()
    {
        IStrategy strategy = Strategies[Iterations++ % Strategies.Length];

        double[] suggestedXs = strategy.SuggestXs(EquationCount, Solutions, Rand);

        PhiEquationSolution newSolution = Equation.Calculate(suggestedXs);

        Solutions = Solutions
            .Append(newSolution)
            .OrderBy(x => x.MaxAbsoluteError)
            .Take(EquationCount * 4) // TODO: optimize this?
            .ToArray();

        return newSolution;
    }
}
