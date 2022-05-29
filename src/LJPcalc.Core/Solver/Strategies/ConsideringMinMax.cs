namespace LJPcalc.Core.Solver.Strategies;

/// <summary>
/// Suggest Xs randomized centered and scaled to the min/max of the existing Xs
/// </summary>
internal class ConsideringMinMax : IStrategy
{
    public double[] SuggestXs(int count, PhiEquationSolution[] solutions, Random rand)
    {
        const double randomness = 3; // TODO: could this value be optimized?

        double[] suggestedXs = new double[count];

        for (int i = 0; i < count; i++)
        {
            double xMin = solutions.Select(x => x.Phis[i]).Min();
            double xMax = solutions.Select(x => x.Phis[i]).Max();

            if (xMin == xMax)
            {
                if (xMin > 0.0)
                {
                    xMin *= 0.8;
                    xMax *= 1.2;
                }
                else if (xMin < 0.0)
                {
                    xMin *= 1.2;
                    xMax *= 0.8;
                }
                else // xMin == 0
                {
                    xMin = -1.0;
                    xMax = 1.0;
                }
            }

            double mean = (xMin + xMax) / 2.0;
            double span = xMax - xMin;
            double randomOffset = span * randomness * (rand.NextDouble() - 0.5);
            suggestedXs[i] = mean + randomOffset;
        }

        return suggestedXs;
    }
}