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
        }
    }
}
