namespace LJPcalc.Core.KnownIonSets;

public class Owen2013 : IKnownIonSet
{
    /* I found this set highly problematic for the electronegativity solver */

    public string Name => "Owen et al., 2013";

    public string Details => "This ion set came from a publication (https://www.nature.com/articles/nature12330) which indicated pClamp was used to correct for LJP.";

    public double Temperature_C => 33;

    public double Ljp_mV => 15.1 - 3.3; // why?

    public double Accuracy_mV => 1;

    /* For voltage ramp recordings, the internal solution
     * contained (in mM) 50 K-Gluconate, 70 CsMeSO3, 10 TEA-Cl, 1 MgCl2, 10
     * HEPES, 0.3 EGTA, 10 Tris-phosphocreatine, 4 Mg-ATP, and 0.3 Na-GTP. The
     * pipette reference potential was set to zero and a junction potential of 215.1 mV
     * (calculated using pClamp) was corrected post hoc.
     */

    // internal
    // K, 50
    // Gluconate, 50
    // 70 Cs
    // 70 MeSO3
    // 10 TEA
    // 10 + 2 = 12 Cl
    // 1 + 4 = 5 Mg
    // 10 HEPES
    // 0.3 EGTA
    // 10 Tris-phosphocreatine
    // 4 ATP
    // 0.3 Na
    // 0.3 GTP

    /* For recordings from rat tissue, ACSF contained (in mM)
     * 122 NaCl, 3 KCl, 10 D-glucose, 1.25 NaH2PO4, 2 CaCl2, 1.3 MgCl2, 26 NaHCO3,
     * 3 sodium pyruvate, 2 sodium ascorbate and 5 L-glutamine.
     */

    // external
    // 122 + 26 + 2 + 3 + 1.25 = 154.25 Na
    // 122 + 3 = 125 Cl
    // 3 K
    // 10 D-glucose
    // 1.25 H2PO4
    // 2 Ca
    // 4 Cl
    // 1.3 Mg
    // 2.6 Cl
    // 26 HCO3
    // 3 Pyruvate
    // 2 ascorbate
    // 5 L-glutamine

    public Ion[] Ions => new Ion[]
    {
        new Ion("K", 50, 3),
        new Ion("Gluconate", 50, 0),
        new Ion("Cs", 70, 0),
        new Ion("Methylsulfonate", 70, 0),
        new Ion("TEA (TetraethylAmmonium)", 10, 0),
        new Ion("Cl", 12, 131.6),
        new Ion("Mg", 5, 1.3),
        new Ion("HEPES", 10, 0),
        new Ion("EGTA(2-)", .3, 0),
        new Ion("Tris", 10, 0),
        new Ion("ATP (Adenosine 5'-Triphosphate)", 4 + 0.3, 0),
        new Ion("Na", 0.3, 154.25),
        new Ion("HCO3", 0, 26),
        new Ion("H2PO4", 0, 1.25),
        new Ion("Ca", 0, 2),
    };
}
