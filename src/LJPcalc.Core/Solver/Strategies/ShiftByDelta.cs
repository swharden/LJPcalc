namespace LJPcalc.Core.Solver.Strategies;

/// <summary>
/// Suggest Xs shifted by the delta of the solved matrix
/// </summary>
public class ShiftByDelta : IStrategy
{
    public double[] SuggestXs(int count, PhiEquationSolution[] solutions, Random rand)
    {
        if (solutions.Length < count + 1)
        {
            return new NearBestSolution().SuggestXs(count, solutions, rand);
        }

        double[] suggestedXs = new double[count];

        PhiEquationSolution referenceSolution = solutions[count];

        // 2D array of error differences vs. the Nth solution
        double[,] Mm = new double[count, count];
        for (int j = 0; j < count; j++)
            for (int k = 0; k < count; k++)
                Mm[j, k] = solutions[k].Errors[j] - referenceSolution.Errors[j];

        // 1d array of errors for the worst solution known
        double[] mF0 = new double[count];
        for (int j = 0; j < count; j++)
            mF0[j] = -referenceSolution.Errors[j];

        // 2D array of phis differences vs. the worst solution
        double[,] Vm = new double[count, count];
        for (int j = 0; j < count; j++)
            for (int k = 0; k < count; k++)
                Vm[j, k] = solutions[k].Phis[j] - referenceSolution.Phis[j];

        double[] u = LinearAlgebra.Solve(Mm, mF0);
        double[] delta = LinearAlgebra.Product(Vm, u);

        for (int j = 0; j < count; j++)
            suggestedXs[j] = referenceSolution.Phis[j] + delta[j];

        return suggestedXs;
    }
}
