namespace MatrixCalc
{
    public static class Validator
    {
        public static bool IsNumber(char numberForCheck, bool includeZero=true)
        {
            bool result = int.TryParse(numberForCheck.ToString(), out int number);
            return result && (number != 0 || includeZero);
        }
    }
}