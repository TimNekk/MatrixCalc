namespace MatrixCalc
{
    /// <summary>
    ///     Class that contains different validators
    /// </summary>
    public static class Validator
    {
        /// <summary>
        ///     Checks if input is number
        /// </summary>
        /// <param name="numberForCheck">Item to check</param>
        /// <param name="includeZero">Include zero?</param>
        /// <returns>Validator</returns>
        public static bool IsNumber(char numberForCheck, bool includeZero = true)
        {
            // Tries to parse input
            var result = int.TryParse(numberForCheck.ToString(), out var number);

            return result && (number != 0 || includeZero);
        }

        /// <summary>
        ///     Checks if matrices have same size
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns>Checking result</returns>
        public static bool AreMatricesHaveEqualSize(Matrix matrix1, Matrix matrix2)
        {
            return matrix1.ColumnSize == matrix2.ColumnSize && matrix1.RowSize == matrix2.RowSize;
        }

        /// <summary>
        ///     Checks if matrices can be multiplied
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns>Checking result</returns>
        public static bool AreMatricesCanBeMultiplied(Matrix matrix1, Matrix matrix2)
        {
            return matrix1.RowSize == matrix2.ColumnSize;
        }

        /// <summary>
        ///     Checks if matrix is square
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Checking result</returns>
        public static bool IsMatrixSquare(Matrix matrix)
        {
            return matrix.ColumnSize == matrix.RowSize;
        }
    }
}