namespace LJPcalc.Core.Solver;

public class PhiEquationSolution
{
    /// <summary>
    /// Values passed into the calculation system
    /// </summary>
    public readonly double[] Phis;

    /// <summary>
    /// Resulting values after the calculation
    /// </summary>
    public readonly double[] SolvedCLs;

    /// <summary>
    /// Scaled error of each output relative to expectation.
    /// Iterative solvers work to minimize the largest error.
    /// An equation set is typically considered solved when the largest error is < 1%.
    /// </summary>
    public readonly double[] Errors;

    public readonly double LjpVolts;
    public double LjpMillivolts => LjpVolts * 1000;

    public double MaxAbsoluteError => Errors.Any() ? Errors.Select(value => Math.Abs(value)).Max() : 0;

    public PhiEquationSolution(double[] phis, double[] solvedCLs, double[] solutionError, double ljpVolts)
    {
        Phis = phis;
        SolvedCLs = solvedCLs;
        Errors = solutionError;
        LjpVolts = ljpVolts;
    }

    public override string ToString()
    {
        if (MaxAbsoluteError > 1)
            return $"LJP = {LjpMillivolts} (max CL error: {Math.Round(MaxAbsoluteError, 1)}%)";
        else
            return $"LJP = {LjpMillivolts} (max CL error: {MaxAbsoluteError:#.##E+0}%)";
    }
}
