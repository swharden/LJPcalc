namespace LJPcalc.Core.Solver;

class EquationSolver
{
    private readonly IEquationSystem Equations;
    private int EquationCount => Equations.EquationCount;

    private EquationPoint[] PossibleSolutions = Array.Empty<EquationPoint>();
    public EquationPoint BestSolution => PossibleSolutions[0];

    private readonly Random Rand = new(0);

    private int Iterations;

    public EquationSolver(IEquationSystem equations)
    {
        Equations = equations;
    }

    /// <summary>
    /// Solve the equations and return the best solution
    /// </summary>
    public double[] Solve(double[] x, double timeoutMilliseconds, bool throwIfTimeout)
    {
        if (EquationCount == 0)
            throw new Exception("equation count cannot be 0");

        System.Diagnostics.Stopwatch stopwatch = new();
        stopwatch.Start();

        PossibleSolutions = new EquationPoint[] { new EquationPoint(x, Equations) };

        while (BestSolution.FMax > 1.0)
        {
            PossibleSolutions = PossibleSolutions
                .Append(GetSuggestedPoint())
                .OrderBy(x => Math.Abs(x.FMax))
                .Take(EquationCount * 4)
                .ToArray();

            if (stopwatch.ElapsedMilliseconds > timeoutMilliseconds)
            {
                if (throwIfTimeout)
                    throw new OperationCanceledException($"Solver timed out while calculating Phis ({timeoutMilliseconds} ms)");
                break;
            }
        }

        double[] bestSolutionXs = Enumerable.Range(0, EquationCount).Select(x => BestSolution.X[x]).ToArray();

        return bestSolutionXs;
    }

    private EquationPoint GetSuggestedPoint()
    {
        Func<EquationPoint>[] methods =
        {
            GetPoint_ShiftedBySolutionDelta,
            GetPoint_NearFirstPoint,
            GetPoint_TotallyRandom,
            GetPoint_ConsideringMinMax,
        };

        Func<EquationPoint> method = methods[Iterations++ % methods.Length];

        return method.Invoke();
    }

    /// <summary>
    /// Add a point with Xs shifted by the delta of the solved matrix
    /// </summary>
    private EquationPoint GetPoint_ShiftedBySolutionDelta()
    {
        if (PossibleSolutions.Length < EquationCount + 1)
        {
            return GetPoint_NearFirstPoint();
        }

        double[] suggestedXs = new double[EquationCount];

        double[,] Mm = new double[EquationCount, EquationCount];
        for (int j = 0; j < EquationCount; j++)
            for (int k = 0; k < EquationCount; k++)
                Mm[j, k] = PossibleSolutions[k].F[j] - PossibleSolutions[EquationCount].F[j];

        double[] mF0 = new double[EquationCount];
        for (int j = 0; j < EquationCount; j++)
            mF0[j] = -PossibleSolutions[EquationCount].F[j];

        double[,] Vm = new double[EquationCount, EquationCount];
        for (int j = 0; j < EquationCount; j++)
            for (int k = 0; k < EquationCount; k++)
                Vm[j, k] = PossibleSolutions[k].X[j] - PossibleSolutions[EquationCount].X[j];

        double[] u = LinearAlgebra.Solve(Mm, mF0);
        double[] delta = LinearAlgebra.Product(Vm, u);

        for (int j = 0; j < EquationCount; j++)
            suggestedXs[j] = PossibleSolutions[EquationCount].X[j] + delta[j];

        return new EquationPoint(suggestedXs, Equations);
    }

    /// <summary>
    /// Add a point with Xs randomly offset from the Xs of the first point
    /// </summary>
    private EquationPoint GetPoint_NearFirstPoint()
    {
        const double randomness = 4; // TODO: could this value be optimized?

        double[] suggestedXs = BestSolution.X.Select(x => x * (Rand.NextDouble() - 0.5) * randomness)
                                          .ToArray();

        return new EquationPoint(suggestedXs, Equations);
    }

    /// <summary>
    /// Add a point with totally random Xs
    /// </summary>
    private EquationPoint GetPoint_TotallyRandom()
    {
        const double randomness = 4; // TODO: could this value be optimized?

        double[] suggestedXs = Enumerable.Range(0, EquationCount)
                                         .Select(x => (Rand.NextDouble() - 0.5) * randomness)
                                         .ToArray();

        return new EquationPoint(suggestedXs, Equations);
    }

    /// <summary>
    /// Add a point with Xs randomized centered and scaled to the min/max of the existing Xs
    /// </summary>
    private EquationPoint GetPoint_ConsideringMinMax()
    {
        const double randomness = 3; // TODO: could this value be optimized?

        double[] suggestedXs = new double[EquationCount];

        for (int equationIndex = 0; equationIndex < EquationCount; equationIndex++)
        {
            var equationXs = PossibleSolutions.Select(x => x.X[equationIndex]);
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

        return new EquationPoint(suggestedXs, Equations);
    }
}
