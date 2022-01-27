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