namespace LJPcalc.Core.KnownIonSets;

public class PatchCsGlu : IKnownIonSet
{
    public string Name => "Patch Clamp (Cs gluconate)";

    public string Details => "This ion set is a modified version of the Watz and Wolfgang (2007) " +
        "patch-clamp solution where K-gluconate was replaced with Cs-gluconate.";

    public double Temperature_C => 25;

    public double Ljp_mV => +14.014124840428568d;

    public double Accuracy_mV => .5;

    public Ion[] Ions => new Ion[]
    {
        new Ion("Cs", 110 + 20, 3),
        new Ion("Gluconate", 110, 0),
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
