namespace LJPcalc.Core.Solver;

public class LjpSolution
{
    public readonly double Volts;
    public double Millivolts => Volts * 1000;
    public readonly double[] CLs;

    public LjpSolution(double volts, double[] cls)
    {
        Volts = volts;
        CLs = cls;
    }
}
