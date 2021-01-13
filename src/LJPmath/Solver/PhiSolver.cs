using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LJPmath.Solver
{
    public class PhiSolver
    {
        public readonly double[] SolvedPhis;
        public readonly double SolutionM;

        /// <summary>
        /// Determines phi for all ions except the last two. 
        /// The solution is found when CLs are suffeciently close to those defined in the ion table.
        /// </summary>
        public PhiSolver(Ion[] ions, double temperatureC, double timeoutMilliseconds, bool throwIfTimeout)
        {
            // phis are initialized to the difference in concentration on each side of the junction
            double[] phis = Enumerable.Range(0, ions.Length - 2)
                                      .Select(x => ions[x])
                                      .Select(x => x.cL - x.c0)
                                      .ToArray();

            if (ions.Length > 2)
            {
                IEquationSystem equationSystem = new PhiEquationSystem(ions, temperatureC);
                var equationSolver = new EquationSolver(equationSystem);
                SolutionM = equationSolver.Solve(phis, timeoutMilliseconds, throwIfTimeout);
            }

            SolvedPhis = phis;
        }
    }
}
