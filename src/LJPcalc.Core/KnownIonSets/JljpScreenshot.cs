namespace LJPcalc.Core.KnownIonSets;

public class JljpScreenshot : IKnownIonSet
{
    public string Name => "JLJP screenshot";

    public string Details => "This ion set is from the original JLJP screenshot on SourceForge (https://a.fsdn.com/con/app/proj/jljp/screenshots/GUI.png/max/max/1.jpg).";

    public double Temperature_C => 25;

    public double Ljp_mV => -20.79558643;

    public double Accuracy_mV => .5;

    public Ion[] Ions => new Ion[]
    {
        new Ion("Zn", 9, 2.84E-2),
        new Ion("K", 0, 3),
        new Ion("Cl", 18, 3.062)
    };
}
