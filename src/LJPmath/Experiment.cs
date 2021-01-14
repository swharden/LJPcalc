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
        public bool AutoSortIons = true;
        public double TimeoutSeconds = 10;

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

        public static Experiment FromJson(string json)
        {
            using (JsonDocument document = JsonDocument.Parse(json))
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
