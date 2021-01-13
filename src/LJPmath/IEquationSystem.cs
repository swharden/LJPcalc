using System;
using System.Collections.Generic;
using System.Text;

namespace LJPmath
{
    /// <summary>
    /// A system of equations that can be solved by the Solver
    /// </summary>
    public interface IEquationSystem
    {
        /// <summary>
        /// Vectorial equations in the form f(x) = 0.
        /// The solution is an error (f vs x) less than 1.
        /// A valid solution is a value of x such that f_j(x) is between -1 and 1.
        /// </summary>
        /// <param name="x">unknowns</param>
        /// <param name="f">value of f(x) that must be zero</param>
        void Equations(double[] x, double[] f);

        /// <summary>
        /// Number of equations and unknowns
        /// </summary>
        int GetEquationCount { get; }
    }
}
