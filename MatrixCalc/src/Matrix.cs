using System;
using System.Linq;

namespace MatrixCalc
{
    public class Matrix
    {
        /// <summary>
        /// Matrix
        /// </summary>
        private double[][] _matrix;

        public int columnSize => _matrix.Length;
        public int rowSize => _matrix[0].Length;

        /// <summary>
        /// Initialize Matrix
        /// </summary>
        /// <param name="matrix"></param>
        public Matrix(double[][] matrix)
        {
            _matrix = matrix;
        }

        public bool SetCell(double item, int columnIndex, int rowIndex)
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
        /// ToString overrider to beautify output
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Converting every row to joined string
            // var matrixWithJoinedRows = _matrix.Select(row => string.Join("\t", row)).ToArray();
            var matrix = "";
            int[] rowLengths = new int[_matrix[0].Length];

            // Lopping through every row 
            for (var columnIndex = 0; columnIndex < _matrix.Length; columnIndex++)
            {
                // Getting leading symbols based on column index
                var (leadingSymbol, endingSymbol) = GetLeadingAndEndingChar(columnIndex);
                
                string row = $"{leadingSymbol} ";
                for (var rowIndex = 0; rowIndex < _matrix[0].Length; rowIndex++)
                {
                    row += $"{_matrix[columnIndex][rowIndex]} ";
                    rowLengths[rowIndex] = Math.Max(row.Length, rowLengths[rowIndex]);
                    row += new string(' ', rowLengths[rowIndex] - row.Length);
                }

                row += $"{endingSymbol}\n";
                matrix += row;

                // Wrapping row with symbols
                // matrixWithJoinedRows[rowIndex] = leadingSymbol + '\t' + matrixWithJoinedRows[rowIndex] + new string('\t', maxValueInMatrix / 8 + 1) + endingSymbol;
            }

            return matrix;
        }

        public string ToStringWithZerosAsUnderscores()
        {
            string matrix = ToString();
            int firstEmptyCellIndex = matrix.IndexOf("-999999", StringComparison.Ordinal);
            matrix = matrix.Remove(firstEmptyCellIndex, 7).Insert(firstEmptyCellIndex, "_");
            return matrix.Replace("-999999", "■");
        }

        /// <summary>
        /// Getting leading and ending symbols based on row index
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private (string, string) GetLeadingAndEndingChar(int columnIndex)
        {
            string leadingSymbol;
            string endingSymbol;
                
            if (columnIndex == 0)  // First row
            {
                leadingSymbol = "┌";
                endingSymbol = "┐";
            }
            else if (columnIndex == _matrix.Length - 1)  // Last row
            {
                leadingSymbol = "└";
                endingSymbol = "┘";
            }
            else  // Every other row
            {
                leadingSymbol = "│";
                endingSymbol = "│";
            }
            
            return (leadingSymbol, endingSymbol);
        }
    }
}