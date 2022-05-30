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
    public string ChargeWithSign => (Charge > 0) ? "+" + Charge.ToString() : Charge.ToString();
    public string NameWithCharge => $"{Name.Split(" (")[0]} ({ChargeWithSign})";

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

    public Ion Clone() => new(Name, Charge, Conductivity, C0, CL, Phi);

    public override string ToString()
    {
        return $"Ion {NameWithCharge}: mu={Mu:0.000E+0}, conductivity={Conductivity}, c0={C0:0.000}, cL={CL:0.000}";
    }
}
