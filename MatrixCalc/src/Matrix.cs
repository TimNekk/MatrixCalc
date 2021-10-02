using System;
using System.Collections.Generic;
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
        ///     Size of column
        /// </summary>
        public int ColumnSize => _matrix.Length;

        /// <summary>
        ///     Size of row
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
            // Remove negative Zeros
            SetNegativeZeroToPositiveZeros();
            
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
                    var item =  _matrix[columnIndex][rowIndex];

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
        ///     Getting determinant of the matrix
        /// </summary>
        /// <returns>Determinant</returns>
        /// <exception cref="ArgumentException"></exception>
        public double GetDeterminant()
        {
            // Checks if not square
            if (Validator.IsMatrixSquare(this) is false) throw new ArgumentException("Matrix must be square");

            // Checks if 2x2
            if (ColumnSize == 2 && RowSize == 2)
            {
                var diagonal1 = GetItem(0, 0) * GetItem(1, 1);
                var diagonal2 = GetItem(0, 1) * GetItem(1, 0);
                return diagonal1 - diagonal2;
            }

            double determinant = 0;

            // Getting zeros count
            var (zerosInColumn, rowWithMostZerosIndex) = GetColumnsWithMostZeros();
            var (zerosInRow, columnWithMostZerosIndex) = GetRowWithMostZeros();

            for (var index = 0; index < ColumnSize; index++)
            {
                double selectedNumber, minusRatio;
                Matrix minorMatrix;

                if (zerosInRow > zerosInColumn)
                {
                    selectedNumber = GetItem(columnWithMostZerosIndex, index);
                    minusRatio = Math.Pow(-1, index + 1 + columnWithMostZerosIndex + 1);
                    minorMatrix = GetMinorMatrix(columnWithMostZerosIndex, index);
                }
                else
                {
                    selectedNumber = GetItem(index, rowWithMostZerosIndex);
                    minusRatio = Math.Pow(-1, index + 1 + rowWithMostZerosIndex + 1);
                    minorMatrix = GetMinorMatrix(index, rowWithMostZerosIndex);
                }

                determinant += selectedNumber * minusRatio * minorMatrix.GetDeterminant();
            }

            return determinant;
        }

        public void SolveByGaussianElimination()
        {
            for (int rowIndex = 0; rowIndex < RowSize; rowIndex++)
            {
                double rowItem = GetItem(rowIndex, rowIndex);
                
                for (int columnIndex = rowIndex + 1; columnIndex < ColumnSize; columnIndex++)
                {
                    double columnItem = GetItem(columnIndex, rowIndex);
                    
                    double ratio = columnItem / rowItem;
                    SubtractRowFromRow(columnIndex, rowIndex, ratio);
                }

                if (rowItem != 0)
                {
                    MultiplyRowByNumber(rowIndex, 1 / rowItem);
                }
            }

        }

        private void SetNegativeZeroToPositiveZeros()
        {
            _matrix = _matrix.Select(row => row.Select(item => item == -0 ? 0 : item).ToArray()).ToArray();
        }

        /// <summary>
        /// Multiplies row by given number
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="number"></param>
        private void MultiplyRowByNumber(int columnIndex, double number)
        {
            _matrix[columnIndex] = _matrix[columnIndex].Select(item => Math.Round(item * number, 6)).ToArray();
        }

        /// <summary>
        /// Subtracts one row from another
        /// </summary>
        /// <param name="columnThatChangesIndex"></param>
        /// <param name="columnIndex"></param>
        private void SubtractRowFromRow(int columnThatChangesIndex, int columnIndex, double ratio)
        {
            for (int rowIndex = 0; rowIndex < RowSize; rowIndex++)
            {
                _matrix[columnThatChangesIndex][rowIndex] = Math.Round(
                    _matrix[columnThatChangesIndex][rowIndex] - _matrix[columnIndex][rowIndex] * ratio, 6);
            }
        }

        /// <summary>
        ///     Getting minor matrix
        /// </summary>
        /// <param name="columnRemoveIndex">Column that should be removed</param>
        /// <param name="rowRemoveIndex">Row that should be removed</param>
        /// <returns>Minor matrix</returns>
        private Matrix GetMinorMatrix(int columnRemoveIndex, int rowRemoveIndex)
        {
            // Init empty minor matrix
            var minorMatrixData = new List<double[]>();

            for (var columnIndex = 0; columnIndex < ColumnSize; columnIndex++)
            {
                // Check if row should be removed
                if (columnIndex == columnRemoveIndex) continue;

                var row = new List<double>();

                for (var rowIndex = 0; rowIndex < RowSize; rowIndex++)
                {
                    // Check if column should be removed
                    if (rowIndex == rowRemoveIndex) continue;

                    // Add item to row
                    row.Add(GetItem(columnIndex, rowIndex));
                }

                // Add row to matrix
                minorMatrixData.Add(row.ToArray());
            }

            // Create matrix with given params
            var minorMatrix = new Matrix(minorMatrixData.ToArray());
            return minorMatrix;
        }

        /// <summary>
        ///     Getting row with most zeros
        /// </summary>
        /// <returns>Zeros count and row index</returns>
        private (int, int) GetColumnsWithMostZeros()
        {
            // Counter array
            int[] maxZerosCount = {0, 0};

            for (var rowIndex = 0; rowIndex < RowSize; rowIndex++)
            {
                var column = new double[ColumnSize];

                // Getting column
                for (var columnIndex = 0; columnIndex < ColumnSize; columnIndex++) column[columnIndex] = _matrix[columnIndex][rowIndex];

                // Count zeros in current column
                var zerosInColumn = column.Count(item => item == 0);

                // Update counter if more that max
                if (zerosInColumn > maxZerosCount[0])
                {
                    maxZerosCount[0] = zerosInColumn;
                    maxZerosCount[1] = rowIndex;
                }
            }

            return (maxZerosCount[0], maxZerosCount[1]);
        }

        /// <summary>
        ///     Getting column with most zeros
        /// </summary>
        /// <returns>Zeros count and column index</returns>
        private (int, int) GetRowWithMostZeros()
        {
            // Counter array
            int[] maxZerosCount = {0, 0};

            for (var columnIndex = 0; columnIndex < ColumnSize; columnIndex++)
            {
                // Count zeros in current row
                var zerosInRowCount = _matrix[columnIndex].Count(item => item == 0);

                // Update counter if more that max
                if (zerosInRowCount > maxZerosCount[0])
                {
                    maxZerosCount[0] = zerosInRowCount;
                    maxZerosCount[1] = columnIndex;
                }
            }

            return (maxZerosCount[0], maxZerosCount[1]);
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