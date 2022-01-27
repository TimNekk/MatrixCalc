using System;
using System.Linq;
using src;

namespace MatrixCalc
{
    /// <summary>
    ///     Class that is able to create matrix in different ways
    /// </summary>
    public static class MatrixCreator
    {
        /// <summary>
        ///     Create Matrix with user data
        /// </summary>
        /// <returns>Matrix</returns>
        public static Matrix CreateUserMatrix(bool useSOLE = false, int columnSize = 0, int rowSize = 0)
        {
            MenuController.ShowLogo();

            // Getting matrix data from user
            var matrix = AskUserForMatrixData(columnSize, rowSize, useSOLE);

            return matrix;
        }

        /// <summary>
        ///     Create matrix from file
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <param name="toRange">Maximum matrix size</param>
        /// <param name="mustBeSquare">If matrix must be square</param>
        /// <param name="columnSize">Pre-Determined size of column</param>
        /// <param name="rowSize">Pre-Determined size of row</param>
        /// <returns>Created matrix</returns>
        public static Matrix CreateMatrixFromFile(string fileName = "input.txt", int toRange = int.MaxValue, bool mustBeSquare = false,
            int columnSize = 0, int rowSize = 0)
        {
            // Reading matrix data from file
            string matrixDataFromFile = FileReader.GetTextFromFile(fileName);
            double[][] matrixData;
            
            // Converting to doubles array
            matrixData = matrixDataFromFile
                .Replace('.', ',')
                .Split('\n')
                .Select(row => row.Split(" ").Select(double.Parse).ToArray()).ToArray();

            // Creating matrix with given data
            Matrix matrix = new Matrix(matrixData);

            // Validate matrix
            if (mustBeSquare && (Validator.IsMatrixSquare(matrix) is false)) 
                throw new ArgumentException("Матрица должна быть квадратной");

            if (matrix.ColumnSize > toRange || matrix.RowSize > toRange) 
                throw new ArgumentException($"Размер матрицы не должен превышать {toRange}");
            
            if (columnSize != 0 && columnSize != matrix.ColumnSize || rowSize != 0 && rowSize != matrix.RowSize)
                throw new ArgumentException("Матрицы должны быть правильных размеров");

            return matrix;
        }


        /// <summary>
        ///     Create empty matrix
        /// </summary>
        /// <param name="columnSize">Size of column</param>
        /// <param name="rowSize">Size of row</param>
        /// <param name="useZeros">Use zeros instead of infinities</param>
        /// <returns>Empty matrix</returns>
        public static Matrix CreateEmptyMatrix(int columnSize, int rowSize, bool useZeros = false)
        {
            // Initialize empty array
            var matrixData = new double[columnSize][];

            for (var columnIndex = 0; columnIndex < columnSize; columnIndex++)
            {
                // Initialize empty row
                var row = new double[rowSize];

                for (var rowIndex = 0; rowIndex < rowSize; rowIndex++)
                    // Set infinity as item 
                    row[rowIndex] = useZeros ? 0 : double.PositiveInfinity;

                // Put row into array
                matrixData[columnIndex] = row;
            }

            // Create matrix with created matrix data
            var matrix = new Matrix(matrixData);
            return matrix;
        }


        /// <summary>
        ///     Create random matrix
        /// </summary>
        /// <param name="columnSize">Size of column</param>
        /// <param name="rowSize">Size of row</param>
        /// <returns>Random matrix</returns>
        public static Matrix CreateRandomMatrix(int columnSize, int rowSize, bool rounded = false)
        {
            // Initialize empty array
            var matrixData = new double[columnSize][];

            for (var columnIndex = 0; columnIndex < columnSize; columnIndex++)
            {
                // Initialize empty row
                var row = new double[rowSize];

                for (var rowIndex = 0; rowIndex < rowSize; rowIndex++)
                {
                    // Getting random number
                    var randomNumber = GenerateRandomDouble();
                    if (rounded) randomNumber = Math.Round(randomNumber);
                    row[rowIndex] = randomNumber;
                }

                // Put row into array
                matrixData[columnIndex] = row;
            }

            // Create matrix with created matrix data
            var matrix = new Matrix(matrixData);
            return matrix;
        }

        /// <summary>
        ///     Generating random number
        /// </summary>
        /// <param name="fromRange">From range of generator</param>
        /// <param name="toRange">To range of generator</param>
        /// <param name="accuracy">Rounding accuracy</param>
        /// <returns>Random double</returns>
        private static double GenerateRandomDouble(int fromRange = -100, int toRange = 101, int accuracy = 1)
        {
            // Initiate random instance
            var random = new Random();

            // Generating random double
            var randomDouble = random.NextDouble();
            var randomInt = random.Next(fromRange, toRange);
            var randomNumber = Math.Round(randomDouble * randomInt, accuracy);

            return randomNumber;
        }

        /// <summary>
        ///     Getting size from user
        /// </summary>
        /// <returns>Size Tuple</returns>
        public static (int, int) AskUserForMatrixSize(int toRange = 10, bool mustBeSquare = false, bool useSOLE = false,
            int columnSize = 0, int rowSize = 0)
        {
            var currentInput = "";
            var workingWithFirstNumber = columnSize == 0; // Whether we work with first number
            var offset = columnSize == 0 ? 3 : 1; // Offset of cursor

            if (mustBeSquare) Console.WriteLine("Матрица должна быть квадратной\n");

            while (true)
            {
                // Getting user input key
                var numberBeforeX = workingWithFirstNumber ? currentInput : columnSize.ToString(); // Column Size
                var numberAfterX = workingWithFirstNumber ? " " : currentInput; // Row Size
                var askLine = " - Введите " +
                              (useSOLE ? "кол-во уравнений и переменных" : "размер матрицы") +
                              (toRange == int.MaxValue ? "" : $" (от {1}x{1} до {toRange}x{toRange})") +
                              $": {numberBeforeX}x{numberAfterX} "; // Line to ask
                var userInput = ConsoleController.AskUserOneCharWithOffset(askLine, offset);

                if (Validator.IsNumber(userInput.KeyChar) &&
                    int.Parse(currentInput + userInput.KeyChar) >= 1 &&
                    int.Parse(currentInput + userInput.KeyChar) <= toRange
                ) { currentInput += userInput.KeyChar.ToString(); } // Adding key to currentInput
                
                else if (userInput.Key is ConsoleKey.Tab or ConsoleKey.Enter && currentInput != "" &&
                         (workingWithFirstNumber || !mustBeSquare || columnSize == int.Parse(currentInput + userInput.KeyChar)))
                {
                    if (workingWithFirstNumber is false) { rowSize = int.Parse(currentInput); break; }

                    columnSize = int.Parse(currentInput); // Setting column size
                    if (rowSize != 0) break;
                    currentInput = ""; // Reset current input
                    workingWithFirstNumber = false; // Now working with second number
                    offset = 1; // Adjust offset to work with second number
                }
                
                else if (userInput.Key is ConsoleKey.Backspace && currentInput != "") // If backspace
                { currentInput = currentInput[..^1]; } // Remove last char
            }
            
            return (columnSize, useSOLE ? rowSize + 1 : rowSize);
        }

        /// <summary>
        ///     Getting data from user
        /// </summary>
        /// <param name="columnSize">Size of column</param>
        /// <param name="rowSize">Size of row</param>
        /// <param name="useSOLE">Ask inputs as SOLE</param>
        /// <returns>Matrix</returns>
        private static Matrix AskUserForMatrixData(int columnSize, int rowSize, bool useSOLE = false)
        {
            // Creating empty matrix
            var matrix = CreateEmptyMatrix(columnSize, rowSize);

            var logoHeight = FileReader.GetLogoHeight();

            for (var columnIndex = 0; columnIndex < columnSize; columnIndex++)
            for (var rowIndex = 0; rowIndex < rowSize; rowIndex++)
            {
                var currentInput = "";
                while (true)
                {
                    Console.SetCursorPosition(0, logoHeight);
                    ConsoleController.PrintWithColor(matrix.ToStringWithReplacedCurrentSymbol(useSOLE) + "\n", ConsoleColor.Cyan); // Printing Matrix
                    var askLine = $" - Введите число: {currentInput} "; // Line to ask
                    var userInput = ConsoleController.AskUserOneCharWithOffset(askLine, 1); // Asking for input

                    // Getting Validators
                    var (minusAllowed, dotAllowed, zeroAllowed) = GetAllowValidators(userInput, currentInput);

                    if (minusAllowed || dotAllowed || zeroAllowed || Validator.IsNumber(userInput.KeyChar, false))
                    {
                        // Adding key to currentInput
                        currentInput += userInput.KeyChar.ToString();
                    }
                    else if (userInput.Key is ConsoleKey.Tab or ConsoleKey.Enter &&
                             currentInput != "-" && currentInput != "") // If Tab or enter
                    {
                        // Putting item to matrix
                        matrix.SetItem(double.Parse(currentInput.Replace('.', ',')), columnIndex, rowIndex);
                        break;
                    }
                    else if (userInput.Key is ConsoleKey.Backspace && currentInput != "") // If backspace
                    {
                        currentInput = currentInput[..^1]; // Remove last char
                    }
                }
            }

            return matrix;
        }

        /// <summary>
        ///     Getting validators
        /// </summary>
        /// <param name="userInput">User input key</param>
        /// <param name="currentInput">Current input</param>
        /// <returns>Validators</returns>
        private static (bool, bool, bool) GetAllowValidators(ConsoleKeyInfo userInput, string currentInput)
        {
            // Checking if minus can be placed
            var minusAllowed = userInput.KeyChar == '-' && currentInput.Length == 0;

            // Checking if dot can be placed
            var dotAllowed = userInput.KeyChar is '.' or ',' &&
                             currentInput.Length != 0 &&
                             currentInput.Replace(',', '.').Any(s => s == '.') is false;

            // Checking if zero can be placed
            var zeroAllowed = userInput.KeyChar == '0' && (currentInput.Length == 1 && currentInput[0] != '0' || currentInput.Length != 1);

            return (minusAllowed, dotAllowed, zeroAllowed);
        }
    }
}