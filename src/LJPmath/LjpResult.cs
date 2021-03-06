﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace LJPmath
{
    public class LjpResult
    {
        public double V;
        public double mV { get { return V * 1000; } }
        public double benchmark_s;
        public double benchmark_ms { get { return benchmark_s * 1000; } }
        public string benchmark { get { return string.Format("{0:0.00} ms", benchmark_ms); } }
        public bool isValid
        {
            get
            {
                if (double.IsNaN(V))
                    return false;
                else if (double.IsInfinity(V))
                    return false;
                else
                    return true;
            }
        }

        public string report = "report not generated";
        public readonly List<Ion> ionListOriginal = new List<Ion>();
        public readonly List<Ion> ionListSolved = new List<Ion>();
        public readonly double temperatureC;
        public double temperatureK => temperatureC + Constants.zeroCinK;

        readonly Stopwatch stopwatch = new Stopwatch();

        public LjpResult(Ion[] ionList, double temperatureC)
        {
            ionListOriginal.Clear();
            foreach (Ion ion in ionList)
                ionListOriginal.Add(new Ion(ion));

            this.temperatureC = temperatureC;

            stopwatch.Restart();
        }

        public override string ToString()
        {
            return $"LJP result: {mV} mV";
        }

        public string GetShortDescription()
        {
            return $"{mV} mV ({benchmark_s} sec)";
        }

        public string ToJson(bool pretty = true)
        {
            using (var stream = new MemoryStream())
            {
                var writerOptions = new JsonWriterOptions { Indented = pretty };
                using (var writer = new Utf8JsonWriter(stream, writerOptions))
                {
                    writer.WriteStartObject();
                    writer.WriteNumber("temperatureC", temperatureC);
                    writer.WriteNumber("mV", mV);
                    writer.WriteNumber("calculation seconds", benchmark_s);

                    writer.WriteStartArray("original ion list");
                    foreach (var ion in ionListOriginal)
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

                    writer.WriteStartArray("solved ion list");
                    foreach (var ion in ionListSolved)
                    {
                        writer.WriteStartObject();
                        writer.WriteString("name", ion.name);
                        writer.WriteNumber("charge", ion.charge);
                        writer.WriteNumber("conductivity", ion.conductivity);
                        writer.WriteNumber("phi", ion.phi);
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

        public string GetTimeoutWarning()
        {
            StringBuilder txt = new StringBuilder();
            txt.AppendLine($"WARNING: First ion's M ({Math.Round(FirstIonM, 3)}) did not reach 1.");
            txt.AppendLine("  This indicates the solver may have timed out while balancing phis.");
            txt.AppendLine("  LJP may be accurate, but a fully solved set of equations is preferred.");
            txt.AppendLine("  Reduce the number of ions in the list to achieve this.");
            txt.AppendLine("  Ions with small concentrations can be removed without affecting LJP much.");
            return txt.ToString();
        }

        public double FirstIonM = double.NaN;
        public void Finished(Ion[] ions, double ljpVolts, double firstIonM)
        {
            stopwatch.Stop();
            benchmark_s = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;

            V = ljpVolts;
            FirstIonM = firstIonM;

            ionListSolved.Clear();
            foreach (Ion ion in ions)
                ionListSolved.Add(new Ion(ion));
            var solvedIonSet = new IonSet(ionListSolved);

            // build report
            StringBuilder txt = new StringBuilder();
            txt.AppendLine("Concentrations were slightly adjusted to achieve electroneutrality:");
            txt.AppendLine();
            txt.AppendLine(solvedIonSet.GetTableString());
            txt.AppendLine();
            txt.AppendLine($"Equations were solved in {benchmark}");
            if (FirstIonM > 1)
                txt.AppendLine(GetTimeoutWarning());
            txt.AppendLine($"LJP at {temperatureC} C ({temperatureK} K) = {mV} mV");

            report = txt.ToString().TrimEnd();
        }
    }
}
