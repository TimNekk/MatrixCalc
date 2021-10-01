using System;

namespace src
{
    public static class ConsoleExtensions
    {
        /// <summary>
        /// Method prints given string on the previous line and sets cursor to start position
        /// </summary>
        /// <param name="request"></param>
        /// <param name="placeholderLength"></param>
        public static void PrintRenewableInputRequest(string request, int placeholderLength = 0)
        {
            request = ReplaceTabsWithSpaces(request);
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(request + new string('_', placeholderLength) + new string(' ', 50));
            Console.SetCursorPosition(request.Length, Console.CursorTop);
        }

        /// <summary>
        /// Method replaces tabs of given string with spaces
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Replaced string</returns>
        private static string ReplaceTabsWithSpaces(string text)
        {
            return text.Replace("\t", new string(' ', 8));
        }
    }
}