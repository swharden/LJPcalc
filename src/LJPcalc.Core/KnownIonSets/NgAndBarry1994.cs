namespace LJPcalc.Core.KnownIonSets
{
    internal class NgAndBarry1994 : IKnownIonSet
    {
        public string Name => "Ng and Barry (1994)";

        public string Details => "This ion set is from the bottom row of Table 2 (https://doi.org/10.1016/0165-0270(94)00087-W)";

        public double Temperature_C => 25;

        public double Ljp_mV => -8.2;

        public double Accuracy_mV => .5;

        public Ion[] Ions => new Ion[]
        {
            new Ion("Ca", 50, 0),
            new Ion("Cl", 200, 100),
            new Ion("Mg", 50, 0),
            new Ion("Li", 0, 100)
        };
    }
}
