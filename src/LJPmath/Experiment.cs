using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace LJPmath
{
    public class Experiment
    {
        public readonly Ion[] Ions;
        public readonly double TemperatureC;

        // configuration going in
        public bool AutoSortIons = true;
        public double TimeoutSeconds = 10;

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
            Calculate.Ljp(Ions, TemperatureC, AutoSortIons, TimeoutSeconds, throwIfTimeout: false);

        public string GetShortDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{TemperatureC}C");
            foreach (var ion in Ions)
                sb.Append($", {ion.name.Split(' ')[0]} {ion.c0:0.000}:{ion.cL:0.000}");
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
                sb.AppendLine($"  {ion.name} phi={ion.phi}, C0={ion.c0}, CL={ion.cL}");
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
                        writer.WriteString("name", ion.name);
                        writer.WriteNumber("charge", ion.charge);
                        writer.WriteNumber("conductivity", ion.conductivity);
                        writer.WriteNumber("c0", ion.c0);
                        writer.WriteNumber("cL", ion.cL);
                        writer.WriteEndObject();
                    }
                    writer.WriteEndArray();
                    writer.WriteEndObject();
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
