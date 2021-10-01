using System;
using System.Linq;

namespace MatrixCalc
{
    public static class MatrixCreator
    {
        public static void CreateUserMatrix()
        {
            var (columnSize, rowSize) =  AskUserForMatrixSize();
            AskUserForMatrixData(columnSize, rowSize);
        }

        public static Matrix CreateEmptyMatrix(int columnSize, int rowSize)
        {
            double[][] matrixData = new double[columnSize][];

            for (var columnIndex = 0; columnIndex < rowSize; columnIndex++)
            {
                double[] row = new double[columnSize];
                
                for (var rowIndex = 0; rowIndex < columnSize; rowIndex++)
                {
                    row[rowIndex] = -999999;
                }
                
                matrixData[columnIndex] = row;
            }

            Matrix matrix = new Matrix(matrixData);
            return matrix;
        }


        public static Matrix CreateRandomMatrix(int columnSize, int rowSize)
        {
            double[][] matrixData = new double[columnSize][];

            for (var columnIndex = 0; columnIndex < rowSize; columnIndex++)
            {
                double[] row = new double[columnSize];
                
                for (var rowIndex = 0; rowIndex < columnSize; rowIndex++)
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
            string currentInput = "";
            int columnSize = 0;
            int rowSize = 0;
            bool workingWithFirstNumber = true; // Whether we work with first number 
            int offset = 3; // Offset of cursor
            
            while (true)
            {
                // Getting user input key
                string numberBeforeX = workingWithFirstNumber ? currentInput : columnSize.ToString();  // Column Size
                string numberAfterX = workingWithFirstNumber ? " " : currentInput;  // Row Size
                string askLine = $"Введите размер матрицы: {numberBeforeX}x{numberAfterX} ";  // Line to ask
                ConsoleKeyInfo userInput = ConsoleController.AskUserOneCharWithOffset(askLine, offset);
                
                if (Validator.IsNumber(userInput.KeyChar) && (userInput.KeyChar == '0' && currentInput == "") is false) // If a number
                {
                    currentInput += userInput.KeyChar.ToString();
                }
                else if (userInput.Key is ConsoleKey.Tab or ConsoleKey.Enter && currentInput != "") // If Tab or enter
                {
                    if (workingWithFirstNumber is false)  // If last number was entered
                    {
                        rowSize = int.Parse(currentInput);
                        break;
                    }
                    
                    columnSize = int.Parse(currentInput);  // Setting column size
                    currentInput = "";  // Reset current input
                    workingWithFirstNumber = false;  // Now working with second number
                    offset = 1;  // Adjust offset to work with second number
                }
                else if (userInput.Key is ConsoleKey.Backspace && currentInput != "") // If backspace
                {
                    currentInput = currentInput[..^1];  // Remove last char
                }
            }
            
            return (columnSize, rowSize);
        }

        private static void AskUserForMatrixData(int columnSize, int rowSize)
        {
            Matrix matrix = CreateEmptyMatrix(columnSize, rowSize);
            Console.Clear();

            for (int columnIndex = 0; columnIndex < columnSize; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < rowSize; rowIndex++)
                {
                    string currentInput = "";

                    while (true)
                    {
                        Console.SetCursorPosition(0,0);
                        Console.WriteLine(matrix.ToStringWithZerosAsUnderscores() + "\n\n");
                        string askLine =  $"Введите число: {currentInput} ";  // Line to ask
                        ConsoleKeyInfo userInput = ConsoleController.AskUserOneCharWithOffset(askLine, 1);

                        bool minusAllowed = userInput.KeyChar == '-' && currentInput.Length == 0;
                        bool dotAllowed = userInput.KeyChar is '.' or ',' && 
                                          currentInput.Length != 0 && 
                                          currentInput.Replace(',', '.').Count(s => s == '.') == 0;
                        bool zeroAllowed = userInput.KeyChar == '0' && ((currentInput.Length == 1 && currentInput[0] != '0') || currentInput.Length != 1);
                        if (minusAllowed || dotAllowed || zeroAllowed || Validator.IsNumber(userInput.KeyChar, false)) // If a number or minus
                        {
                            currentInput += userInput.KeyChar.ToString();
                        }
                        else if (userInput.Key is ConsoleKey.Tab or ConsoleKey.Enter && currentInput != "") // If Tab or enter
                        {
                            matrix.SetCell(double.Parse(currentInput.Replace('.', ',')), columnIndex, rowIndex);
                            break;
                        } 
                    }
                }
            }
        }
    }
}