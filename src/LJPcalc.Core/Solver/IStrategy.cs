namespace LJPcalc.Core.Solver;

public interface IStrategy
{
    double[] SuggestXs(int count, PhiEquationSolution[] solutions, Random rand);
}