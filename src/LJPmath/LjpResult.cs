using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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

        readonly Stopwatch stopwatch = new Stopwatch();

        public LjpResult(List<Ion> ionList)
        {
            ionListOriginal.Clear();
            foreach (Ion ion in ionList)
                ionListOriginal.Add(new Ion(ion));

            stopwatch.Restart();
        }

        public void Finished(List<Ion> ionList, double ljp_V)
        {
            stopwatch.Stop();
            benchmark_s = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;

            this.V = ljp_V;

            ionListSolved.Clear();
            foreach (Ion ion in ionList)
                ionListSolved.Add(new Ion(ion));

            var solvedIonSet = new IonSet(ionListSolved);

            StringBuilder txt = new StringBuilder();
            txt.AppendLine("Values for cL were adjusted to achieve electro-neutrality:");
            txt.AppendLine();
            txt.AppendLine(solvedIonSet.GetTableString());
            txt.AppendLine();
            txt.AppendLine($"Equations were solved in {benchmark}");
            txt.AppendLine($"LJP = {mV} mV");

            report = txt.ToString().TrimEnd();
        }
    }
}
