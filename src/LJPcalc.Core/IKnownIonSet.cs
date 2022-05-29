namespace LJPcalc.Core;

public interface IKnownIonSet
{
    public string Name { get; }
    public string Details { get; }
    public double Temperature_C { get; }
    public double Ljp_mV { get; }
    public double Accuracy_mV { get; }
    public Ion[] Ions { get; }
}
