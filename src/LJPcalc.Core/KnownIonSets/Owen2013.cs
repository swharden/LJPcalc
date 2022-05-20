namespace LJPcalc.Core.KnownIonSets;

internal class Owen2013 : IKnownIonSet
{
    /* I found this set highly problematic for the electronegativity solver */

    public string Name => "Owen et al., 2013";

    public string Details => "This ion set came from a publication (https://www.nature.com/articles/nature12330) which indicated pClamp was used to correct for LJP.";

    public double Temperature_C => 33;

    public double Ljp_mV => 15.1 - 3.3; // why?

    public double Accuracy_mV => 1;

    public Ion[] Ions => new Ion[]
    {
        new Ion("K", 50, 3),
        new Ion("Gluconate", 50, 0),
        new Ion("Cs", 70, 0),
        new Ion("HSO3", 70, 0),
        new Ion("TEA (TetraethylAmmonium)", 10, 10),
        new Ion("Cl", 12, 131.6),
        new Ion("Mg", 5, 1.3),
        new Ion("HEPES", 10, 0),
        new Ion("EGTA(2-)", .3, 0),
        new Ion("Tris", 10, 0),
        new Ion("ATP (Adenosine 5'-Triphosphate)", 4, 0),
        new Ion("Na", 0.3, 139.25),
        new Ion("HCO3", 0, 26),
        new Ion("H2PO4", 0, 1.25),
        new Ion("Ca", 0, 2),
        new Ion("4-AP (4-aminopyridine)", 0, 5),
    };
}
