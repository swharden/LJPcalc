namespace LJPcalc.Core.KnownIonSets;

public class AxoBitsArticle : IKnownIonSet
{
    public string Name => "AxoBits Article";

    public string Details => "This ion set came from a JPCalc screenshot in an AxoBits article about LJP corrections (Issue 39, page 8, https://medicalsciences.med.unsw.edu.au/sites/default/files/soms/page/ElectroPhysSW/LJP_article_%20AxoBits%2039.pdf).";

    public double Temperature_C => 20;

    public double Ljp_mV => +15.6;

    public double Accuracy_mV => .5;

    public Ion[] Ions => new Ion[]
    {
        new Ion("K", 145, 2.8),
        new Ion("Na", 13, 145),
        new Ion("Cl", 10, 148.8),
        new Ion("Gluconate", 145, 0),
        new Ion("Mg", 1, 2),
        new Ion("Ca", 0, 1),
        new Ion("HEPES", 5, 5),
    };
}
