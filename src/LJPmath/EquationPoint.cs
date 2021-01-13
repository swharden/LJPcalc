using System;
using System.Linq;

namespace LJPmath
{
    class EquationPoint : IComparable<EquationPoint>
    {
        public readonly double[] X;
        public readonly double[] F;
        public readonly double M;

        public EquationPoint(double[] x, IEquationSystem equationSystem)
        {
            X = x;
            F = new double[equationSystem.EquationCount];
            equationSystem.Calculate(x, F);
            M = F.Select(f => Math.Abs(f)).Max();
        }

        public int CompareTo(EquationPoint p)
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
