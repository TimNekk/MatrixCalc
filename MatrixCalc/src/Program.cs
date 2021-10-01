using System;
using static MatrixCalc.src.Settings;

namespace MatrixCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            // double[][] matrixData = new double[3][];
            // matrixData[0] = new double[] {1, 2, 3};
            // matrixData[1] = new double[] {4, 5, 6};
            // matrixData[2] = new double[] {7, 8, 9};
            //
            // Matrix matrix = new Matrix(matrixData);
            // matrix.Print(MatrixPrintMode.Compact);
            // matrix.Print(MatrixPrintMode.Normal);

            Console.Write("Введите размер матрицы: ");
            Console.ReadLine();

            // Matrix randomMatrix = MatrixCreator.CreateRandomMatrix(3, 3);
            // Console.WriteLine(randomMatrix);
            // Console.ReadKey();
        }
    }
}