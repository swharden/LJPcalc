﻿namespace LJPcalc.Core;

public class Ion
{
    public string Name { get; private set; } = "?";
    public int Charge { get; private set; } = 0;
    public double Conductivity { get; private set; } = 0;
    public double Mu => Conductivity / (Constants.Nav * Math.Pow(Constants.e, 2) * Math.Abs(Charge));
    public double C0 { get; set; } = 0;
    public double CL { get; set; } = 0;
    public double Phi { get; set; } = 0;
    public string NameWithCharge
    {
        get
        {
            string chargeWithSign = (Charge > 0) ? "+" + Charge.ToString() : Charge.ToString();
            return $"{Name} ({chargeWithSign})";
        }
    }

    public Ion(Ion ion)
    {
        Name = ion.Name;
        Charge = ion.Charge;
        Conductivity = ion.Conductivity;
        C0 = ion.C0;
        CL = ion.CL;
        Phi = ion.Phi;
    }

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
    }

    public override string ToString()
    {
        return $"Ion {NameWithCharge}: mu={Mu:0.000E+0}, c0={C0:0.000}, cL={CL:0.000}";
    }
}
