using System;
using System.Diagnostics;

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

        /// <summary>
        ///     Asking user for number with limitations
        /// </summary>
        /// <param name="ask">Line that will be printed</param>
        /// <param name="toRange">The maximum value of number</param>
        /// <returns>User number</returns>
        public static int AskUserForNumber(string ask, int toRange = int.MaxValue)
        {
            var currentInput = "";

            while (true)
            {
                var askLine = $"{ask}: {currentInput} "; // Line to ask
                var userInput = AskUserOneCharWithOffset(askLine, 1); // Asking for input

                if (Validator.IsNumber(userInput.KeyChar) &&
                    int.Parse(currentInput + userInput.KeyChar) >= 1 &&
                    int.Parse(currentInput + userInput.KeyChar) <= toRange) // If a valid number
                    // Adding key to currentInput
                    currentInput += userInput.KeyChar.ToString();
                else if (userInput.Key is ConsoleKey.Enter && currentInput != "") // If Tab or enter
                    break;
                else if (userInput.Key is ConsoleKey.Backspace && currentInput != "") // If backspace
                    currentInput = currentInput[..^1]; // Remove last char
                else if (userInput.Key == ConsoleKey.Escape)
                    Process.GetCurrentProcess().Kill();
            }

            return int.Parse(currentInput);
        }

        /// <summary>
        ///     Prints text with given colors
        /// </summary>
        /// <param name="whatToPrint">Object to print</param>
        /// <param name="foregroundColor">Color of foreground</param>
        /// <param name="backgroundColor">Color of background</param>
        public static void PrintWithColor(object whatToPrint, 
            ConsoleColor foregroundColor = ConsoleColor.White, 
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(whatToPrint);
            Console.ResetColor();
        }
    }
}