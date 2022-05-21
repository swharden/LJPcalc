namespace LJPcalc.Core.Solver;

/// <summary>
/// Vectorial equations in the form f(x) = 0
/// A valid solution is a set of xs where for every x, f(x) is between -1 and 1.
/// </summary>
interface IEquationSystem
{
    double[] Calculate(double[] x);

    /// <summary>
    /// Number of equations and unknowns
    /// </summary>
    int EquationCount { get; }
}
