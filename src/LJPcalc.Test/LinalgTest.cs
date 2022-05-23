using NUnit.Framework;
using LJPcalc.Core.Solver;

namespace LJPcalc.Test;

class LinalgTest
{
    [Test]
    public void Test_SolveRandomDataset_SolutionIsValid()
    {
        Random rand = new Random(0);

        int arrayLength = 10;
        double[,] input2d = LinearAlgebra.RandomArray2d(arrayLength, arrayLength, rand);
        double[] input1d = LinearAlgebra.RandomArray1d(arrayLength, rand);

        double[] solution = LinearAlgebra.Solve(input2d, input1d);
        double[] product = LinearAlgebra.Product(input2d, solution);

        for (int rowIndex = 0; rowIndex < arrayLength; rowIndex++)
        {
            double difference = Math.Abs(product[rowIndex] - input1d[rowIndex]);
            Console.WriteLine($"row {rowIndex + 1} difference: {difference}");
            Assert.That(difference < 1E-10);
        }
    }
}
