namespace LJPmath.Solver
{
    /// <summary>
    /// A system of equations that can be solved by the Solver
    /// </summary>
    public interface IEquationSystem
    {
        /// <summary>
        /// Vectorial equations in the form f(x) = 0.
        /// A valid solution is a set of xs where for every x, f(x) is between -1 and 1.
        /// </summary>
        /// <param name="x">unknowns</param>
        /// <param name="f">f(x)</param>
        void Calculate(double[] x, double[] f);

        /// <summary>
        /// Number of equations and unknowns
        /// </summary>
        int EquationCount { get; }
    }
}
