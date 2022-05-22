namespace LJPcalc.Core.Solver;

public class EquationSolver
{
    public readonly IEquation Equation;
    public readonly int EquationCount;
    private readonly Random Rand = new(0);
    private EquationSolution[] Solutions;

    /// <summary>
    /// Values for the best solution found so far (the one with the smallest maximum error)
    /// </summary>
    public EquationSolution BestSolution => Solutions[0];

    /// <summary>
    /// Number of times a proposed solution was tested
    /// </summary>
    public int Iterations { get; private set; }

    /// <summary>
    /// This event is invoked after each iteration
    /// </summary>
    public event EventHandler? IterationFinished;

    /// <summary>
    /// Abort the solving process after this number of iterations
    /// </summary>
    public int MaximumIterations = int.MaxValue;

    /// <summary>
    /// Controls whether a hard exception is thrown if the maximum iteration limit is hit
    /// </summary>
    public bool ThrowIfIterationLimitExceeded;

    /// <summary>
    /// Vectorial equations in the form f(x) = 0
    /// </summary>
    public EquationSolver(IEquation equation, double[] initialXs)
    {
        if (initialXs.Length == 0)
            throw new Exception($"{nameof(initialXs)} cannot be empty");

        Equation = equation;
        EquationCount = initialXs.Length;
        Solutions = new EquationSolution[] { Equation.Calculate(initialXs) };
    }

    /// <summary>
    /// Find the best set of inputs where the errors are all close to zero.
    /// A valid solution is found when the error for every input is between -1 and 1 (each is < 1% error)
    /// </summary>
    public double[] Solve()
    {
        Func<EquationSolution>[] solutionMethods =
        {
            GetPoint_ShiftedBySolutionDelta,
            GetPoint_NearFirstPoint,
            GetPoint_TotallyRandom,
            GetPoint_ConsideringMinMax,
        };

        while (BestSolution.MaxAbsoluteError > 1.0)
        {
            EquationSolution newSolution = solutionMethods[Iterations++ % solutionMethods.Length].Invoke();

            Solutions = Solutions
                .Append(newSolution)
                .OrderBy(x => x.MaxAbsoluteError)
                .Take(EquationCount * 4)
                .ToArray();

            IterationFinished?.Invoke(this, EventArgs.Empty);

            if (Iterations >= MaximumIterations)
            {
                if (ThrowIfIterationLimitExceeded)
                    throw new OperationCanceledException($"hit maximum iteration limit ({MaximumIterations}) while solving Phis");
                else
                    break;
            }
        }

        return BestSolution.Inputs.ToArray();
    }

    /// <summary>
    /// Add a point with Xs shifted by the delta of the solved matrix
    /// </summary>
    private EquationSolution GetPoint_ShiftedBySolutionDelta()
    {
        if (Solutions.Length < EquationCount + 1)
        {
            return GetPoint_NearFirstPoint();
        }

        double[] suggestedXs = new double[EquationCount];

        double[,] Mm = new double[EquationCount, EquationCount];
        for (int j = 0; j < EquationCount; j++)
            for (int k = 0; k < EquationCount; k++)
                Mm[j, k] = Solutions[k].Errors[j] - Solutions[EquationCount].Errors[j];

        double[] mF0 = new double[EquationCount];
        for (int j = 0; j < EquationCount; j++)
            mF0[j] = -Solutions[EquationCount].Errors[j];

        double[,] Vm = new double[EquationCount, EquationCount];
        for (int j = 0; j < EquationCount; j++)
            for (int k = 0; k < EquationCount; k++)
                Vm[j, k] = Solutions[k].Inputs[j] - Solutions[EquationCount].Inputs[j];

        double[] u = LinearAlgebra.Solve(Mm, mF0);
        double[] delta = LinearAlgebra.Product(Vm, u);

        for (int j = 0; j < EquationCount; j++)
            suggestedXs[j] = Solutions[EquationCount].Inputs[j] + delta[j];

        return Equation.Calculate(suggestedXs);
    }

    /// <summary>
    /// Add a point with Xs randomly offset from the Xs of the first point
    /// </summary>
    private EquationSolution GetPoint_NearFirstPoint()
    {
        const double randomness = 4; // TODO: could this value be optimized?

        double[] suggestedXs = BestSolution.Inputs.Select(x => x * (Rand.NextDouble() - 0.5) * randomness).ToArray();

        return Equation.Calculate(suggestedXs);
    }

    /// <summary>
    /// Add a point with totally random Xs
    /// </summary>
    private EquationSolution GetPoint_TotallyRandom()
    {
        const double randomness = 4; // TODO: could this value be optimized?

        double[] suggestedXs = Enumerable.Range(0, EquationCount)
                                         .Select(x => (Rand.NextDouble() - 0.5) * randomness)
                                         .ToArray();

        return Equation.Calculate(suggestedXs);
    }

    /// <summary>
    /// Add a point with Xs randomized centered and scaled to the min/max of the existing Xs
    /// </summary>
    private EquationSolution GetPoint_ConsideringMinMax()
    {
        const double randomness = 3; // TODO: could this value be optimized?

        double[] suggestedXs = new double[EquationCount];

        for (int equationIndex = 0; equationIndex < EquationCount; equationIndex++)
        {
            var equationXs = Solutions.Select(x => x.Inputs[equationIndex]);
            double xMin = equationXs.Min();
            double xMax = equationXs.Max();

            if (xMin == xMax)
            {
                if (xMin > 0)
                    (xMin, xMax) = (xMin * .8, xMin * 1.2);
                else if (xMin > 0)
                    (xMin, xMax) = (xMin * 1.2, xMin * 0.8);
                else
                    (xMin, xMax) = (-1, 1);
            }

            double mean = (xMin + xMax) / 2.0;
            double span = xMax - xMin;
            double randomOffset = span * randomness * (Rand.NextDouble() - 0.5);
            suggestedXs[equationIndex] = mean + randomOffset;
        }

        return Equation.Calculate(suggestedXs);
    }
}
