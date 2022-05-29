namespace LJPcalc.Core.Solver.Strategies;

/// <summary>
/// Suggest totally random values
/// </summary>
internal class TotallyRandom : IStrategy
{
    public double[] SuggestXs(int count, PhiEquationSolution[] solutions, Random rand)
    {
        const double randomness = 4; // TODO: could this value be optimized?

        double[] suggestedXs = Enumerable.Range(0, count)
                                         .Select(x => (rand.NextDouble() - 0.5) * randomness)
                                         .ToArray();

        return suggestedXs;
    }
}