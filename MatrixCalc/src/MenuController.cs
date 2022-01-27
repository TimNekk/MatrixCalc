using System;
using System.Collections.Generic;
using System.Linq;
using src;

namespace MatrixCalc
{
    public static class MenuController
    {
        /// <summary>
        ///     All available actions
        /// </summary>
        private static readonly string[] AvailableActions =
        {
            "Найти след матрицы",
            "Транспонировать матрицу",
            "Сложить матрицы",
            "Вычесть матрицы",
            "Переможить матрицы",
            "Умножить матрицу на число",
            "Найти определитель матрицы",
            "Решить СЛАУ методом Гаусса",
            "Решить СЛАУ методом Крамера"
        };

        /// <summary>
        ///     All available matrix input options
        /// </summary>
        private static readonly string[] AvailableInputOption =
        {
            "Ввести вручную",
            "Создать случайную",
            "Создать случайную (целые числа)",
            "Прочитать из файла"
        };

        private static readonly string SplitLine = $"\t{new string('─', FileReader.GetLogoWidth() - 16)}\n";


        /// <summary>
        ///     Showing logo on empty screen
        /// </summary>
        public static void ShowLogo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            FileReader.PrintTextFromFile("logo.txt");
            Console.ResetColor();
        }

        /// <summary>
        ///     Showing menu
        /// </summary>
        public static void ShowMenu()
        {
            ShowLogo();
            ShowArray(AvailableActions);
            Console.WriteLine("Нажмите ESC чтобы выйти");
        }

        /// <summary>
        ///     Showing Input Options
        /// <param name="actionIndex">Index of action</param>
        /// </summary>
        public static void ShowInputOptions(int actionIndex)
        {
            ShowLogo();
            ShowArray(actionIndex is 7 or 8 ? AvailableInputOption[..^1] : AvailableInputOption);
        }

        /// <summary>
        ///     Printing given array
        /// </summary>
        /// <param name="array">Array that will be printed</param>
        private static void ShowArray(IReadOnlyList<string> array)
        {
            for (var index = 0; index < array.Count; index++) Console.WriteLine($"\t{index + 1}) {array[index]}");
            Console.WriteLine('\n');
        }

        /// <summary>
        ///     Ask user for action index
        /// </summary>
        /// <returns>Action index</returns>
        public static int AskUserForAction()
        {
            return ConsoleController.AskUserForNumber(" - Введите номер операции", AvailableActions.Length) - 1;
        }

        /// <summary>
        ///     Ask user for input method index
        /// </summary>
        /// <param name="actionIndex">Index of action</param>
        /// <returns>Input method index</returns>
        public static int AskUserForInputMethod(int actionIndex)
        {
            return ConsoleController.AskUserForNumber(" - Введите номер операции", 
                AvailableInputOption.Length - (actionIndex is 7 or 8 ? 1 : 0)) - 1;
        }

        /// <summary>
        ///     Does given action base on index
        /// </summary>
        /// <param name="actionIndex">Index of action to perform</param>
        /// <param name="inputOptionIndex">Index of input option to perform</param>
        public static void DoAction(int actionIndex, int inputOptionIndex)
        {
            // Getting settings
            var (toRange, mustBeSquare, useSOLE) = GetParameters(actionIndex);

            // Getting matrices and number for future operations
            Matrix matrix, matrix2;
            double number;
            try
            {
                (matrix, matrix2, number) = GetInputs(actionIndex, inputOptionIndex, toRange, mustBeSquare, useSOLE);
            }
            catch (Exception e)
            {
                Console.WriteLine($" - {e.Message}");
                Console.WriteLine("\nНажмите любую клавишу чтобы выйти в меню");
                Console.ReadKey();
                return;
            }

            // Show user inputs
            ShowInputs(actionIndex, matrix, matrix2, number, useSOLE);

            // Perform the calculations
            var (label, item) = DoOperationWithMatrix(actionIndex, matrix, matrix2, number);

            // Show the result
            Console.WriteLine(label);
            ConsoleController.PrintWithColor(item, ConsoleColor.Cyan);
            Console.WriteLine("\nНажмите любую клавишу чтобы выйти в меню");
            Console.ReadKey();
        }

        /// <summary>
        ///     Getting setting for action
        /// </summary>
        /// <param name="actionIndex">Index of action</param>
        /// <returns>toRange, mustBeSquare, useSOLE</returns>
        private static (int, bool, bool) GetParameters(int actionIndex)
        {
            // Default values
            var toRange = int.MaxValue;
            var mustBeSquare = false;
            var useSOLE = actionIndex is 7 or 8;

            // Setting parameters
            switch (actionIndex)
            {
                case 0 or 8:
                    mustBeSquare = true;
                    break;
                case 6:
                    mustBeSquare = true;
                    toRange = 10;
                    break;
            }

            return (toRange, mustBeSquare, useSOLE);
        }

        /// <summary>
        ///     Getting matrix based on given parameters
        /// </summary>
        /// <param name="inputOptionIndex">Index of input option to perform</param>
        /// <param name="toRange">Maximum matrix size</param>
        /// <param name="mustBeSquare">If matrix must be square</param>
        /// <param name="columnSize">Pre-Determined size of column</param>
        /// <param name="rowSize">Pre-Determined size of row</param>
        /// <param name="useSOLE">Create matrix as SOLE</param>
        /// <param name="fileName">Name of file to read</param>
        /// <returns>Created matrix</returns>
        private static Matrix GetMatrix(int inputOptionIndex, int toRange = int.MaxValue, bool mustBeSquare = false,
            int columnSize = 0, int rowSize = 0, bool useSOLE = false, string fileName = "input.txt")
        {
            ShowLogo();

            // Getting size of new matrix
            if (inputOptionIndex != 3)
            {
                if (columnSize == 0 && rowSize == 0) (columnSize, rowSize) = MatrixCreator.AskUserForMatrixSize(toRange, mustBeSquare, useSOLE);
                else if (columnSize == 0)
                    (columnSize, rowSize) = MatrixCreator.AskUserForMatrixSize(toRange, mustBeSquare, useSOLE, rowSize: rowSize);
                else if (rowSize == 0) (columnSize, rowSize) = MatrixCreator.AskUserForMatrixSize(toRange, mustBeSquare, useSOLE, columnSize);
            }

            // Creating matrix based of inputOptionIndex
            switch (inputOptionIndex)
            {
                case 0:
                    return MatrixCreator.CreateUserMatrix(useSOLE, columnSize, rowSize);
                case 1:
                    return MatrixCreator.CreateRandomMatrix(columnSize, rowSize);
                case 2:
                    return MatrixCreator.CreateRandomMatrix(columnSize, rowSize, true);
                case 3:
                    return MatrixCreator.CreateMatrixFromFile(fileName, toRange, mustBeSquare, columnSize, rowSize);
            }

            // If nothing was created
            return MatrixCreator.CreateEmptyMatrix(1, 1);
        }

        /// <summary>
        ///     Getting matrices and number based on given parameters
        /// </summary>
        /// <param name="actionIndex">Index of action to perform</param>
        /// <param name="inputOptionIndex">Index of input option to perform</param>
        /// <param name="toRange">Maximum matrix size</param>
        /// <param name="mustBeSquare">If matrix must be square</param>
        /// <param name="useSOLE">Create matrix as SOLE</param>
        /// <returns>Two matrices and number</returns>
        private static (Matrix, Matrix, double) GetInputs(int actionIndex, int inputOptionIndex, int toRange, bool mustBeSquare, bool useSOLE)
        {
            // Default matrices and number
            var matrix = GetMatrix(inputOptionIndex, toRange, mustBeSquare, useSOLE: useSOLE);
            var matrix2 = MatrixCreator.CreateEmptyMatrix(2, 3);
            double number = 1;

            // Getting matrices and number based on actionIndex
            switch (actionIndex)
            {
                case 2 or 3:
                    matrix2 = GetMatrix(inputOptionIndex, toRange, mustBeSquare, matrix.ColumnSize, matrix.RowSize, useSOLE, "input2.txt");
                    break;
                case 4:
                    matrix2 = GetMatrix(inputOptionIndex, toRange, mustBeSquare, matrix.RowSize, useSOLE: useSOLE, fileName: "input2.txt");
                    break;
                case 5:
                    ShowLogo();
                    number = ConsoleController.AskUserForNumber(" - Введите число для умножения");
                    break;
            }

            return (matrix, matrix2, number);
        }

        /// <summary>
        ///     Showing that user have already put into program
        /// </summary>
        /// <param name="actionIndex">Index of action to perform</param>
        /// <param name="matrix">Main matrix</param>
        /// <param name="matrix2">Second matrix</param>
        /// <param name="number">The number</param>
        /// <param name="useSOLE">Show matrix as SOLE</param>
        private static void ShowInputs(int actionIndex, Matrix matrix, Matrix matrix2, double number, bool useSOLE)
        {
            ShowLogo();

            // Printing action name
            Console.WriteLine($"\t{AvailableActions[actionIndex]}\n");
            Console.WriteLine(SplitLine);

            // Show main matrix based on useSOLE
            Console.WriteLine(useSOLE ? "\tВходная система\n" : "\tВходная матрица\n");
            ConsoleController.PrintWithColor(useSOLE ? matrix.ToStringAsSOLE() : matrix, ConsoleColor.Cyan);

            // Show second matrix or number based on actionIndex
            switch (actionIndex)
            {
                case 2 or 3 or 4:
                    Console.WriteLine($"\tВходная матрица 2\n");
                    ConsoleController.PrintWithColor(matrix2, ConsoleColor.Cyan);

                    break;
                case 5:
                    Console.WriteLine($"\tВходное число\n");
                    ConsoleController.PrintWithColor("\t" + number + "\n", ConsoleColor.Cyan);
                    break;
            }

            Console.WriteLine(SplitLine);
        }

        /// <summary>
        ///     Perform the given operation with given matrices and number
        /// </summary>
        /// <param name="actionIndex">Index of action to perform</param>
        /// <param name="matrix">Main matrix</param>
        /// <param name="matrix2">Second matrix</param>
        /// <param name="number">The number</param>
        /// <returns>Result of operation</returns>
        private static (string, object) DoOperationWithMatrix(int actionIndex, Matrix matrix, Matrix matrix2, double number)
        {
            switch (actionIndex)
            {
                case 0:
                    var trace = Math.Round(matrix.GetTrace(), 6);
                    return ("\tСлед матрицы равен:\n", "\t" + trace);
                case 1:
                    matrix.Transpose();
                    return ("\tТранспонированная матрица\n", matrix);
                case 2:
                    matrix.AddToMatrix(matrix2);
                    return ("\tРезультат сложения\n", matrix);
                case 3:
                    matrix.AddToMatrix(matrix2, true);
                    return ("\tРезультат вычитания\n", matrix);
                case 4:
                    matrix.MultiplyByMatrix(matrix2);
                    return ("\tРезультат умножения\n", matrix);
                case 5:
                    matrix.MultiplyByNumber(number);
                    return ("\tРезультат умножения\n", matrix);
                case 6:
                    var determinant = Math.Round(matrix.GetDeterminant(), 6);
                    return ("\tОпределитель матрицы равен\n", "\t" + determinant);
                case 7 or 8:
                    try
                    {
                        Matrix solvedMatrix = actionIndex == 7 ? matrix.SolveByGaussianElimination() : matrix.SolveByCramersRule();
                        return ("\tРешение системы\n", solvedMatrix.ToStringAsSOLE(true));
                    }
                    catch (ArgumentException e) // If matrix does not have solution
                    {
                        return ($"\t{e.Message}", "");
                    }
            }

            return ("", "");
        }
    }
}