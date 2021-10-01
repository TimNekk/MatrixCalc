using System;
using System.Linq;

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
        public static Matrix CreateUserMatrix()
        {
            // Getting size from user
            var (columnSize, rowSize) = AskUserForMatrixSize();

            // Getting matrix data from user
            var matrix = AskUserForMatrixData(columnSize, rowSize);

            return matrix;
        }

        /// <summary>
        ///     Create empty matrix
        /// </summary>
        /// <param name="columnSize"></param>
        /// <param name="rowSize"></param>
        /// <returns>Empty matrix</returns>
        public static Matrix CreateEmptyMatrix(int columnSize, int rowSize)
        {
            // Initialize empty array
            var matrixData = new double[columnSize][];

            for (var columnIndex = 0; columnIndex < columnSize; columnIndex++)
            {
                // Initialize empty row
                var row = new double[rowSize];

                for (var rowIndex = 0; rowIndex < rowSize; rowIndex++)
                    // Set infinity as item 
                    row[rowIndex] = double.PositiveInfinity;

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
        /// <param name="columnSize"></param>
        /// <param name="rowSize"></param>
        /// <returns>Random matrix</returns>
        public static Matrix CreateRandomMatrix(int columnSize, int rowSize)
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
        private static (int, int) AskUserForMatrixSize()
        {
            var currentInput = "";
            int columnSize = 0, rowSize = 0; // Default sizes
            var workingWithFirstNumber = true; // Whether we work with first number 
            var offset = 3; // Offset of cursor

            while (true)
            {
                // Getting user input key
                var numberBeforeX = workingWithFirstNumber ? currentInput : columnSize.ToString(); // Column Size
                var numberAfterX = workingWithFirstNumber ? " " : currentInput; // Row Size
                var askLine = $"Введите размер матрицы: {numberBeforeX}x{numberAfterX} "; // Line to ask
                var userInput = ConsoleController.AskUserOneCharWithOffset(askLine, offset);

                if (Validator.IsNumber(userInput.KeyChar) && (userInput.KeyChar == '0' && currentInput == "") is false) // If a number
                {
                    // Adding key to currentInput
                    currentInput += userInput.KeyChar.ToString();
                }
                else if (userInput.Key is ConsoleKey.Tab or ConsoleKey.Enter && currentInput != "") // If Tab or enter
                {
                    if (workingWithFirstNumber is false) // If last number was entered
                    {
                        rowSize = int.Parse(currentInput);
                        break;
                    }

                    columnSize = int.Parse(currentInput); // Setting column size
                    currentInput = ""; // Reset current input
                    workingWithFirstNumber = false; // Now working with second number
                    offset = 1; // Adjust offset to work with second number
                }
                else if (userInput.Key is ConsoleKey.Backspace && currentInput != "") // If backspace
                {
                    currentInput = currentInput[..^1]; // Remove last char
                }
            }

            return (columnSize, rowSize);
        }

        /// <summary>
        ///     Getting data from user
        /// </summary>
        /// <param name="columnSize"></param>
        /// <param name="rowSize"></param>
        /// <returns>Matrix</returns>
        private static Matrix AskUserForMatrixData(int columnSize, int rowSize)
        {
            // Creating empty matrix
            var matrix = CreateEmptyMatrix(columnSize, rowSize);

            Console.Clear();

            for (var columnIndex = 0; columnIndex < columnSize; columnIndex++)
            for (var rowIndex = 0; rowIndex < rowSize; rowIndex++)
            {
                var currentInput = "";
                while (true)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(matrix.ToStringWithReplacedCurrentSymbol() + "\n\n"); // Printing Matrix
                    var askLine = $"Введите число: {currentInput} "; // Line to ask
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
        /// <param name="userInput"></param>
        /// <param name="currentInput"></param>
        /// <returns>validators</returns>
        private static (bool, bool, bool) GetAllowValidators(ConsoleKeyInfo userInput, string currentInput)
        {
            // Checking if minus can be placed
            var minusAllowed = userInput.KeyChar == '-' && currentInput.Length == 0;

            // Checking if dot can be placed
            var dotAllowed = userInput.KeyChar is '.' or ',' &&
                             currentInput.Length != 0 &&
                             currentInput.Replace(',', '.').Any(s => s == '.');

            // Checking if zero can be placed
            var zeroAllowed = userInput.KeyChar == '0' && (currentInput.Length == 1 && currentInput[0] != '0' || currentInput.Length != 1);

            return (minusAllowed, dotAllowed, zeroAllowed);
        }
    }
}