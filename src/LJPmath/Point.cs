using System;
using System.Linq;

namespace LJPmath
{
    class Point : IComparable<Point>
    {
        public readonly double[] X;
        public readonly double[] F;
        public readonly double M;

        public Point(double[] x, IEquationSystem es)
        {
            X = x;
            F = new double[es.GetEquationCount];
            es.Equations(x, F);
            M = F.Select(f => Math.Abs(f)).Max();
        }

        public int CompareTo(Point p)
        {
            if (M < p.M)
                return -1;
            else if (M > p.M)
                return 1;
            else
                return 0;
        }
    }
}
