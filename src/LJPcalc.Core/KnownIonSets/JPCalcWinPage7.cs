namespace LJPcalc.Core.KnownIonSets;

public class JPCalcWinPage7 : IKnownIonSet
{
    public string Name => "JPCalcWin manual (page 7)";

    public string Details => "This ion set originated from the JPCalcWin manual (page 7, https://medicalsciences.med.unsw.edu.au/sites/default/files/soms/page/ElectroPhysSW/JPCalcWin-Demo%20Manual.pdf).";

    public double Temperature_C => 20;

    public double Ljp_mV => +8.74;

    public double Accuracy_mV => 0.5;

    public Ion[] Ions => new Ion[]
    {
        new Ion("Na", 10, 145),
        new Ion("Cl", 10, 145),
        new Ion("Cs", 135, 0),
        new Ion("F", 135, 0)
    };
}
