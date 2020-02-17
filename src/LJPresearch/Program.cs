using System;

namespace LJPresearch
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoScottPlot();
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
