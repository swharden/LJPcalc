namespace LJPcalc.Core;

public class Ion
{
    public readonly string Name;
    public readonly int Charge;
    public readonly double Conductivity;

    public double Mu => Conductivity / (Constants.Nav * Math.Pow(Constants.e, 2) * Math.Abs(Charge));
    public readonly double InitialC0;
    public readonly double InitialCL;
    public double PercentChangeC0 => 100 * Math.Abs(InitialC0 - C0) / InitialC0;
    public double PercentChangeCL => 100 * Math.Abs(InitialCL - CL) / InitialCL;

    public double C0 { get; set; }
    public double CL { get; set; }
    public double Phi { get; set; }

    public Ion(string name, double c0, double cL)
    {
        Name = name;
        C0 = c0;
        CL = cL;
    }

    public Ion(string name, int charge, double conductivity, double c0, double cL, double phi = 0)
    {
        Name = name;
        Charge = charge;
        Conductivity = conductivity;

        C0 = c0;
        CL = cL;
        Phi = phi;

        InitialC0 = c0;
        InitialCL = cL;
    }

    public override string ToString()
    {

        string chargeWithSign = (Charge > 0) ? "+" + Charge.ToString() : Charge.ToString();
        string nameWithCharge = $"{Name} ({chargeWithSign})";

        return $"Ion {nameWithCharge}: mu={Mu:0.000E+0}, c0={C0:0.000}, cL={CL:0.000}";
    }
}
