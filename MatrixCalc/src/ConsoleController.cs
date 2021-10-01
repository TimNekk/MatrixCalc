using System;

namespace MatrixCalc
{
    public class ConsoleController
    {
        public static ConsoleKeyInfo AskUserOneCharWithOffset(string askLine, int offsetLeft=0)
        {
            // Move cursor to the beginning of the current line
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
            
            // Write the ask
            Console.Write(askLine + new string(' ', 20));
            
            // Offset cursor
            Console.SetCursorPosition(askLine.Length - offsetLeft, Console.GetCursorPosition().Top);

            ConsoleKeyInfo inputKey = Console.ReadKey();
            return inputKey;
        }
        
        public static ConsoleKeyInfo AskUserOneCharToPosition(string askLine, int x, int y)
        {
            // Move cursor to the beginning of the current line
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
            
            // Write the ask
            Console.Write(askLine);
            
            // Offset cursor
            Console.SetCursorPosition(x, y);

            ConsoleKeyInfo inputKey = Console.ReadKey();
            return inputKey;
        }
    }
}