using System;

namespace MatrixCalc
{
    /// <summary>
    ///     Class that has special printing methods
    /// </summary>
    public class ConsoleController
    {
        /// <summary>
        ///     Getting user input with replacing old console text
        /// </summary>
        /// <param name="askLine">Line that will be printed</param>
        /// <param name="offsetLeft">Amount of symbols that will be skipped from right edge of line</param>
        /// <returns>User input keu</returns>
        public static ConsoleKeyInfo AskUserOneCharWithOffset(string askLine, int offsetLeft = 0)
        {
            // Move cursor to the beginning of the current line
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top);

            // Write the ask
            Console.Write(askLine + new string(' ', 20));

            // Offset cursor
            Console.SetCursorPosition(askLine.Length - offsetLeft, Console.GetCursorPosition().Top);

            // Getting user input key
            var inputKey = Console.ReadKey();
            return inputKey;
        }
    }
}