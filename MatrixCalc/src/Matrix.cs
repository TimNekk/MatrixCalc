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
        private double GetItem(int columnIndex, int rowIndex, bool round = false)
        {
            var item = _matrix[columnIndex][rowIndex];

            if (round) item = Math.Round(item, 3);

            return item;
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
                    var item = GetItem(columnIndex, rowIndex, true);

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
        public string ToStringWithReplacedCurrentSymbol(bool useSOLE = false)
        {
            // Getting matrix as string
            var matrix = useSOLE ? ToStringAsSOLE() : ToString();

            // Getting string that will be replaced
            var stringToReplace = double.PositiveInfinity.ToString();

            // Getting first empty cell index
            var firstEmptyCellIndex = matrix.IndexOf(stringToReplace);

            // Replacing string
            matrix = matrix.Remove(firstEmptyCellIndex, stringToReplace.Length).Insert(firstEmptyCellIndex, "_");

            return matrix;
        }

        /// <summary>
        ///     Gets matrix as SOLE
        /// </summary>
        /// <returns>SOLE</returns>
        public string ToStringAsSOLE(bool showFreeVariables=false)
        {
            // Remove negative Zeros
            SetNegativeZeroToPositiveZeros();

            var matrix = "";

            var rowLimit = RowSize > ColumnSize ? ColumnSize : RowSize - 1;

            
            for (var columnIndex = 0; columnIndex < rowLimit; columnIndex++)
            {
                if (_matrix[columnIndex].Sum() == 0) continue;

                if (_matrix.Select(row => row.Take(RowSize - 1).All(item => item == 0)).Any(x => x)) throw new ArgumentException("СЛАУ не имеет решения!");
                
                string row = " │" + string.Join("", _matrix[columnIndex].Take(RowSize - 1)
                           .Select((item, rowIndex) =>
                               item != 0 ? (item > 0 ? " +" : " ") +
                                     (item != 1 ? $"{Math.Round(item, 3)}" : item < 0 ? "-" : "") +
                                     (char) (rowIndex + 97)
                                   : "")) +
                       $" = {GetItem(columnIndex, RowSize - 1, true)} {new string(' ', RowSize)}\n";


                if (row[3] == '+') row = row.Remove(3, 1);

                matrix += row;
            }

            if (showFreeVariables && RowSize > ColumnSize)
            {
                for (var columnIndex = rowLimit; columnIndex < RowSize - 1; columnIndex++)
                {
                    matrix += $" │ {(char) (columnIndex + 97)} - свободная переменная\n";
                }
            }

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
                item = Math.Round(item * number, 10);

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
                item1 = difference ? Math.Round(item1 - item2, 10) : Math.Round(item1 + item2, 10);

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
                GetItem(columnIndex, rowIndex, true).ToString()).OrderByDescending(s => s.Length).First().Length).ToArray();

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

        /// <summary>
        /// Getting new matrix that is solved by Gaussian Elimination
        /// </summary>
        /// <returns>Solved matrix</returns>
        public Matrix SolveByGaussianElimination()
        {
            // Create matrix clone
            var matrixClone = new Matrix(_matrix);

            matrixClone.ConvertToReducedRowEchelonMatrix();

            return matrixClone;
        }

        /// <summary>
        /// Getting new matrix that is solved by Cramer's Rule
        /// </summary>
        /// <returns>Solved matrix</returns>
        /// <exception cref="ArgumentException"></exception>
        public Matrix SolveByCramersRule()
        {
            // Init empty array for answers
            var answersMatrix = MatrixCreator.CreateEmptyMatrix(ColumnSize, RowSize, true);

            // Get determinant of matrix without last column
            var matrixWithoutLastColumn = new Matrix(_matrix);
            matrixWithoutLastColumn.RemoveColumn(RowSize - 1);
            var matrixDeterminant = matrixWithoutLastColumn.GetDeterminant();

            // Check if can be solved
            if (matrixDeterminant == 0) throw new ArgumentException("Невозможно решить методом Крамера");

            for (var rowIndex = 0; rowIndex < matrixWithoutLastColumn.RowSize; rowIndex++)
            {
                // Create matrix with swapped columns
                var swappedMatrix = new Matrix(_matrix);
                swappedMatrix.SwapColumns(rowIndex, RowSize - 1);
                swappedMatrix.RemoveColumn(RowSize - 1);

                // Get determinant of this matrix
                var swappedMatrixDeterminant = swappedMatrix.GetDeterminant();

                // Calculate answer
                var answer = Math.Round(swappedMatrixDeterminant / matrixDeterminant, 10);

                // Save answer to answerMatrix
                answersMatrix.SetItem(1, rowIndex, rowIndex);
                answersMatrix.SetItem(answer, rowIndex, RowSize - 1);
            }

            return answersMatrix;
        }

        /// <summary>
        ///     Removes columns from matrix
        /// </summary>
        /// <param name="columnIndex"></param>
        private void RemoveColumn(int columnIndex)
        {
            _matrix = _matrix.Select(row => row[..columnIndex].Concat(row[(columnIndex + 1)..]).ToArray()).ToArray();
        }

        /// <summary>
        ///     Converts Matrix to Row Echelon Form
        /// </summary>
        private void ConvertToRowEchelonMatrix()
        {
            if (RowSize < 3) throw new ArgumentException("Нужно минимум 2 переменные");

            // Swap if 1st element is zero
            if (GetItem(0, 0) == 0) SwapRows(0, ColumnSize - 1);

            // Gets the row limit
            var rowLimit = RowSize > ColumnSize ? ColumnSize : RowSize - 1;

            for (var rowIndex = 0; rowIndex < rowLimit; rowIndex++)
            {
                // Gets the row item
                var rowItem = GetItem(rowIndex, rowIndex);

                for (var columnIndex = rowIndex + 1; columnIndex < ColumnSize; columnIndex++)
                {
                    // Gets the column item
                    var columnItem = GetItem(columnIndex, rowIndex);

                    // Gets the ratio for multiplication
                    var ratio = columnItem / rowItem;

                    // Subtracts rows
                    SubtractRowFromRow(columnIndex, rowIndex, ratio);
                }

                // Sets leader of row to 1
                if (rowItem != 0) MultiplyRowByNumber(rowIndex, 1 / rowItem);
            }
        }

        /// <summary>
        ///     Converts Matrix to Reduced Row Echelon Form
        /// </summary>
        private void ConvertToReducedRowEchelonMatrix()
        {
            ConvertToRowEchelonMatrix();
            
            // Gets the row limit
            var columnLimit = RowSize > ColumnSize ? ColumnSize : RowSize - 1;

            for (var columnIndex = 0; columnIndex < columnLimit - 1; columnIndex++)
            {
                if (_matrix[columnIndex].Take(RowSize - 1).Sum() == 0) continue;
                
                int rowLimit = RowSize > ColumnSize + 1 ? ColumnSize : RowSize - 1;
                
                for (var rowIndex = columnIndex + 1; rowIndex < rowLimit; rowIndex++)
                {
                    // Gets the row item
                    var rowItem = GetItem(columnIndex, rowIndex);

                    // Gets the column item
                    var columnItem = GetItem(rowIndex, rowIndex);

                    // Gets the ratio for multiplication
                    var ratio = rowItem / columnItem;

                    // Subtracts rows
                    SubtractRowFromRow(columnIndex, rowIndex, ratio);
                }
            }
        }

        /// <summary>
        ///     Turns every negative zero to positive zero
        /// </summary>
        private void SetNegativeZeroToPositiveZeros()
        {
            _matrix = _matrix.Select(row => row.Select(item => item == -0 ? 0 : item).ToArray()).ToArray();
        }

        /// <summary>
        ///     Multiplies row by given number
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="number"></param>
        private void MultiplyRowByNumber(int columnIndex, double number)
        {
            _matrix[columnIndex] = _matrix[columnIndex].Select(item => Math.Round(item * number, 10)).ToArray();
        }

        /// <summary>
        ///     Subtracts one row from another
        /// </summary>
        /// <param name="columnThatChangesIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="ratio"></param>
        private void SubtractRowFromRow(int columnThatChangesIndex, int columnIndex, double ratio)
        {
            for (var rowIndex = 0; rowIndex < RowSize; rowIndex++)
                _matrix[columnThatChangesIndex][rowIndex] = Math.Round(
                    _matrix[columnThatChangesIndex][rowIndex] - _matrix[columnIndex][rowIndex] * ratio, 10);
        }

        /// <summary>
        ///     Swaps 2 rows
        /// </summary>
        /// <param name="rowIndex1"></param>
        /// <param name="rowIndex2"></param>
        private void SwapRows(int rowIndex1, int rowIndex2)
        {
            (_matrix[rowIndex1], _matrix[rowIndex2]) = (_matrix[rowIndex2], _matrix[rowIndex1]);
        }

        /// <summary>
        ///     Swaps 2 column
        /// </summary>
        /// <param name="columnIndex1"></param>
        /// <param name="columnIndex2"></param>
        private void SwapColumns(int columnIndex1, int columnIndex2)
        {
            Transpose();
            SwapRows(columnIndex1, columnIndex2);
            Transpose();
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