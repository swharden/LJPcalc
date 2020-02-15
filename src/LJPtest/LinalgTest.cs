using NUnit.Framework;
using System;

namespace LJPtest
{
    class LinalgTest
    {
        [Test]
        public void Test_SolveRandomDataset_SolutionIsValid()
        {
            Random rand = new Random(0);

            int arrayLength = 10;
            double[,] input2d = LJPmath.Linalg.RandomArray2d(arrayLength, arrayLength, rand);
            double[] input1d = LJPmath.Linalg.RandomArray1d(arrayLength, rand);

            double[] solution = LJPmath.Linalg.Solve(input2d, input1d);
            double[] product = LJPmath.Linalg.Product(input2d, solution);

            for (int rowIndex = 0; rowIndex < arrayLength; rowIndex++)
            {
                double difference = Math.Abs(product[rowIndex] - input1d[rowIndex]);
                Console.WriteLine($"row {rowIndex + 1} difference: {difference}");
                Assert.That(difference < 1E-10);
            }
        }
    }
}
