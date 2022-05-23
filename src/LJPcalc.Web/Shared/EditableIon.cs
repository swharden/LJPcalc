namespace LJPcalc.Web.Shared;

public class EditableIon
{
    public int Index { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Charge { get; set; }
    public double Conductivity { get; set; }
    public double Mobility => LJPcalc.Core.Constants.Mobility(Conductivity, Charge);
    public double C0 { get; set; }
    public double CL { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsNewIon { get; set; } = false;

    public LJPcalc.Core.Ion ToIon()
    {
        return new Core.Ion(Name, Charge, Conductivity, C0, CL);
    }

    public void Reset()
    {
        Index = -1;
        Name = string.Empty;
        Charge = 0;
        Conductivity = 0;
        C0 = 0;
        CL = 0;
    }

    public void UpdateFromIon(Core.Ion[] ions, int index)
    {
        Index = index;
        Name = ions[index].Name;
        Charge = ions[index].Charge;
        Conductivity = ions[index].Conductivity;
        C0 = ions[index].C0;
        CL = ions[index].CL;
    }

    public override string ToString()
    {
        string plus = Charge > 0 ? "+" : string.Empty;
        return $"{Name} ({plus}{Charge})";
    }
}
