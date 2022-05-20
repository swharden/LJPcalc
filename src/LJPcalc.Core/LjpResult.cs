namespace LJPcalc.Core;

public class LjpResult
{
    public readonly Ion[] IonsOriginal;
    public readonly Ion[] IonsSolved;
    public readonly double mV;
    public readonly double TemperatureC;
    public readonly double PhiSolutionM;

    public LjpResult(Ion[] ionsIn, Ion[] ionsOut, double tempC, double ljpMv, double phiM)
    {
        IonsOriginal = ionsIn;
        IonsSolved = ionsOut;
        mV = ljpMv;
        TemperatureC = tempC;
        PhiSolutionM = phiM;
    }
}