namespace LJPcalc.Core.KnownIonSets;

public class PatchKGlu : IKnownIonSet
{
    public string Name => "Patch Clamp (K gluconate)";

    public string Details => "This ion set is from Patch-Clamp Analysis (Watz and Wolfgang, 2007) " +
        "Second Edition page 191 (http://springer.com/gp/book/9781588297051).";

    public double Temperature_C => 25;

    public double Ljp_mV => +13.469093234512627d;

    public double Accuracy_mV => .1;

    /* Our standard solution contains the following (in mM): 
     * 110 K-gluconate, 
     * 10 N-[2-Hydroxyethyl]piperazine-N′-[2-ethanesulfonic acid] (HEPES), 
     * 1.0 ethyleneglycotetraacetic acid (EGTA), 
     * 20 KCl, 
     * 2.0 MgCl2,
     * 2.0 Na2ATP, 
     * 0.25 Na3GTP, 
     * 10 phosphocreatine (di-tris)
     */

    // INTERNAL
    // K 110
    // gluconate 110
    // HEPES 10
    // EGTA 1.0
    // K 20
    // Cl 20
    // Mg 2
    // Cl 4
    // Na 4
    // ATP 2
    // omit GTP and phosphocreatine

    /* ACSF contains (in mM): 126 NaCl, 11 D-glucose, 1.5 MgSO4,
     * 3 KCL, 1.2 NaH2PO4, 2.4 CaCl2, 25 NaHCO3 */

    // EXTERNAL
    // 126 + 25 + 1.2 = 152.2 Na
    // 126 + 3 + 4.8 = 133.8 Cl
    // 1.5 Mg
    // 1.5 SO4
    // 3 K
    // 1.2 H2PO4
    // 2.4 Ca
    // 25 HCO3

    public Ion[] Ions => new Ion[]
    {
        new Ion("K", 110 + 20, 3),
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
