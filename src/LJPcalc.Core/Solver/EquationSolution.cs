namespace LJPcalc.Core.Solver;

public class EquationSolution
{
    public readonly double[] Inputs;

    public readonly double[] Outputs;

    public double AbsoluteLargestOutput => Outputs.Select(value => Math.Abs(value)).Max();

    public EquationSolution(double[] inputs, double[] outputs)
    {
        Inputs = inputs;
        Outputs = outputs;
    }
}
