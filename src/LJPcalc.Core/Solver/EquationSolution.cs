namespace LJPcalc.Core.Solver;

public class EquationSolution
{
    public readonly double[] Inputs;

    public readonly double[] Errors;

    public double MaxAbsoluteError => Errors.Select(value => Math.Abs(value)).Max();

    public EquationSolution(double[] inputs, double[] errors)
    {
        Inputs = inputs;
        Errors = errors;
    }
}
