using System;
using System.Linq;

namespace MatrixCalc
{
    /// <summary>
    ///     Class that represents matrix itself
    /// </summary>
    public class Matrix
    {
        /// <summary>
        ///     Matrix
        /// </summary>
        private double[][] _matrix;

        /// <summary>
        ///     Initialize Matrix
        /// </summary>
        /// <param name="matrix"></param>
        public Matrix(double[][] matrix)
        {
            _matrix = matrix;
        }

        /// <summary>
        /// Size of column
        /// </summary>
        public int ColumnSize => _matrix.Length;
        
        /// <summary>
        /// Size of row
        /// </summary>
        public int RowSize => _matrix[0].Length;

        /// <summary>
        ///     Setting matrix item with item
        /// </summary>
        /// <param name="item">Item to set</param>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <returns>If cell was modified</returns>
        public bool SetItem(double item, int columnIndex, int rowIndex)
        {
            try
            {
                _matrix[columnIndex][rowIndex] = item;
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        /// <summary>
        ///     Getting item
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <returns>Cell value</returns>
        public double GetItem(int columnIndex, int rowIndex)
        {
            return _matrix[columnIndex][rowIndex];
        }

        /// <summary>
        ///     ToString overrider to beautify output
        /// </summary>
        /// <returns>Matrix as string</returns>
        public override string ToString()
        {
            var matrix = "";

            // Create array of longest number in a column
            var rowLengths = GetArrayOfLongestItemsInColumns();

            for (var columnIndex = 0; columnIndex < _matrix.Length; columnIndex++)
            {
                // Getting leading symbols based on column index
                var (leadingSymbol, endingSymbol) = GetLeadingAndEndingChar(columnIndex);

                // Create row with starting symbol
                var row = $"{leadingSymbol}  ";

                for (var rowIndex = 0; rowIndex < _matrix[0].Length; rowIndex++)
                {
                    // Getting matrix cell
                    var item = _matrix[columnIndex][rowIndex];

                    // Create spaces to max length 
                    var spaces = new string(' ', rowLengths[rowIndex] - item.ToString().Length + 2);

                    // Adding item and spaces to row
                    row += item + spaces;
                }

                // Adding ending symbol to row
                row += $"{endingSymbol}\n";

                // Adding row to matrix
                matrix += row;
            }

            return matrix;
        }

        /// <summary>
        ///     Gets matrix with replaced current symbol
        /// </summary>
        /// <returns>Matrix as string</returns>
        public string ToStringWithReplacedCurrentSymbol()
        {
            // Getting matrix as string
            var matrix = ToString();

            // Getting string that will be replaced
            var stringToReplace = double.PositiveInfinity.ToString();

            // Getting first empty cell index
            var firstEmptyCellIndex = matrix.IndexOf(stringToReplace);

            // Replacing string
            matrix = matrix.Remove(firstEmptyCellIndex, stringToReplace.Length).Insert(firstEmptyCellIndex, "_");

            return matrix;
        }

        /// <summary>
        ///     Multiply matrix by number
        /// </summary>
        /// <param name="number"></param>
        public void MultiplyByNumber(double number)
        {
            for (var columnIndex = 0; columnIndex < ColumnSize; columnIndex++)
            for (var rowIndex = 0; rowIndex < RowSize; rowIndex++)
            {
                // Getting item of matrix
                var item = GetItem(columnIndex, rowIndex);

                // Multiply item by number
                item = Math.Round(item * number, 6);

                // Setting item to matrix
                SetItem(item, columnIndex, rowIndex);
            }
        }

        /// <summary>
        ///     Add matrix to matrix
        /// </summary>
        /// <param name="matrixToAdd"></param>
        /// <param name="difference">If minus</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddToMatrix(Matrix matrixToAdd, bool difference = false)
        {
            // Check matrices' size equality
            if (Validator.AreMatricesHaveEqualSize(this, matrixToAdd) is false) throw new ArgumentException("Matrices have different sizes");

            for (var columnIndex = 0; columnIndex < ColumnSize; columnIndex++)
            for (var rowIndex = 0; rowIndex < RowSize; rowIndex++)
            {
                // Getting item of matrix
                var item1 = GetItem(columnIndex, rowIndex);
                var item2 = matrixToAdd.GetItem(columnIndex, rowIndex);

                // Add item to item
                item1 = difference ? Math.Round(item1 - item2, 6) : Math.Round(item1 + item2, 6);

                // Setting item to matrix
                SetItem(item1, columnIndex, rowIndex);
            }
        }

        /// <summary>
        ///     Multiply matrix by matrix
        /// </summary>
        /// <param name="matrixToMultiply"></param>
        /// <exception cref="ArgumentException"></exception>
        public void MultiplyByMatrix(Matrix matrixToMultiply)
        {
            // Check if matrices can be multiplied
            if (Validator.AreMatricesCanBeMultiplied(this, matrixToMultiply) is false)
                throw new ArgumentException("Matrices can not be multiplied due to sizes");

            // Create empty array
            var matrix = new double[ColumnSize][];

            for (var columnIndex = 0; columnIndex < ColumnSize; columnIndex++)
            {
                // Create empty row
                var row = new double[matrixToMultiply.RowSize];

                for (var rowIndex = 0; rowIndex < matrixToMultiply.RowSize; rowIndex++)
                {
                    double item = 0;

                    for (var itemIndex = 0; itemIndex < RowSize; itemIndex++)
                        // Adding items
                        item += GetItem(columnIndex, itemIndex) *
                                matrixToMultiply.GetItem(itemIndex, rowIndex);

                    // Set item to row
                    row[rowIndex] = item;
                }

                // Set row to matrix
                matrix[columnIndex] = row;
            }

            _matrix = matrix;
        }

        /// <summary>
        ///     Transpose matrix
        /// </summary>
        public void Transpose()
        {
            // Change rows and columns
            _matrix = _matrix[0].Select((item, rowIndex) =>
                _matrix.Select((row, columnIndex) => _matrix[columnIndex][rowIndex]).ToArray()).ToArray();
        }

        /// <summary>
        ///     Getting array of longest items in columns
        /// </summary>
        /// <returns>Array of integers</returns>
        private int[] GetArrayOfLongestItemsInColumns()
        {
            // Getting longest items in Column
            var rowLengths = _matrix[0].Select((item, rowIndex) => _matrix.Select((row, columnIndex) =>
                _matrix[columnIndex][rowIndex].ToString()).OrderByDescending(s => s.Length).First().Length).ToArray();

            return rowLengths;
        }

        /// <summary>
        ///     Getting trace of matrix
        /// </summary>
        /// <returns>Trace of matrix</returns>
        /// <exception cref="ArgumentException"></exception>
        public double GetTrace()
        {
            // Check if matrix is square
            if (Validator.IsMatrixSquare(this) is false) throw new ArgumentException("Matrix must be square");

            double trace = 0;

            for (var index = 0; index < RowSize; index++)
                // Adding diagonal
                trace += GetItem(index, index);

            return trace;
        }

        /// <summary>
        ///     Getting leading and ending symbols based on row index
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns>Array of symbols</returns>
        private (string, string) GetLeadingAndEndingChar(int columnIndex)
        {
            string leadingSymbol;
            string endingSymbol;

            if (columnIndex == 0) // First row
            {
                leadingSymbol = "┌";
                endingSymbol = "┐";
            }
            else if (columnIndex == _matrix.Length - 1) // Last row
            {
                leadingSymbol = "└";
                endingSymbol = "┘";
            }
            else // Every other row
            {
                leadingSymbol = "│";
                endingSymbol = "│";
            }

            return (leadingSymbol, endingSymbol);
        }
    }
}