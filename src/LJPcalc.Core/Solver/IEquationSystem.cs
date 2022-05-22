namespace LJPcalc.Core.Solver;

public interface IEquationSystem
{
    /// <summary>
    /// Solve the equation system using the given inputs and return the error
    /// </summary>
    double[] Calculate(double[] x);
}
