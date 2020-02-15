using System;
using System.Diagnostics;

namespace LJPmath
{
    // Linear algebra tools by Doriano Brogioli translated from JAVA to C# by Scott Harden
    // Original source code: https://github.com/swharden/JLJP
    public class Linalg
    {

        public static double[,] RandomArray2d(int arrayLength, int arrayHeight, Random rand)
        {
            double[,] M = new double[arrayLength, arrayHeight];
            for (int n = 0; n < arrayLength; n++)
                for (int m = 0; m < arrayHeight; m++)
                    M[n, m] = rand.NextDouble() * 2 - 1;
            return M;
        }

        public static double[] RandomArray1d(int arrayLength, Random rand)
        {
            double[] v = new double[arrayLength];
            for (int n = 0; n < arrayLength; n++)
                v[n] = rand.NextDouble() * 2 - 1;
            return v;
        }

        public static double[] Solve(double[,] Mv, double[] vv)
        {
            if (Mv.GetLength(0) != Mv.GetLength(1))
                throw new ArgumentException("Mv must have identical length and width");

            if (Mv.GetLength(0) != vv.Length)
                throw new ArgumentException("Mv and vv must have identical length");


            double[,] M = Mv.Clone() as double[,];
            double[] v = vv.Clone() as double[];

            int arrayLength = Mv.GetLength(0);
            int arrayWidth = Mv.GetLength(1);

            // row reduce
            int row = 0;
            int column = 0;
            while ((row < arrayLength) && (column < arrayWidth))
            {
                // look for the largest number in the column m
                double maxv = 0;
                int maxn = -10;

                for (int tn = row; tn < arrayLength; tn++)
                {
                    if (Math.Abs(M[tn, column]) > maxv)
                    {
                        maxv = Math.Abs(M[tn, column]);
                        maxn = tn;
                    }
                }

                if (maxn < 0)
                {
                    column++;
                    continue;
                }

                // order
                if (maxn != row)
                {
                    double h;

                    for (int j = column; j < arrayWidth; j++)
                    {
                        h = M[row, j];
                        M[row, j] = M[maxn, j];
                        M[maxn, j] = h;
                    }

                    h = v[row];
                    v[row] = v[maxn];
                    v[maxn] = h;
                }

                // normalize
                for (int j = column + 1; j < arrayWidth; j++)
                    M[row, j] /= M[row, column];
                v[row] /= M[row, column];
                M[row, column] = 1;

                // reduce
                for (int tn = 0; tn < arrayLength; tn++)
                {
                    if (tn != row)
                    {
                        for (int j = column + 1; j < arrayWidth; j++)
                            M[tn, j] -= M[row, j] * M[tn, column];
                        v[tn] -= v[row] * M[tn, column];
                        M[tn, column] = 0;
                    }
                }

                row++;
                column++;
            }

            // solution
            double[] x = new double[arrayWidth];
            for (int j = 0; j < arrayWidth; j++)
                x[j] = 0;

            row = 0;
            column = 0;
            while ((row < arrayLength) && (column < arrayWidth))
            {

                // look for the first 1
                if (M[row, column] == 0)
                {
                    column++;
                    continue;
                }

                // set value
                x[column] = v[row];

                row++;
                column++;
            }

            return x;

        }

        public static double[,] Identity(int num)
        {
            double[,] r = new double[num, num];
            for (int j = 0; j < num; j++)
                for (int k = 0; k < num; k++)
                    r[j, k] = (j == k) ? 1 : 0;
            return r;
        }

        public static double ScalarProduct(double[] a, double[] b)
        {
            double r = 0;
            for (int n = 0; n < a.Length; n++)
                r += a[n] * b[n];
            return r;
        }

        public static double[] ScalarMultiply(double[] a, double b)
        {
            double[] r = new double[a.Length];
            for (int n = 0; n < r.Length; n++)
                r[n] = a[n] * b;
            return r;
        }

        public static double[] Product(double[,] A, double[] v)
        {
            if (A.GetLength(0) != A.GetLength(1))
                throw new ArgumentException("A must have identical length and width");

            if (A.GetLength(0) != v.Length)
                throw new ArgumentException("A and v must have identical width");

            int arrayLength = A.GetLength(0);
            double[] r = new double[arrayLength];
            for (int n = 0; n < r.Length; n++)
            {
                r[n] = 0;
                for (int j = 0; j < arrayLength; j++)
                    r[n] += A[n, j] * v[j];
            }
            return r;
        }

        public static double[,] Product(double[,] A, double[,] B)
        {
            if (A.GetLength(0) != A.GetLength(1))
                throw new ArgumentException("A must have identical length and width");

            if (A.GetLength(0) != A.GetLength(1))
                throw new ArgumentException("B must have identical length and width");

            if (A.GetLength(0) != B.GetLength(0))
                throw new ArgumentException("A and B must have identical dimensions");

            int arraySize = A.GetLength(0);
            double[,] R = new double[arraySize, arraySize];
            for (int n = 0; n < arraySize; n++)
                for (int m = 0; m < arraySize; m++)
                {
                    R[n, m] = 0;
                    for (int j = 0; j < arraySize; j++)
                        R[n, m] += A[n, j] * B[j, m];
                }
            return R;
        }

        public static double[,] Transpose(double[,] M)
        {
            if (M.GetLength(0) != M.GetLength(1))
                throw new ArgumentException("M must have identical length and width");

            int arraySize = M.GetLength(0);
            double[,] R = new double[arraySize, arraySize];
            for (int n = 0; n < arraySize; n++)
                for (int m = 0; m < arraySize; m++)
                    R[n, m] = M[m, n];
            return R;
        }

        public static double[] Sum(double a, double[] A, double b, double[] B)
        {
            if (A.Length != B.Length)
                throw new ArgumentException("A and B must have identical length");

            int arraySize = A.Length;
            double[] R = new double[arraySize];
            for (int n = 0; n < arraySize; n++)
                R[n] = a * A[n] + b * B[n];
            return R;
        }

        public static double[,] Sum(double a, double[,] A, double b, double[,] B)
        {
            if (A.GetLength(0) != A.GetLength(1))
                throw new ArgumentException("A must have identical length and width");

            if (A.GetLength(0) != A.GetLength(1))
                throw new ArgumentException("B must have identical length and width");

            if (A.GetLength(0) != B.GetLength(0))
                throw new ArgumentException("A and B must have identical dimensions");

            int arraySize = A.GetLength(0);

            double[,] R = new double[arraySize, arraySize];
            for (int n = 0; n < arraySize; n++)
                for (int m = 0; m < arraySize; m++)
                    R[n, m] = a * A[n, m] + b * B[n, m];
            return R;
        }

    }
}
