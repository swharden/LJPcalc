using System;
using System.Linq;

namespace LJPmath.Solver
{
    class EquationPoint : IComparable<EquationPoint>
    {
        public readonly double[] X;
        public readonly double[] F;
        public readonly double FMax;

        public EquationPoint(double[] x, IEquationSystem equationSystem)
        {
            X = x;
            F = new double[equationSystem.EquationCount];
            equationSystem.Calculate(x, F);
            FMax = F.Select(f => Math.Abs(f)).Max();
        }

        public int CompareTo(EquationPoint p)
        {
            if (FMax < p.FMax)
                return -1;
            else if (FMax > p.FMax)
                return 1;
            else
                return 0;
        }
    }
}
