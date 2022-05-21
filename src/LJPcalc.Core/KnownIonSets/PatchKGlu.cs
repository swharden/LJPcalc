namespace LJPcalc.Core.KnownIonSets;

public class PatchKGlu : IKnownIonSet
{
    public string Name => "Patch Clamp (K-glu)";

    public string Details => "This ion set is from Patch-Clamp Analysis (Watz and Wolfgang, 2007) Second Edition page 191 (http://springer.com/gp/book/9781588297051).";

    public double Temperature_C => 25;

    public double Ljp_mV => +15.729;

    public double Accuracy_mV => .5;

    public Ion[] Ions => new Ion[]
    {
        new Ion("Ca", 0, 2.4),
        new Ion("Cl", 2, 133.8),
        new Ion("Gluconate", 125, 0),
        new Ion("H2PO4", 0, 1.2),
        new Ion("HCO3", 0, 25),
        new Ion("HEPES", 10, 0),
        new Ion("K", 125, 3),
        new Ion("Mg", 1, 1.5),
        new Ion("SO4", 0, 1.5),
        new Ion("Na", 4.25, 152.2)
    };
}
