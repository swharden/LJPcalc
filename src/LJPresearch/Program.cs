using LJPmath;
using System;
using System.Collections.Generic;

namespace LJPresearch
{
    class Program
    {
        static void Main(string[] args)
        {
            Figure_EffectOfTemperature_DifferentMobilities();
        }

        public static void Figure_EffectOfTemperature_Simple()
        {
            var ionTable = new IonTable();

            double[] temps = ScottPlot.DataGen.Consecutive(50);
            double[] ljps = new double[temps.Length];

            for (int i = 0; i < temps.Length; i++)
            {
                Console.WriteLine($"Calculating for {temps[i]}C...");
                var ionSet = new List<Ion> { new Ion("Zn", 9, 0.0284), new Ion("K", 0, 3), new Ion("Cl", 18, 3.0568) };
                ljps[i] = Calculate.Ljp(ionTable.Lookup(ionSet).ToArray(), temps[i]).mV;
            }

            var plt = new ScottPlot.Plot(800, 600);
            plt.PlotScatter(temps, ljps);
            plt.Title("JLJP Screenshot Ion Set");
            plt.YLabel("LJP (mV)");
            plt.XLabel("Temperature (C)");

            string filePath = System.IO.Path.GetFullPath("Figure_EffectOfTemperatureSimple.png");
            plt.SaveFig(filePath);
            Console.WriteLine($"Saved: {filePath}");
        }

        public static void Figure_EffectOfTemperature_DifferentMobilities()
        {
            double[] temps = ScottPlot.DataGen.Consecutive(50);
            double[] ljps25C = new double[temps.Length];
            double[] ljps18C = new double[temps.Length];
            double[] ljps0C = new double[temps.Length];

            for (int i = 0; i < temps.Length; i++)
            {
                Console.WriteLine($"Calculating for {temps[i]}C...");

                var ionSet0C = new List<Ion> {
                    new Ion("Zn", 2, 28, 9, 0.0284),
                    new Ion("K", 1, 40.3, 0, 3),
                    new Ion("Cl", -1, 41.4, 18, 3.0568)
                };
                ljps0C[i] = Calculate.Ljp(ionSet0C, temps[i]).mV;

                var ionSet18C = new List<Ion> {
                    new Ion("Zn", 2, 45.0, 9, 0.0284),
                    new Ion("K", 1, 64.6, 0, 3),
                    new Ion("Cl", -1, 65.5, 18, 3.0568)
                };
                ljps18C[i] = Calculate.Ljp(ionSet18C, temps[i]).mV;

                var ionSet25C = new List<Ion> {
                    new Ion("Zn", 2, 52.8, 9, 0.0284),
                    new Ion("K", 1, 73.5, 0, 3),
                    new Ion("Cl", -1, 76.31, 18, 3.0568)
                };
                ljps25C[i] = Calculate.Ljp(ionSet25C, temps[i]).mV;

            }

            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotScatter(temps, ljps25C, label: "25C mobilities");
            plt.PlotScatter(temps, ljps18C, label: "18C mobilities");
            plt.PlotScatter(temps, ljps0C, label: "0C mobilities");
            plt.Legend(location: ScottPlot.legendLocation.lowerLeft);
            plt.Title("JLJP Screenshot Ion Set");
            plt.YLabel("Junction Potential (mV)");
            plt.XLabel("Temperature (C)");

            string filePath = System.IO.Path.GetFullPath("Figure_EffectOfTemperature_DifferentMobilities.png");
            plt.SaveFig(filePath);
            Console.WriteLine($"Saved: {filePath}");
        }

        public static void Figure_EffectOfTemperature_SeveralSets()
        {
            /* this study calculates one of several known ionSets at every temperature between 0C and 50C */

            var ionTable = new IonTable();

            double[] temps = ScottPlot.DataGen.Consecutive(50);
            double[] jljpResults = new double[temps.Length];
            double[] ngAndBarryResults = new double[temps.Length];
            double[] HarperResults = new double[temps.Length];
            double[] jpWinResults = new double[temps.Length];

            List<Ion> ionSet;
            for (int i = 0; i < temps.Length; i++)
            {
                Console.WriteLine($"Calculating for {temps[i]}C...");

                // see LjpCalculationTests.cs for ion set citations

                // create a new ion set for each iteration to prevent modifications due to solving from carrying to the next solve

                ionSet = new List<Ion> { new Ion("Zn", 9, 0.0284), new Ion("K", 0, 3), new Ion("Cl", 18, 3.0568) };
                jljpResults[i] = Calculate.Ljp(ionTable.Lookup(ionSet), temps[i]).mV;

                ionSet = new List<Ion> { new Ion("Ca", 2, 2), new Ion("K", 100, 0), new Ion("Li", 0, 100), new Ion("Cl", 104, 104) };
                ngAndBarryResults[i] = Calculate.Ljp(ionTable.Lookup(ionSet), temps[i]).mV;

                ionSet = new List<Ion> { new Ion("Ca", .29, .00545), new Ion("Cl", .29 * 2, .00545 * 2) };
                HarperResults[i] = Calculate.Ljp(ionTable.Lookup(ionSet), temps[i]).mV;

                ionSet = new List<Ion> { new Ion("Na", 10, 145), new Ion("Cl", 10, 145), new Ion("Cs", 135, 0), new Ion("F", 135, 0) };
                jpWinResults[i] = Calculate.Ljp(ionTable.Lookup(ionSet), temps[i]).mV;
            }

            var mplt = new ScottPlot.MultiPlot(800, 600, 2, 2);

            mplt.subplots[0].PlotScatter(temps, jljpResults);
            mplt.subplots[0].Title("JLJP screenshot");

            mplt.subplots[1].PlotScatter(temps, ngAndBarryResults);
            mplt.subplots[1].Title("Ng and Barry (1994)");

            mplt.subplots[2].PlotScatter(temps, HarperResults);
            mplt.subplots[2].Title("Harper (1985)");

            mplt.subplots[3].PlotScatter(temps, jpWinResults);
            mplt.subplots[3].Title("JPWin screenshot");

            for (int i = 0; i < 4; i++)
            {
                mplt.subplots[i].YLabel("LJP (mV)");
                mplt.subplots[i].XLabel("Temperature (C)");
            }

            string filePath = System.IO.Path.GetFullPath("Figure_EffectOfTemperature.png");
            mplt.SaveFig(filePath);
            Console.WriteLine($"Saved: {filePath}");
        }

        public static void DemoScottPlot()
        {
            double[] xs = ScottPlot.DataGen.Consecutive(101);
            double[] ys1 = ScottPlot.DataGen.Sin(xs.Length, 2);
            double[] ys2 = ScottPlot.DataGen.Cos(xs.Length, 2);

            var plt = new ScottPlot.Plot(600, 400);
            plt.PlotScatter(xs, ys1, label: "Nernst-Planck");
            plt.PlotScatter(xs, ys2, label: "Henderson");
            plt.YLabel("LJP (mV)");
            plt.XLabel("Alpha (%)");
            plt.Title("Experimental Plot Test");
            plt.AxisAuto(0, .3);
            plt.Legend();

            string filePath = System.IO.Path.GetFullPath("Figure_ScottPlotDemo.png");
            plt.SaveFig(filePath);
            Console.WriteLine($"Saved: {filePath}");
        }
    }
}
