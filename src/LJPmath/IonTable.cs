using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LJPmath
{
    public static class IonTable
    {
        public static Ion Lookup(string name)
        {
            int charge = 0;
            double mu = 0;

            // TODO: add tests for failed lookups

            // TODO: refactor this to be more elegant

            // TODO: find the source of this list and add a proper citation

            if (name.Equals("Ag", StringComparison.InvariantCultureIgnoreCase)) { charge = +1; mu = 4.01656e+11; }
            if (name.Equals("Al", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.31939e+11; }
            if (name.Equals("Au(CN)2", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 3.24439e+11; }
            if (name.Equals("Au(CN)4", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 2.33596e+11; }
            if (name.Equals("Ba", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 2.06343e+11; }
            if (name.Equals("B(C6H5)4", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 1.36265e+11; }
            if (name.Equals("Be", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 1.45998e+11; }
            if (name.Equals("Br3", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 2.79018e+11; }
            if (name.Equals("Br", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 5.06774e+11; }
            if (name.Equals("BrO3", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 3.61425e+11; }
            if (name.Equals("Ca", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 1.92944e+11; }
            if (name.Equals("Cd", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 1.75197e+11; }
            if (name.Equals("Ce", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.50972e+11; }
            if (name.Equals("Cl", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 4.95159e+11; }
            if (name.Equals("ClO2", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 3.37417e+11; }
            if (name.Equals("ClO3", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 4.19176e+11; }
            if (name.Equals("ClO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 4.36695e+11; }
            if (name.Equals("CN", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 5.06125e+11; }
            if (name.Equals("CNO", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 4.19176e+11; }
            if (name.Equals("CO3", StringComparison.InvariantCultureIgnoreCase)) { charge = -2; mu = 2.24836e+11; }
            if (name.Equals("Co", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 1.78442e+11; }
            if (name.Equals("[Co(CN)6]", StringComparison.InvariantCultureIgnoreCase)) { charge = -3; mu = 2.13914e+11; }
            if (name.Equals("[Co(NH3)6]", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 2.20402e+11; }
            if (name.Equals("Cr", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.44916e+11; }
            if (name.Equals("CrO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -2; mu = 2.75773e+11; }
            if (name.Equals("Cs", StringComparison.InvariantCultureIgnoreCase)) { charge = +1; mu = 5.00934e+11; }
            if (name.Equals("Cu", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 1.739e+11; }
            if (name.Equals("D", StringComparison.InvariantCultureIgnoreCase)) { charge = +1; mu = 1.62155e+12; }
            if (name.Equals("Dy", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.41888e+11; }
            if (name.Equals("Er", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.42537e+11; }
            if (name.Equals("Eu", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.46647e+11; }
            if (name.Equals("F", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 3.59479e+11; }
            if (name.Equals("FeII", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 1.75197e+11; }
            if (name.Equals("FeIII", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.47079e+11; }
            if (name.Equals("[Fe(CN)6]III", StringComparison.InvariantCultureIgnoreCase)) { charge = -3; mu = 2.1824e+11; }
            if (name.Equals("[Fe(CN)6]IIII", StringComparison.InvariantCultureIgnoreCase)) { charge = -4; mu = 1.79091e+11; }
            if (name.Equals("Gd", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.45565e+11; }
            if (name.Equals("H2AsO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 2.20619e+11; }
            if (name.Equals("H2PO2", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 2.98484e+11; }
            if (name.Equals("H2PO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 2.33596e+11; }
            if (name.Equals("H2SbO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 2.01152e+11; }
            if (name.Equals("H", StringComparison.InvariantCultureIgnoreCase)) { charge = +1; mu = 2.2688e+12; }
            if (name.Equals("HCO3", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 2.88751e+11; }
            if (name.Equals("HF2", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 4.86659e+11; }
            if (name.Equals("Hg", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 2.22565e+11; }
            if (name.Equals("Ho", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.43402e+11; }
            if (name.Equals("HPO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -2; mu = 1.8493e+11; }
            if (name.Equals("HS", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 4.21771e+11; }
            if (name.Equals("HSO3", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 3.7635e+11; }
            if (name.Equals("HSO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 3.37417e+11; }
            if (name.Equals("I", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 4.98339e+11; }
            if (name.Equals("IO3", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 2.62796e+11; }
            if (name.Equals("IO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 3.53639e+11; }
            if (name.Equals("K", StringComparison.InvariantCultureIgnoreCase)) { charge = +1; mu = 4.76796e+11; }
            if (name.Equals("La", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.50756e+11; }
            if (name.Equals("Li", StringComparison.InvariantCultureIgnoreCase)) { charge = +1; mu = 2.50857e+11; }
            if (name.Equals("Mg", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 1.71953e+11; }
            if (name.Equals("Mn", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 1.73575e+11; }
            if (name.Equals("MnO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 3.97763e+11; }
            if (name.Equals("MoO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -2; mu = 2.41707e+11; }
            if (name.Equals("N2H5", StringComparison.InvariantCultureIgnoreCase)) { charge = +1; mu = 3.82838e+11; }
            if (name.Equals("N3", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 4.47726e+11; }
            if (name.Equals("Na", StringComparison.InvariantCultureIgnoreCase)) { charge = +1; mu = 3.24958e+11; }
            if (name.Equals("N(CN)2", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 3.53639e+11; }
            if (name.Equals("Nd", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.50107e+11; }
            if (name.Equals("NH2SO3", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 3.13408e+11; }
            if (name.Equals("NH4", StringComparison.InvariantCultureIgnoreCase)) { charge = +1; mu = 4.76926e+11; }
            if (name.Equals("Ni", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 1.60922e+11; }
            if (name.Equals("NO2", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 4.65895e+11; }
            if (name.Equals("NO3", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 4.63429e+11; }
            if (name.Equals("OCN", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 4.19176e+11; }
            if (name.Equals("OD", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 7.72166e+11; }
            if (name.Equals("OH", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 1.28478e+12; }
            if (name.Equals("Pb", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 2.30352e+11; }
            if (name.Equals("PF6", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 3.69212e+11; }
            if (name.Equals("PO3F", StringComparison.InvariantCultureIgnoreCase)) { charge = -2; mu = 2.0537e+11; }
            if (name.Equals("PO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -3; mu = 2.0072e+11; }
            if (name.Equals("Pr", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.50324e+11; }
            if (name.Equals("Ra", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 2.16725e+11; }
            if (name.Equals("Rb", StringComparison.InvariantCultureIgnoreCase)) { charge = +1; mu = 5.04828e+11; }
            if (name.Equals("ReO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 3.56234e+11; }
            if (name.Equals("Sb(OH)6", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 2.06992e+11; }
            if (name.Equals("Sc", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.39942e+11; }
            if (name.Equals("SCN", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 4.2826e+11; }
            if (name.Equals("SeCN", StringComparison.InvariantCultureIgnoreCase)) { charge = -1; mu = 4.19825e+11; }
            if (name.Equals("SeO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -2; mu = 2.45601e+11; }
            if (name.Equals("Sm", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.48161e+11; }
            if (name.Equals("SO3", StringComparison.InvariantCultureIgnoreCase)) { charge = -2; mu = 2.33596e+11; }
            if (name.Equals("SO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -2; mu = 2.59551e+11; }
            if (name.Equals("Sr", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 1.92717e+11; }
            if (name.Equals("Tl", StringComparison.InvariantCultureIgnoreCase)) { charge = +1; mu = 4.84712e+11; }
            if (name.Equals("Tm", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.41456e+11; }
            if (name.Equals("UO2", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 1.03821e+11; }
            if (name.Equals("WO4", StringComparison.InvariantCultureIgnoreCase)) { charge = -2; mu = 2.23863e+11; }
            if (name.Equals("Yb", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.41888e+11; }
            if (name.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) { charge = +3; mu = 1.34102e+11; }
            if (name.Equals("Zn", StringComparison.InvariantCultureIgnoreCase)) { charge = +2; mu = 1.71304e+11; }

            if (charge == 0 || mu == 0)
                throw new ArgumentException("ion not found");
            else
                return new Ion(name, 0, 0, charge, mu, attemptLookup: false);
        }
    }
}
