namespace LJPcalc.Core.Solver;

public class EquationSolver
{
    public readonly IEquationSystem Equation;
    public readonly int EquationCount;

    private EquationSolution[] EquationSolutions;

    public EquationSolution BestSolution => EquationSolutions[0];

    /// <summary>
    /// Largest scaled result absolute value for the best solution available.
    /// The solver tries to minimize this value, considering < 1 to be a solution.
    /// </summary>
    public double M => EquationSolutions[0].AbsoluteLargestOutput;

    private readonly Random Rand = new(0);

    public int Iterations { get; private set; }

    public event EventHandler? IterationFinished;
    public int MaximumIterations;
    public bool ThrowIfIterationLimitExceeded;

    /// <summary>
    /// Vectorial equations in the form f(x) = 0
    /// </summary>
    public EquationSolver(IEquationSystem equation, double[] initialXs)
    {
        if (initialXs.Length == 0)
            throw new Exception($"{nameof(initialXs)} cannot be empty");

        Equation = equation;
        EquationCount = initialXs.Length;

        double[] initialYs = Equation.Calculate(initialXs);

        EquationSolutions = new EquationSolution[] { new EquationSolution(initialXs, initialYs) };
    }

    /// <summary>
    /// Find the best set of inputs (xs) where the scaled outputs are all close to zero.
    /// A valid solution is a set of xs where for every x, f(x) is between -1 and 1.
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

        while (BestSolution.AbsoluteLargestOutput > 1.0)
        {
            EquationSolution newSolution = solutionMethods[Iterations++ % solutionMethods.Length].Invoke();

            EquationSolutions = EquationSolutions
                .Append(newSolution)
                .OrderBy(x => Math.Abs(x.AbsoluteLargestOutput))
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
        if (EquationSolutions.Length < EquationCount + 1)
        {
            return GetPoint_NearFirstPoint();
        }

        double[] suggestedXs = new double[EquationCount];

        double[,] Mm = new double[EquationCount, EquationCount];
        for (int j = 0; j < EquationCount; j++)
            for (int k = 0; k < EquationCount; k++)
                Mm[j, k] = EquationSolutions[k].Outputs[j] - EquationSolutions[EquationCount].Outputs[j];

        double[] mF0 = new double[EquationCount];
        for (int j = 0; j < EquationCount; j++)
            mF0[j] = -EquationSolutions[EquationCount].Outputs[j];

        double[,] Vm = new double[EquationCount, EquationCount];
        for (int j = 0; j < EquationCount; j++)
            for (int k = 0; k < EquationCount; k++)
                Vm[j, k] = EquationSolutions[k].Inputs[j] - EquationSolutions[EquationCount].Inputs[j];

        double[] u = LinearAlgebra.Solve(Mm, mF0);
        double[] delta = LinearAlgebra.Product(Vm, u);

        for (int j = 0; j < EquationCount; j++)
            suggestedXs[j] = EquationSolutions[EquationCount].Inputs[j] + delta[j];

        double[] solvedXs = Equation.Calculate(suggestedXs);

        return new EquationSolution(suggestedXs, solvedXs);
    }

    /// <summary>
    /// Add a point with Xs randomly offset from the Xs of the first point
    /// </summary>
    private EquationSolution GetPoint_NearFirstPoint()
    {
        const double randomness = 4; // TODO: could this value be optimized?

        double[] suggestedXs = BestSolution.Inputs.Select(x => x * (Rand.NextDouble() - 0.5) * randomness).ToArray();

        double[] solvedXs = Equation.Calculate(suggestedXs);

        return new EquationSolution(suggestedXs, solvedXs);
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

        double[] solvedXs = Equation.Calculate(suggestedXs);

        return new EquationSolution(suggestedXs, solvedXs);
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
            var equationXs = EquationSolutions.Select(x => x.Inputs[equationIndex]);
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

        double[] solvedXs = Equation.Calculate(suggestedXs);

        return new EquationSolution(suggestedXs, solvedXs);
    }
}
