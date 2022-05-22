namespace LJPcalc.Core.Solver;

public interface IEquation
{
    EquationSolution Calculate(double[] x);
}
