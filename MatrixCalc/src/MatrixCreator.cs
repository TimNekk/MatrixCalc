using System;

namespace MatrixCalc
{
    public static class MatrixCreator
    {
        public static void CreateUserMatrix()
        {
            AskUserForMatrixSize();
            
        }
        

        public static Matrix CreateRandomMatrix(int sizeX, int sizeY)
        {
            double[][] matrixData = new double[sizeY][];

            for (var columnIndex = 0; columnIndex < sizeX; columnIndex++)
            {
                double[] row = new double[sizeY];
                
                for (var rowIndex = 0; rowIndex < sizeY; rowIndex++)
                {
                    // Getting random number
                    double randomNumber = GenerateRandomDouble();
                    row[rowIndex] = randomNumber;
                }

                matrixData[columnIndex] = row;
            }

            Matrix matrix = new Matrix(matrixData);
            return matrix;
        }
        
        private static double GenerateRandomDouble(int fromRange=-100, int toRange=101, int accuracy=1)
        {
            // Initiate random instance
            Random random = new Random();
            
            // Generating random double
            double randomDouble = random.NextDouble();
            int randomInt = random.Next(fromRange, toRange);
            double randomNumber = Math.Round(randomDouble * randomInt, accuracy);

            return randomNumber;
        }
        
        private static (int, int) AskUserForMatrixSize()
        {
            Console.WriteLine("Введите размер матрицы: ");
            return (1, 2);
        }
    }
}