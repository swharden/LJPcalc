using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LJPmath
{
    class Solver
    {
        private readonly PhiEquations es;
        private readonly Random rand = new Random(0);

        public Solver(PhiEquations es)
        {
            this.es = es;
        }

        private readonly List<Point> list = new List<Point>();
        public void Solve(double[] x, double timeoutMilliseconds = 0)
        {
            if (es.number() == 0)
                return;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            list.Add(new Point(x, es));
            while (list[0].getM() > 1.0)
            {
                Suggest();
                list.Sort();
                while (list.Count > es.number() * 4)
                    list.RemoveAt(list.Count - 1);
                if ((timeoutMilliseconds > 0) && (stopwatch.ElapsedMilliseconds > timeoutMilliseconds))
                    throw new OperationCanceledException($"Solver timed out while calculating Phis ({timeoutMilliseconds} ms)");
            }

            for (int j = 0; j < es.number(); j++)
                x[j] = list[0].getIndex(j);
        }

        public int sn = 0; // TODO: make sn an enumeration
        private void Suggest()
        {
            double[] x = new double[es.number()];
            switch (sn)
            {
                case 0:
                    if (list.Count >= es.number() + 1)
                    {
                        double[,] Mm = new double[es.number(), es.number()];
                        for (int j = 0; j < es.number(); j++)
                            for (int k = 0; k < es.number(); k++)
                                Mm[j, k] = list[k].getF(j) - list[es.number()].getF(j);
                        double[] mF0 = new double[es.number()];
                        for (int j = 0; j < es.number(); j++)
                            mF0[j] = -list[es.number()].getF(j);
                        double[] u = Linalg.Solve(Mm, mF0);
                        double[,] Vm = new double[es.number(), es.number()];
                        for (int j = 0; j < es.number(); j++)
                            for (int k = 0; k < es.number(); k++)
                                Vm[j, k] = list[k].getIndex(j) - list[es.number()].getIndex(j);
                        double[] delta = Linalg.Product(Vm, u);
                        for (int j = 0; j < es.number(); j++)
                            x[j] = list[es.number()].getIndex(j) + delta[j];
                    }
                    break;
                case 1:
                    for (int j = 0; j < es.number(); j++)
                        x[j] = list[0].getIndex(j) * (rand.NextDouble() - 0.5) * 4.0;
                    break;
                case 2:
                    for (int j = 0; j < es.number(); j++)
                        x[j] = (rand.NextDouble() - 0.5) * 4.0;
                    break;
                case 3:
                    for (int j = 0; j < es.number(); j++)
                    {
                        double min, max;
                        min = max = list[0].getIndex(j);
                        for (int k = 0; k < list.Count; k++)
                        {
                            double v = list[k].getIndex(j);
                            if (v < min)
                                min = v;
                            if (v > max)
                                max = v;
                        }
                        if (min == max)
                        {
                            if (min > 0.0)
                            {
                                min *= 0.8;
                                max *= 1.2;
                            }
                            else if (min < 0.0)
                            {
                                min *= 1.2;
                                max *= 0.8;
                            }
                            else
                            {
                                min = -1.0;
                                max = 1.0;
                            }
                        }
                        x[j] = (min + max) / 2.0 + (max - min) * (rand.NextDouble() - 0.5) * 3.0;
                    }
                    break;
            }

            list.Add(new Point(x, es));
            sn++;
            if (sn > 3)
                sn = 0;
        }
    }
}
