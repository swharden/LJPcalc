namespace LJPcalc.Core.Solver;

public class EquationSolution
{
    /// <summary>
    /// Values passed into the calculation system
    /// </summary>
    public readonly double[] Inputs;

    /// <summary>
    /// Resulting values after the calculation
    /// </summary>
    public readonly double[] Outputs;

    /// <summary>
    /// Scaled error of each output relative to expectation.
    /// Iterative solvers work to minimize the largest error.
    /// An equation set is typically considered solved when the largest error is < 1%.
    /// </summary>
    public readonly double[] Errors;

    public double MaxAbsoluteError => Errors.Select(value => Math.Abs(value)).Max();

    public EquationSolution(double[] inputs, double[] outputs, double[] errors)
    {
        Inputs = inputs;
        Outputs = outputs;
        Errors = errors;
    }
}
