namespace LJPcalc.Core.Solver;

class EquationPoint
{
    /// <summary>
    /// inputs
    /// </summary>
    public readonly double[] X;

    /// <summary>
    /// outputs (f(x) for each input)
    /// </summary>
    public readonly double[] F;

    /// <summary>
    /// largest absolute value of the output (F)
    /// </summary>
    public readonly double FMax;

    public EquationPoint(double[] x, IEquationSystem equationSystem)
    {
        X = x;
        F = new double[equationSystem.EquationCount];
        equationSystem.Calculate(x, F);
        FMax = F.Select(value => Math.Abs(value)).Max();
    }
}
