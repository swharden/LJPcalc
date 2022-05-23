namespace LJPcalc.Core.KnownIonSets;

public class PatchHighCl : IKnownIonSet
{
    /*  Start with the standard K-gluconate internal then
     *  increase KCl by 30 mM (20 -> 50) and decrease Kglu by 30 (110 -> 80)
     *  [K] doesn't change, but Cl rises by 30 and Glu drops by 30
     */

    public string Name => "Patch Clamp (high [Cl])";

    public string Details => "This ion set is a modified version of the Watz and Wolfgang (2007) patch-clamp solution where 30 mM of K-gluconate was replaced with 30 mM KCl.";

    public double Temperature_C => 25;

    public double Ljp_mV => +12.441;

    public double Accuracy_mV => .5;

    public Ion[] Ions => new Ion[]
    {
        new Ion("Ca", 0, 2.4),
        new Ion("Cl", 2 + 30, 133.8),
        new Ion("Gluconate", 125 - 30, 0),
        new Ion("H2PO4", 0, 1.2),
        new Ion("HCO3", 0, 25),
        new Ion("HEPES", 10, 0),
        new Ion("K", 125, 3),
        new Ion("Mg", 1, 1.5),
        new Ion("SO4", 0, 1.5),
        new Ion("Na", 4.25, 152.2)
    };
}
