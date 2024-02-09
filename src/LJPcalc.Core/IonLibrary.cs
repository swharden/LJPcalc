namespace LJPcalc.Core;

public static class IonLibrary
{
    public readonly static Ion[] KnownIons = GetKnownIons();

    /// <summary>
    /// Given an array of ions, look up the conductivity for each and return the result as a new list.
    /// </summary>
    public static Ion[] Lookup(Ion[] input)
    {
        Ion[] output = new Ion[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            Ion ion = input[i];
            Ion match = Lookup(input[i].Name);
            output[i] = new Ion(match.Name, match.Charge, match.Conductivity, ion.C0, ion.CL, ion.Phi);
        }

        return output;
    }

    /// <summary>
    /// Return the ion with the same name (or one with zero everything if not)
    /// </summary>
    public static Ion Lookup(string name)
    {
        foreach (Ion ion in KnownIons)
        {
            if (ion.Name.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
                return ion;
        }

        return new Ion(name, 0, 0);
    }

    public static Ion[] GetKnownIons(bool removeDuplicates = true)
    {
        List<Ion> ions = new()
        {
            // Inorganic Cations
            MakeReferenceIon("Ag", +1, 61.9),
            MakeReferenceIon("Al", +3, 61),
            MakeReferenceIon("Ba", +2, 63.9),
            MakeReferenceIon("Be", +2, 45),
            MakeReferenceIon("Ca", +2, 59.5),
            MakeReferenceIon("Cd", +2, 54),
            MakeReferenceIon("Ce", +3, 70),
            MakeReferenceIon("Co", +2, 53),
            MakeReferenceIon("Co(NH3)6", +3, 100),
            MakeReferenceIon("Co(ethylenediamine)3", +3, 74.7),
            MakeReferenceIon("Cr", +3, 67),
            MakeReferenceIon("Cs", +1, 77.3),
            MakeReferenceIon("Cu", +2, 56.6),
            MakeReferenceIon("D", +1, 213.7),
            MakeReferenceIon("Dy", +3, 65.7),
            MakeReferenceIon("Er", +3, 66),
            MakeReferenceIon("Eu", +3, 67.9),
            MakeReferenceIon("FeII", +2, 53.5),
            MakeReferenceIon("FeIII", +3, 69),
            MakeReferenceIon("Gd", +3, 67.4),
            MakeReferenceIon("H", +1, 350.1),
            MakeReferenceIon("Hg2", +2, 68.7),
            MakeReferenceIon("Hg", +2, 63.6),
            MakeReferenceIon("Ho", +3, 66.3),
            MakeReferenceIon("K", +1, 73.5),
            MakeReferenceIon("La", +3, 69.6),
            MakeReferenceIon("Li", +1, 38.69),
            MakeReferenceIon("Mg", +2, 53.06),
            MakeReferenceIon("Mn", +2, 53.5),
            MakeReferenceIon("NH", 4, 73.7),
            MakeReferenceIon("N2H5", +1, 59),
            MakeReferenceIon("Na", +1, 50.11),
            MakeReferenceIon("Nd", +3, 69.6),
            MakeReferenceIon("Ni", +2, 50),
            MakeReferenceIon("Pb", +2, 71),
            MakeReferenceIon("Pr", +3, 69.6),
            MakeReferenceIon("Ra", +2, 66.8),
            MakeReferenceIon("Rb", +1, 77.8),
            MakeReferenceIon("Sc", +3, 64.7),
            MakeReferenceIon("Sm", +3, 68.5),
            MakeReferenceIon("Sr", +2, 59.46),
            MakeReferenceIon("Tl", +1, 74.9),
            MakeReferenceIon("Tm", +3, 65.5),
            MakeReferenceIon("UO2", +2, 32),
            MakeReferenceIon("Y", +3, 62),
            MakeReferenceIon("Yb", +3, 65.2),
            MakeReferenceIon("Zn", +2, 52.8),

            // Inorganic Anions
            MakeReferenceIon("Au(CN)2", -1, 50),
            MakeReferenceIon("Au(CN)4", -1, 36),
            MakeReferenceIon("B(C6H5)4", -1, 21),
            MakeReferenceIon("Br", -1, 78.1),
            MakeReferenceIon("Br3", -1, 43),
            MakeReferenceIon("BrO3", -1, 55.7),
            MakeReferenceIon("Cl", -1, 76.31),
            MakeReferenceIon("ClO2", -1, 52),
            MakeReferenceIon("ClO3", -1, 64.6),
            MakeReferenceIon("ClO4", -1, 67.3),
            MakeReferenceIon("CN", -1, 78),
            MakeReferenceIon("CO3", -2, 69.3),
            MakeReferenceIon("Co(CN)6", -3, 98.9),
            MakeReferenceIon("CrO4", -2, 85),
            MakeReferenceIon("F", -1, 55.4),
            MakeReferenceIon("Fe(CN)6 IIII", -4, 110.4),
            MakeReferenceIon("Fe(CN)6 III", -3, 100.9),
            MakeReferenceIon("H2AsO4", -1, 34),
            MakeReferenceIon("HCO3", -1, 44.5),
            MakeReferenceIon("HF2", -1, 75),
            MakeReferenceIon("HPO4", -2, 33),
            MakeReferenceIon("H2PO4", -1, 33),
            MakeReferenceIon("HS", -1, 65),
            MakeReferenceIon("HSO3", -1, 50),
            MakeReferenceIon("HSO4", -1, 50),
            MakeReferenceIon("H2SbO4", -1, 31),
            MakeReferenceIon("I", -1, 76.9),
            MakeReferenceIon("IO3", -1, 40.5),
            MakeReferenceIon("IO4", -1, 54.5),
            MakeReferenceIon("MnO4", -1, 61.3),
            MakeReferenceIon("MoO4", -2, 74.5),
            MakeReferenceIon("N3", -1, 69.5),
            MakeReferenceIon("N(CN)2", -1, 54.5),
            MakeReferenceIon("NO2", -1, 71.8),
            MakeReferenceIon("NO3", -1, 71.42),
            MakeReferenceIon("NH2SO3 (sulfamate)", -1, 48.6),
            MakeReferenceIon("OCN (cyanate)", -1, 64.6),
            MakeReferenceIon("OH", -1, 198),
            MakeReferenceIon("PF6", -1, 56.9),
            MakeReferenceIon("PO3F", -2, 63.3),
            MakeReferenceIon("PO4", -3, 69),
            MakeReferenceIon("P2O7", -4, 96),
            MakeReferenceIon("P3O9", -3, 83.6),
            MakeReferenceIon("P3O10", -5, 109),
            MakeReferenceIon("ReO4", -1, 54.9),
            MakeReferenceIon("SCN (thiocyanate)", -1, 66.5),
            MakeReferenceIon("SeCN", -1, 64.7),
            MakeReferenceIon("SeO4", -2, 75.7),
            MakeReferenceIon("SO3", -2, 79.9),
            MakeReferenceIon("SO4", -2, 80),
            MakeReferenceIon("S2O3", -2, 85),
            MakeReferenceIon("S2O4", -2, 66.5),
            MakeReferenceIon("S2O6", -2, 93),
            MakeReferenceIon("S2O8", -2, 86),
            MakeReferenceIon("WO4", -2, 69.4),

            // Organic Cations
            MakeReferenceIon("Decylpyridinium", +1, 29.5),
            MakeReferenceIon("Diethylammonium", +1, 42),
            MakeReferenceIon("Dimethylammonium", +1, 51.5),
            MakeReferenceIon("Dipropylammonium", +1, 30.1),
            MakeReferenceIon("Dodecylammonium", +1, 23.8),
            MakeReferenceIon("Ethylammonium", +1, 47.2),
            MakeReferenceIon("Ethyltrimethylammonium", +1, 40.5),
            MakeReferenceIon("Isobutylammonium", +1, 38),
            MakeReferenceIon("Methylammonium", +1, 58.3),
            MakeReferenceIon("Piperidinium", +1, 37.2),
            MakeReferenceIon("Propylammonium", +1, 40.8),
            MakeReferenceIon("Tetrabutylammonium", +1, 19.5),
            MakeReferenceIon("Tetraethylammonium", +1, 32.6),
            MakeReferenceIon("Tetramethylammonium", +1, 44.9),
            MakeReferenceIon("Tetrapropylammonium", +1, 23.5),
            MakeReferenceIon("Triethylsulfonium", +1, 36.1),
            MakeReferenceIon("Trimethylammonium", +1, 47.2),
            MakeReferenceIon("Trimethylsulfonium", +1, 51.4),
            MakeReferenceIon("Tripropylammonium", +1, 26.1),

            // Organic Anions
            MakeReferenceIon("Acetate", -1, 41),
            MakeReferenceIon("Benzoate", -1, 32.4),
            MakeReferenceIon("Bromoacetate", -1, 39.2),
            MakeReferenceIon("Bromobenzoate", -1, 30),
            MakeReferenceIon("Butanoate", -1, 32.6),
            MakeReferenceIon("Chloroacetate", -1, 42.2),
            MakeReferenceIon("m-Chlorobenzoate", -1, 31),
            MakeReferenceIon("o-Chlorobenzoate", -1, 30.5),
            MakeReferenceIon("Citrate", -3, 70.2),
            MakeReferenceIon("Crotonate", -1, 33.2),
            MakeReferenceIon("Cyanoacetate", -1, 43.4),
            MakeReferenceIon("Cyclohexanecarboxylate", -1, 28.7),
            MakeReferenceIon("Cyclopropane-1,3-dicarboxylate2", -1, 53.4),
            MakeReferenceIon("Decylsulfonate", -1, 26),
            MakeReferenceIon("Dichloroacetate", -1, 38.3),
            MakeReferenceIon("Diethylbarbiturate", -2, 26.3),
            MakeReferenceIon("Dihydrogencitrate", -1, 30),
            MakeReferenceIon("Dimethylmalonate", -2, 49.4),
            MakeReferenceIon("3, 5-Dinitrobenzoate", -1, 28.3),
            MakeReferenceIon("Dodecylsulfonate", -1, 24),
            MakeReferenceIon("Ethylmalonate", -1, 49.3),
            MakeReferenceIon("Ethylsulfonate", -1, 39.6),
            MakeReferenceIon("Fluoroacetate", -1, 44.4),
            MakeReferenceIon("Fluorobenzoate", -1, 33),
            MakeReferenceIon("Formate", -1, 54.6),
            MakeReferenceIon("Fumarate", -2, 61.8),
            MakeReferenceIon("Glutarate", -2, 52.6),
            MakeReferenceIon("Hydrogenoxalate", -1, 40.2),
            MakeReferenceIon("Iodoacetate", -1, 40.6),
            MakeReferenceIon("Lactate", -1, 38.8),
            MakeReferenceIon("Malate", -2, 58.8),
            MakeReferenceIon("Malonate", -1, 63.5),
            MakeReferenceIon("3-Methylbutanoate", -1, 32.7),
            MakeReferenceIon("Methylsulfonate", -1, 48.8),
            MakeReferenceIon("Naphthylacetate", -1, 28.4),
            MakeReferenceIon("1, 8-Octanedioate", -2, 36),
            MakeReferenceIon("Octylsulfonate", -1, 29),
            MakeReferenceIon("Oxalate", -2, 74.11),
            MakeReferenceIon("Phenylacetate", -1, 30.6),
            MakeReferenceIon("m-Phthalate", -2, 54.7),
            MakeReferenceIon("o-Phthalate", -2, 52.3),
            MakeReferenceIon("Picrate", -1, 30.37),
            MakeReferenceIon("Propanoate", -1, 35.8),
            MakeReferenceIon("Propylsulfonate", -1, 37.1),
            MakeReferenceIon("Salicylate", -1, 36),
            MakeReferenceIon("Succinate", -2, 58.8),
            MakeReferenceIon("Tartrate", -2, 59.6),
            MakeReferenceIon("Trichloroacetate", -1, 36.6),
            MakeReferenceIon("Trimethylacetate", -1, 31.9),

            // UNSW Medicine
            // https://medicalsciences.med.unsw.edu.au/research/research-services/ies/ionicmobilitytables
            MakeReferenceIonScaledToK("Choline", +1, 0.51),
            MakeReferenceIonScaledToK("Cs", +1, 1.05),
            MakeReferenceIonScaledToK("K", +1, 1),
            MakeReferenceIonScaledToK("Li", +1, 0.526),
            MakeReferenceIonScaledToK("NH4 (Ammonium)", +1, 1.001),
            MakeReferenceIonScaledToK("Na", +1, 0.682),
            MakeReferenceIonScaledToK("Rb", +1, 1.059),
            MakeReferenceIonScaledToK("TEA (TetraethylAmmonium)", +1, 0.444),
            MakeReferenceIonScaledToK("TMA (TetramethylAmmonium)", +1, 0.611),
            MakeReferenceIonScaledToK("Acetate", -1, 0.556),
            MakeReferenceIonScaledToK("Benzoate", -1, 0.441),
            MakeReferenceIonScaledToK("Br", -1, 1.063),
            MakeReferenceIonScaledToK("Cl", -1, 1.0382),
            MakeReferenceIonScaledToK("ClO4 (Perchlorate)", -1, 0.916),
            MakeReferenceIonScaledToK("F", -1, 0.753),
            MakeReferenceIonScaledToK("H2PO", -1, 0.45),
            MakeReferenceIonScaledToK("HCO3", -1, 0.605),
            MakeReferenceIonScaledToK("I", -1, 1.0456),
            MakeReferenceIonScaledToK("NO3 (Nitrate)", -1, 0.972),
            MakeReferenceIonScaledToK("Picrate", -1, 0.411),
            MakeReferenceIonScaledToK("Propionate", -1, 0.487),
            MakeReferenceIonScaledToK("SCN (Thiocyanate)", -1, 0.901),
            MakeReferenceIonScaledToK("Co", +2, 0.367),
            MakeReferenceIonScaledToK("Mg", +2, 0.361),
            MakeReferenceIonScaledToK("HPO4", -1, 0.39),
            MakeReferenceIonScaledToK("SO4", -2, 0.544),
            MakeReferenceIonScaledToK("NMDG", +1, 0.33),
            MakeReferenceIonScaledToK("Tris", +1, 0.4),
            MakeReferenceIonScaledToK("Aspartate", -1, 0.3),
            MakeReferenceIonScaledToK("Gluconate", -1, 0.33),
            MakeReferenceIonScaledToK("Glutamate", -1, 0.26),
            MakeReferenceIonScaledToK("HEPES", -1, 0.3),
            MakeReferenceIonScaledToK("Isethionate", -1, 0.52),
            MakeReferenceIonScaledToK("MES", -1, 0.37),
            MakeReferenceIonScaledToK("MOPS", -1, 0.35),
            MakeReferenceIonScaledToK("EGTA(2-)", -2, 0.24),
            MakeReferenceIonScaledToK("EGTA(3-)", -3, 0.25),
            MakeReferenceIonScaledToK("Thallium", +1, 1.02),
            MakeReferenceIonScaledToK("Butyrate", -1, 0.44),
            MakeReferenceIonScaledToK("Citrate", -3, 0.318),
            MakeReferenceIonScaledToK("2-(Methyl-Amino) Ethanol", +1, 0.49),
            MakeReferenceIonScaledToK("N-Methylethanolamine", +1, 0.49),
            MakeReferenceIonScaledToK("Ag", +1, 0.842),
            MakeReferenceIonScaledToK("Diethylammonium", +1, 0.57),
            MakeReferenceIonScaledToK("Dimethylammonium", +1, 0.705),
            MakeReferenceIonScaledToK("Ethyltrimethylammonium", +1, 0.551),
            MakeReferenceIonScaledToK("H", +1, 4.757),
            MakeReferenceIonScaledToK("Piperidinium", +1, 0.506),
            MakeReferenceIonScaledToK("Tetrabutylammonium", +1, 0.265),
            MakeReferenceIonScaledToK("Tetrapropylammonium", +1, 0.318),
            MakeReferenceIonScaledToK("Triethylammonium", +1, 0.467),
            MakeReferenceIonScaledToK("Trimethylammonium", +1, 0.643),
            MakeReferenceIonScaledToK("Bromoacetate", -1, 0.533),
            MakeReferenceIonScaledToK("Bromobenzoate", -1, 0.41),
            //MakeReferenceIon2("Chloroacetate", -1, 0.541), // different by 
            MakeReferenceIonScaledToK("CNO (Cyanate)", -1, 0.879),
            MakeReferenceIonScaledToK("Cyanoacetate", -1, 0.59),
            MakeReferenceIonScaledToK("Dichloroacetate", -1, 0.521),
            MakeReferenceIonScaledToK("Ethylsulfate", -1, 0.539),
            MakeReferenceIonScaledToK("Ethylsulfonate", -1, 0.539),
            MakeReferenceIonScaledToK("Fluoroacetate", -1, 0.604),
            MakeReferenceIonScaledToK("Fluorobenzoate", -1, 0.45),
            MakeReferenceIonScaledToK("Formate", -1, 0.743),
            MakeReferenceIonScaledToK("Iodoacetate", -1, 0.552),
            MakeReferenceIonScaledToK("Lactate", -1, 0.528),
            MakeReferenceIonScaledToK("Methylsulfate", -1, 0.664),
            MakeReferenceIonScaledToK("methanesulfonate", -1, 0.664),
            MakeReferenceIonScaledToK("Methylsulfonate", -1, 0.664),
            MakeReferenceIonScaledToK("OH (hydroxide)", -1, 2.69),
            MakeReferenceIonScaledToK("ReO4 (Rhenate)", -1, 0.747),
            MakeReferenceIonScaledToK("Salicylate", -1, 0.49),
            MakeReferenceIonScaledToK("Trichloroacetate", -1, 0.476),
            MakeReferenceIonScaledToK("Cd", +2, 0.37),
            MakeReferenceIonScaledToK("Cu", +2, 0.365),
            MakeReferenceIonScaledToK("Fe", +2, 0.37),
            MakeReferenceIonScaledToK("Hg", +2, 0.433),
            MakeReferenceIonScaledToK("Mn", +2, 0.364),
            MakeReferenceIonScaledToK("Ni", +2, 0.337),
            MakeReferenceIonScaledToK("Pb", +2, 0.48),
            MakeReferenceIonScaledToK("Malate", -2, 0.4),
            MakeReferenceIonScaledToK("Maleate", -2, 0.421),
            MakeReferenceIonScaledToK("Oxalate", -2, 0.504),
            MakeReferenceIonScaledToK("Succinate", -2, 0.4),
            MakeReferenceIonScaledToK("Gd", +3, 0.305),
            MakeReferenceIonScaledToK("Fe", +3, 0.308),
            MakeReferenceIonScaledToK("La", +3, 0.316),
            MakeReferenceIonScaledToK("Citrate", -3, 0.318),
            MakeReferenceIonScaledToK("ATP (Adenosine 5'-Triphosphate)", -2, 0.15),
            MakeReferenceIonScaledToK("ATP (Adenosine 5'-Triphosphate)", -3, 0.15),
            MakeReferenceIonScaledToK("ATP (Adenosine 5'-Triphosphate)", -4, 0.15),
            MakeReferenceIonScaledToK("2-AP (2-aminopyridine)", +1, 0.45),
            MakeReferenceIonScaledToK("3-AP (3-aminopyridine)", +1, 0.46),
            MakeReferenceIonScaledToK("4-AP (4-aminopyridine)", +2, 0.29),

            // TODO: decide how to report GTP, whose mobility is not reported last I checked
            // https://github.com/swharden/LJPcalc/issues/25
            MakeReferenceIon("GTP", 1, 0),

            // By listing such molecules here we confirm we know they exist,
            // and remind users they are probably not important for caculating LJP.
            MakeReferenceIon("Glucose", 0, 0),
            MakeReferenceIon("Sucrose", 0, 0),
            MakeReferenceIon("Dextrose", 0, 0),
            MakeReferenceIon("Phosphocreatine", 0, 0),
            MakeReferenceIon("HEPES", 0, 0),
        };

        if (removeDuplicates)
        {
            // keep the first instance of duplicated ions
            HashSet<string> seenIons = new();
            List<Ion> uniqueIons = new();
            foreach (Ion ion in ions)
            {
                if (!seenIons.Contains(ion.NameWithCharge))
                {
                    seenIons.Add(ion.NameWithCharge);
                    uniqueIons.Add(ion);
                }
            }
            ions = uniqueIons;
        }

        return ions.OrderBy(x => x.Name).ToArray();
    }

    private static Ion MakeReferenceIon(string name, int charge, double conductance)
    {
        return new(name, charge, conductance, 0, 0, 0);
    }

    private static Ion MakeReferenceIonScaledToK(string name, int charge, double scaledConductance)
    {
        double conductance = scaledConductance * Constants.KConductance * Math.Abs(charge);

        return new(name, charge, Math.Round(conductance, 6), 0, 0, 0);
    }
}
