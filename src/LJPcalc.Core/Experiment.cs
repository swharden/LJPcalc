using System.Text;
using System.Text.Json;

namespace LJPcalc.Core;

public class Experiment
{
    public readonly Ion[] Ions;
    public readonly double TemperatureC;

    // configuration going in
    public bool AutoSortIons = true;
    public double TimeoutSeconds = 10;
    public double TimeoutMilliSeconds => TimeoutSeconds * 1e3;

    // results coming out
    public double CalculationSeconds = double.NaN;
    public double LjpMillivolts = double.NaN;
    public Ion[] SolvedIons;

    public Experiment(Ion[] ions, double temperatureC)
    {
        Ions = ions;
        TemperatureC = temperatureC;
    }

    public LjpResult Execute() =>
        Calculate.Ljp(Ions, TemperatureC, AutoSortIons, TimeoutMilliSeconds, throwIfTimeout: false);

    public string GetShortDescription()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"{TemperatureC}C");
        foreach (var ion in Ions)
            sb.Append($", {ion.Name.Split(' ')[0]} {ion.C0:0.000}:{ion.CL:0.000}");
        return sb.ToString();
    }

    public string GetReport()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"LJPcalc {typeof(Ion).Assembly.GetName().Version}");
        sb.AppendLine($"Temperature: {TemperatureC} C");
        sb.AppendLine($"LJP = {LjpMillivolts} mV");
        sb.AppendLine($"Calculation time: {CalculationSeconds} sec");
        sb.AppendLine($"");
        sb.AppendLine($"Given Ions:");
        foreach (var ion in Ions)
            sb.AppendLine($"  {ion}");
        sb.AppendLine($"");
        sb.AppendLine($"Solution Ions (slightly adjusted to achieve electronegativity):");
        foreach (var ion in Ions)
            sb.AppendLine($"  {ion.Name} phi={ion.Phi}, C0={ion.C0}, CL={ion.CL}");
        return sb.ToString();
    }

    /// <summary>
    /// The server calls this to create an experiment from scratch
    /// </summary>
    public static Experiment FromJson(string experimentJson)
    {
        using (JsonDocument document = JsonDocument.Parse(experimentJson))
        {
            double temperatureC = document.RootElement.GetProperty("temperatureC").GetDouble();
            List<Ion> ions = new List<Ion>();
            foreach (JsonElement ionElement in document.RootElement.GetProperty("ions").EnumerateArray())
            {
                string name = ionElement.GetProperty("name").GetString();
                int charge = ionElement.GetProperty("charge").GetInt32();
                double conductivity = ionElement.GetProperty("conductivity").GetDouble();
                double c0 = ionElement.GetProperty("c0").GetDouble();
                double cL = ionElement.GetProperty("cL").GetDouble();
                ions.Add(new Ion(name, charge, conductivity, c0, cL));
            }
            return new Experiment(ions.ToArray(), temperatureC);
        };
    }

    /// <summary>
    /// Add experiment results to this experiment
    /// </summary>
    public void AddResultsJson(string resultsJson)
    {
        using (JsonDocument document = JsonDocument.Parse(resultsJson))
        {
            LjpMillivolts = document.RootElement.GetProperty("mV").GetDouble();
            CalculationSeconds = document.RootElement.GetProperty("calculation seconds").GetDouble();
            List<Ion> ions = new List<Ion>();
            foreach (JsonElement ionElement in document.RootElement.GetProperty("solved ion list").EnumerateArray())
            {
                string name = ionElement.GetProperty("name").GetString();
                int charge = ionElement.GetProperty("charge").GetInt32();
                double conductivity = ionElement.GetProperty("conductivity").GetDouble();
                double c0 = ionElement.GetProperty("c0").GetDouble();
                double cL = ionElement.GetProperty("cL").GetDouble();
                ions.Add(new Ion(name, charge, conductivity, c0, cL));
            }
            SolvedIons = ions.ToArray();
        };
    }

    public string ToJson()
    {
        using (var stream = new MemoryStream())
        {
            using (var writer = new Utf8JsonWriter(stream))
            {
                writer.WriteStartObject();
                writer.WriteNumber("temperatureC", TemperatureC);
                writer.WriteStartArray("ions");
                foreach (var ion in Ions)
                {
                    writer.WriteStartObject();
                    writer.WriteString("name", ion.Name);
                    writer.WriteNumber("charge", ion.Charge);
                    writer.WriteNumber("conductivity", ion.Conductivity);
                    writer.WriteNumber("c0", ion.C0);
                    writer.WriteNumber("cL", ion.CL);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
