using System.Linq;

namespace MatrixCalc
{
    public class Matrix
    {
        /// <summary>
        /// Matrix
        /// </summary>
        private double[][] _matrix;
        
        /// <summary>
        /// Initialize Matrix
        /// </summary>
        /// <param name="matrix"></param>
        public Matrix(double[][] matrix)
        {
            _matrix = matrix;
        }

        public override string ToString()
        {
            // Converting every row to joined string
            var matrixWithJoinedRows = _matrix.Select(row => string.Join("\t", row)).ToArray();

            // Lopping through every row 
            for (var rowIndex = 0; rowIndex < _matrix.Length; rowIndex++)
            {
                // Getting leading symbols based on row index
                var (leadingSymbol, endingSymbol) = GetLeadingAndEndingChar(rowIndex);

                // Wrapping row with symbols
                matrixWithJoinedRows[rowIndex] = leadingSymbol + matrixWithJoinedRows[rowIndex] + endingSymbol;
            }
                
            // Joining every row
            string matrixForOutput = string.Join("\n", matrixWithJoinedRows);
            
            return matrixForOutput;
        }

        /// <summary>
        /// Getting leading and ending symbols based on row index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private (string, string) GetLeadingAndEndingChar(int rowIndex)
        {
            string leadingSymbol;
            string endingSymbol;
                
            if (rowIndex == 0)  // First row
            {
                leadingSymbol = "┌\t";
                endingSymbol = "\t┐";
            }
            else if (rowIndex == _matrix.Length - 1)  // Last row
            {
                leadingSymbol = "└\t";
                endingSymbol = "\t┘";
            }
            else  // Every other row
            {
                leadingSymbol = "│\t";
                endingSymbol = "\t│";
            }
            
            return (leadingSymbol, endingSymbol);
        }
    }
}