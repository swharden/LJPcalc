namespace LJPcalc.Core.KnownIonSets;

public class PatchKMeSO : IKnownIonSet
{
    public string Name => "Patch Clamp (K methanesulfonate)";

    public string Details => "Modified K-gluconate pipette solution where gluconate is replaced with methanesulfonate";

    public double Temperature_C => 25;

    public double Ljp_mV => +8.5940720894670459d;

    public double Accuracy_mV => .1;

    public Ion[] Ions => new Ion[]
    {
        new Ion("K", 110 + 20, 3),
        new Ion("Methanesulfonate", 110, 0),
        new Ion("HEPES", 5, 0), // assume 5 NaOH was added to tirate 10 mM HEPES
        new Ion("EGTA", 1, 0),
        new Ion("Cl", 20 + 4, 133.8),
        new Ion("Mg", 2, 1.5),
        new Ion("Na", 4 + 5, 152.2),
        new Ion("ATP (Adenosine 5'-Triphosphate)", 2, 0),
        new Ion("SO4", 0, 1.5),
        new Ion("H2PO4", 0, 1.2),
        new Ion("Ca", 0, 2.4),
        new Ion("HCO3", 0, 25),
    };
}
