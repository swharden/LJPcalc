using System.Text;

namespace LJPcalc.Core;

public class IonSet
{
    public static string ToText(Ion[] ions, double temperatureC)
    {
        StringBuilder sb = new();
        foreach (Ion ion in ions)
            sb.AppendLine($"ion: {ion.Name}, {ion.ChargeWithSign}, {ion.Conductivity}, {ion.C0}, {ion.CL}");
        sb.AppendLine($"temperatureC: {temperatureC}");
        return sb.ToString();
    }

    public static (Ion[] ions, double temperatureC) FromText(string text)
    {
        List<Ion> ions = new();
        double temperatureC = double.NaN;

        foreach (string rawLine in text.Split("\n"))
        {
            string line = rawLine.Trim();

            if (line.StartsWith("ion:"))
                ions.Add(GetIonFromLine(line));

            if (line.StartsWith("temperatureC:"))
                temperatureC = GetTemperatureFromLine(line);
        }

        return (ions.ToArray(), temperatureC);
    }

    private static Ion GetIonFromLine(string line)
    {
        string[] parts = line.Split(":", 2)[1].Split(",");
        string name = parts[0].Trim();
        int charge = int.Parse(parts[1]);
        double conductivity = double.Parse(parts[2]);
        double c0 = double.Parse(parts[3]);
        double cL = double.Parse(parts[4]);
        return new Ion(name, charge, conductivity, c0, cL);
    }

    private static double GetTemperatureFromLine(string line)
    {
        return double.Parse(line.Split(":", 2)[1]);
    }
}
