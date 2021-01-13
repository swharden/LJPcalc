using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LJPmath
{
    class EquationSolver
    {
        private readonly IEquationSystem Equations;
        private readonly List<Point> Points = new List<Point>();
        private readonly Random rand = new Random(0);
        private readonly int EquationCount;

        private int Iterations;
        private readonly List<Action> AddPointMethods;
        private void AddSuggestedPoint() => AddPointMethods[Iterations++ % AddPointMethods.Count].Invoke();

        public EquationSolver(IEquationSystem equations)
        {
            Equations = equations;
            EquationCount = equations.GetEquationCount;

            AddPointMethods = new List<Action>()
            {
                AddSuggestedPoint_ShiftedBySolutionDelta,
                AddSuggestedPoint_NearFirstPoint,
                AddSuggestedPoint_TotallyRandom,
                AddSuggestedPoint_ConsideringMinMax,
            };
        }

        /// <summary>
        /// Solve the equations and reutrn the M for the first point
        /// </summary>
        public double Solve(double[] x, double timeoutMilliseconds, bool throwIfTimeout)
        {
            if (EquationCount == 0)
                return double.NaN;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Points.Add(new Point(x, Equations));
            while (Points[0].M > 1.0)
            {
                AddSuggestedPoint();
                Points.Sort();
                RemovePointsAfter(EquationCount * 4);

                Debug.WriteLine(Points[0].M);
                if (stopwatch.ElapsedMilliseconds > timeoutMilliseconds)
                {
                    if (throwIfTimeout)
                        throw new OperationCanceledException($"Solver timed out while calculating Phis ({timeoutMilliseconds} ms)");
                    break;
                }
            }

            for (int j = 0; j < EquationCount; j++)
                x[j] = Points[0].X[j];

            return Points[0].M;
        }

        private void RemovePointsAfter(int maxCount)
        {
            while (Points.Count > maxCount)
                Points.RemoveAt(Points.Count - 1);
        }

        /// <summary>
        /// Add a point with Xs shifted by the delta of the solved matrix
        /// </summary>
        private void AddSuggestedPoint_ShiftedBySolutionDelta()
        {
            if (Points.Count < EquationCount + 1)
            {
                AddSuggestedPoint_NearFirstPoint();
                return;
            }

            double[] suggestedXs = new double[EquationCount];

            double[,] Mm = new double[EquationCount, EquationCount];
            for (int j = 0; j < EquationCount; j++)
                for (int k = 0; k < EquationCount; k++)
                    Mm[j, k] = Points[k].F[j] - Points[EquationCount].F[j];

            double[] mF0 = new double[EquationCount];
            for (int j = 0; j < EquationCount; j++)
                mF0[j] = -Points[EquationCount].F[j];

            double[,] Vm = new double[EquationCount, EquationCount];
            for (int j = 0; j < EquationCount; j++)
                for (int k = 0; k < EquationCount; k++)
                    Vm[j, k] = Points[k].X[j] - Points[EquationCount].X[j];

            double[] u = Linalg.Solve(Mm, mF0);
            double[] delta = Linalg.Product(Vm, u);

            for (int j = 0; j < EquationCount; j++)
                suggestedXs[j] = Points[EquationCount].X[j] + delta[j];

            Points.Add(new Point(suggestedXs, Equations));
        }

        /// <summary>
        /// Add a point with Xs randomly offset from the Xs of the first point
        /// </summary>
        private void AddSuggestedPoint_NearFirstPoint()
        {
            const double randomness = 4; // TODO: could this value be optimized?

            double[] suggestedXs = Points[0].X.Select(x => x * (rand.NextDouble() - 0.5) * randomness)
                                              .ToArray();

            Points.Add(new Point(suggestedXs, Equations));
        }

        /// <summary>
        /// Add a point with totally random Xs
        /// </summary>
        private void AddSuggestedPoint_TotallyRandom()
        {
            const double randomness = 4; // TODO: could this value be optimized?

            double[] suggestedXs = Enumerable.Range(0, EquationCount)
                                             .Select(x => (rand.NextDouble() - 0.5) * randomness)
                                             .ToArray();

            Points.Add(new Point(suggestedXs, Equations));
        }

        /// <summary>
        /// Add a point with Xs randomized centered and scaled to the min/max of the existing Xs
        /// </summary>
        private void AddSuggestedPoint_ConsideringMinMax()
        {
            const double randomness = 3; // TODO: could this value be optimized?

            double[] suggestedXs = new double[EquationCount];

            for (int equationIndex = 0; equationIndex < EquationCount; equationIndex++)
            {
                var equationXs = Points.Select(x => x.X[equationIndex]);
                double xMin = equationXs.Min();
                double xMax = equationXs.Max();

                if (xMin == xMax)
                {
                    if (xMin > 0)
                        (xMin, xMax) = (xMin * .8, xMin * 1.2);
                    else if (xMin > 0)
                        (xMin, xMax) = (xMin * 1.2, xMin * 0.8);
                    else
                        (xMin, xMax) = (-1, 1);
                }

                double mean = (xMin + xMax) / 2.0;
                double span = xMax - xMin;
                double randomOffset = span * randomness * (rand.NextDouble() - 0.5);
                suggestedXs[equationIndex] = mean + randomOffset;
            }

            Points.Add(new Point(suggestedXs, Equations));
        }
    }
}
