namespace LJPcalc.Core.Solver;

public static class PhiSolver
{
    /// <summary>
    /// Determines phi for all ions except the last two. 
    /// The solution is found when CLs are suffeciently close to those defined in the ion table.
    /// </summary>
    public static double[] Solve(Ion[] ions, double temperatureC, double timeoutMilliseconds, bool throwIfTimeout)
    {
        double[] phis = ions.Take(ions.Length - 2).Select(x => x.CL - x.C0).ToArray();

        if (ions.Length <= 2)
        {
            return phis;
        }
        else
        {
            IEquationSystem equationSystem = new PhiEquationSystem(ions, temperatureC);
            EquationSolver equationSolver = new(equationSystem);
            double[] solvedPhis = equationSolver.Solve(phis, timeoutMilliseconds, throwIfTimeout);
            return solvedPhis;
        }
    }
}
