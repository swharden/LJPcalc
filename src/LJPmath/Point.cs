using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LJPmath
{
    public class Point : IComparable<Point>
    {
        public double[] x;
        public double[] f;
        public double m;

        public Point(double[] x, IEquationSystem es)
        {

            this.x = x;
            f = new double[es.number()];
            try
            {
                es.equations(x, f);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                f[0] = 10000.0;
            }

            m = 0.0;
            for (int j = 0; j < es.number(); j++)
            {
                double mm = Math.Abs(f[j]);
                if (mm > m)
                    m = mm;
            }
        }

        public int CompareTo(Point p)
        {
            // TODO: one liner
            if (m < p.m)
                return -1;
            else if (m > p.m)
                return 1;
            else
                return 0;
        }

        // TODO: remove getters and setters
        public double getM()
        {
            return m;
        }

        public double getIndex(int n)
        {
            return x[n];
        }

        public double getF(int n)
        {
            return f[n];
        }
    }
}
