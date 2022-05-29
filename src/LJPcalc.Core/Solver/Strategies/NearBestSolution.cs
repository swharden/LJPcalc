namespace LJPcalc.Core.Solver.Strategies;

/// <summary>
/// Suggest Xs randomly offset from those of the best solution found so far
/// </summary>
public class NearBestSolution : IStrategy
{
    public double[] SuggestXs(int count, PhiEquationSolution[] solutions, Random rand)
    {
        const double randomness = 4; // TODO: could this value be optimized?

        PhiEquationSolution BestSolution = solutions[0];

        double[] suggestedXs = BestSolution.Phis.Select(x => x * (rand.NextDouble() - 0.5) * randomness).ToArray();

        return suggestedXs;
    }
}
