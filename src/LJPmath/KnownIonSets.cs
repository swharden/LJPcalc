using System;
using System.Collections.Generic;
using System.Text;

namespace LJPmath
{
    public class KnownIonSets
    {
        public readonly List<KnownIonSet> ionSets;

        public KnownIonSets(IonTable ionTable)
        {
            ionSets = new List<KnownIonSet>();

            if (ionTable is null)
                ionTable = new IonTable();

            ionSets.Add(new KnownIonSet(
                name: "JLJP screenshot",
                details: "This ion set is from the original JLJP screenshot on SourceForge (https://a.fsdn.com/con/app/proj/jljp/screenshots/GUI.png/max/max/1.jpg).",
                expectedLjp_mV: -20.79558643,
                temperatureC: 25,
                expectedAccuracy_mV: .5,
                ions: new List<Ion>(){
                        new Ion("Zn", 9, 2.84E-2),
                        new Ion("K", 0, 3),
                        new Ion("Cl", 18, 3.062)
                },
                ionTable: ionTable));

            ionSets.Add(new KnownIonSet(
                name: "Ng and Barry (1994)",
                details: "This ion set is from the bottom row of Table 2 (https://doi.org/10.1016/0165-0270(94)00087-W)",
                expectedLjp_mV: -8.2,
                temperatureC: 25,
                expectedAccuracy_mV: .5,
                ions: new List<Ion>(){
                        new Ion("Ca", 50, 0),
                        new Ion("Cl", 200, 100),
                        new Ion("Mg", 50, 0),
                        new Ion("Li", 0, 100)
                },
                ionTable: ionTable));

            ionSets.Add(new KnownIonSet(
                name: "JPCalcWin manual (page 7)",
                details: "This ion set originated from the JPCalcWin manual (page 7, https://medicalsciences.med.unsw.edu.au/sites/default/files/soms/page/ElectroPhysSW/JPCalcWin-Demo%20Manual.pdf).",
                expectedLjp_mV: +8.74,
                temperatureC: 20,
                expectedAccuracy_mV: 0.5,
                ions: new List<Ion>(){
                        new Ion("Na", 10, 145),
                        new Ion("Cl", 10, 145),
                        new Ion("Cs", 135, 0),
                        new Ion("F", 135, 0)
                },
                ionTable: ionTable));

            /* Our standard solution contains the following (in mM): 
             *   110 K-gluconate
             *   10 N-[2-Hydroxyethyl]piperazine-N′-[2-ethanesulfonic acid] (HEPES)
             *   1.0 ethyleneglycotetraacetic acid (EGTA)
             *   20 KCl
             *   2.0 MgCl2
             *   2.0 Na2ATP
             *   0.25 Na3GTP
             *   10 phosphocreatine (di-tris)
             * pH 7.3, 290 mOsm, add 0.5% biocytin
             */
            ionSets.Add(new KnownIonSet(
                name: "Patch Clamp (K-glu)",
                details: "This ion set is from Patch-Clamp Analysis (Watz and Wolfgang, 2007) Second Edition page 191 (http://springer.com/gp/book/9781588297051).",
                expectedLjp_mV: +15.729,
                temperatureC: 25,
                expectedAccuracy_mV: .5,
                ions: new List<Ion>(){
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
                },
                ionTable: ionTable));

            // increase KCl by 30 mM (20 -> 50) and decrease Kglu by 30 (110 -> 80)
            // [K] doesn't change, but Cl rises by 30 and Glu drops by 30
            ionSets.Add(new KnownIonSet(
                name: "Patch Clamp (high [Cl])",
                details: "This ion set is a modified version of the Watz and Wolfgang (2007) patch-clamp solution where 30 mM of K-gluconate was replaced with 30 mM KCl.",
                expectedLjp_mV: +12.441,
                temperatureC: 25,
                expectedAccuracy_mV: .5,
                ions: new List<Ion>(){
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
                },
                ionTable: ionTable));

            ionSets.Add(new KnownIonSet(
                name: "AxoBits Article",
                details: "This ion set came from a JPCalc screenshot in an AxoBits article about LJP corrections (Issue 39, page 8, https://medicalsciences.med.unsw.edu.au/sites/default/files/soms/page/ElectroPhysSW/LJP_article_%20AxoBits%2039.pdf).",
                expectedLjp_mV: 15.6,
                temperatureC: 20,
                expectedAccuracy_mV: .5,
                ions: new List<Ion>(){
                        new Ion("K", 145, 2.8),
                        new Ion("Na", 13, 145),
                        new Ion("Cl", 10, 148.8),
                        new Ion("Gluconate", 145, 0),
                        new Ion("Mg", 1, 2),
                        new Ion("Ca", 0, 1),
                        new Ion("HEPES", 5, 5),
                },
                ionTable: ionTable));

            ionSets.Add(new KnownIonSet(
                name: "Owen et al., 2013",
                details: "This ion set came from a publication (https://www.nature.com/articles/nature12330) which indicated pClamp was used to correct for LJP.",
                expectedLjp_mV: 15.1 - 3.3,
                temperatureC: 33,
                expectedAccuracy_mV: .5,
                ions: new List<Ion>(){
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
                },
                ionTable: ionTable));
        }
    }
}
