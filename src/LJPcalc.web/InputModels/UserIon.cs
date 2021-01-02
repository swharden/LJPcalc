using LJPcalc.web.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJPcalc.web.InputModels
{
    public class UserIon
    {
        public readonly UserIonName Name = new UserIonName();
        public readonly UserIonCharge Charge = new UserIonCharge();
        public readonly UserIonMobility Mobility = new UserIonMobility();
        public readonly UserIonConcentration C0 = new UserIonConcentration();
        public readonly UserIonConcentration CL = new UserIonConcentration();
        public bool IsValid => Name.IsValid && Charge.IsValid && Mobility.IsValid && C0.IsValid && CL.IsValid;

        public UserIon() { }

        public UserIon(string name, int charge, double mobility, double c0, double cL)
        {
            Name.Input = name;
            Charge.Input = charge.ToString();
            Mobility.Input = (mobility / 1e11).ToString();
            C0.Input = c0.ToString();
            CL.Input = cL.ToString();
        }

        public static UserIon FromIon(LJPmath.Ion ion) =>
            new UserIon(ion.name, ion.charge, ion.mu, ion.c0, ion.cL);

        public LJPmath.Ion ToIon()
        {
            string name = Name.Input;
            int charge = Charge.Charge;
            double cond = LJPmath.Ion.Conductivity(Mobility.Mobility, Charge.Charge);
            double c0 = C0.Concentration;
            double cL = CL.Concentration;
            return new LJPmath.Ion(name, charge, cond, c0, cL);
        }

        public UserIon Copy() =>
            new UserIon(Name.Input, Charge.Charge, Mobility.Mobility, C0.Concentration, CL.Concentration);
    }
}
