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
            // var matrix1 = MatrixCreator.CreateRandomMatrix(4, 5, true);
            var matrix1 = MatrixCreator.CreateUserMatrix(true);
            Console.Clear();
            // var matrix2 = MatrixCreator.CreateUserMatrix();
            // Console.Clear();
            // var matrix1 = MatrixCreator.CreateRandomMatrix(1, 2);
            // var matrix2 = MatrixCreator.CreateRandomMatrix(2, 1);

            ;
            // matrix1.SolveByGaussianElimination();
            // matrix1.RemoveColumn(1);
            Console.WriteLine(matrix1);
            Console.WriteLine(matrix1.ToStringAsSOLE());
            // matrix1.Transpose();
            // Console.WriteLine(matrix1.SolveByCramersRule().ToStringAsSOLE());
            Console.WriteLine(matrix1.SolveByGaussianElimination().ToStringAsSOLE(true));
            Console.WriteLine(matrix1.SolveByGaussianElimination());

            // matrix1.MultiplyByMatrix(matrix2);


            Console.ReadKey();
        }
    }
}