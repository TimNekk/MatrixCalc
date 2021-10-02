using System;

namespace MatrixCalc
{
    /// <summary>
    ///     Main class
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            // _______89
            var matrix1 = MatrixCreator.CreateUserMatrix();
            Console.Clear();
            // var matrix2 = MatrixCreator.CreateUserMatrix();
            // Console.Clear();
            // var matrix1 = MatrixCreator.CreateRandomMatrix(1, 2);
            // var matrix2 = MatrixCreator.CreateRandomMatrix(2, 1);

            Console.WriteLine(matrix1);
            matrix1.SolveByGaussianElimination();
            Console.WriteLine(matrix1);
            // Console.WriteLine(matrix2);

            // matrix1.MultiplyByMatrix(matrix2);


            Console.ReadKey();
        }
    }
}