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
        public readonly double temperatureC;
        public double temperatureK { get { return temperatureC + Constants.zeroCinK; } }

        readonly Stopwatch stopwatch = new Stopwatch();

        public LjpResult(List<Ion> ionList, double temperatureC)
        {
            ionListOriginal.Clear();
            foreach (Ion ion in ionList)
                ionListOriginal.Add(new Ion(ion));

            this.temperatureC = temperatureC;

            stopwatch.Restart();
        }

        public void Finished(List<Ion> ionList, double ljpVolts)
        {
            stopwatch.Stop();
            benchmark_s = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;

            V = ljpVolts;

            ionListSolved.Clear();
            foreach (Ion ion in ionList)
                ionListSolved.Add(new Ion(ion));
            var solvedIonSet = new IonSet(ionListSolved);

            double chlorideConcC0 = 0;
            double chlorideConcCL = 0;
            foreach (Ion ion in solvedIonSet.ions)
            {
                if (ion.name == "Cl")
                {
                    chlorideConcC0 += ion.c0;
                    chlorideConcCL += ion.cL;
                }
            }
            bool isChlorideOnBothSides = (chlorideConcC0 > 0 && chlorideConcCL > 0);

            // build report
            StringBuilder txt = new StringBuilder();
            txt.AppendLine("Concentrations were slightly adjusted to achieve electroneutrality:");
            txt.AppendLine();
            txt.AppendLine(solvedIonSet.GetTableString());
            txt.AppendLine();
            txt.AppendLine($"Equations were solved in {benchmark}");
            txt.AppendLine($"LJP at {temperatureC} C ({temperatureK} K) = {mV} mV");

            if (isChlorideOnBothSides)
            {

                double chlorideLjp_C0_Mv = -1000 * (Constants.R * temperatureK / Constants.F) * Math.Log(chlorideConcC0);
                double chlorideLjp_CL_Mv = -1000 * (Constants.R * temperatureK / Constants.F) * Math.Log(chlorideConcCL);
                txt.AppendLine();
                txt.AppendLine($"Chloride LJPs may be useful if using Ag/AgCl electrodes:");
                txt.AppendLine($"LJP[Cl] c0 = {chlorideLjp_C0_Mv} mV");
                txt.AppendLine($"LJP[Cl] cL = {chlorideLjp_CL_Mv} mV");
                txt.AppendLine($"difference = {chlorideLjp_CL_Mv - chlorideLjp_C0_Mv} mV");
            }

            report = txt.ToString().TrimEnd();
        }
    }
}
