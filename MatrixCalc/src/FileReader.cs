using System;
using System.IO;
using System.Linq;

namespace src
{
    /// <summary>
    ///     Class that reads files
    /// </summary>
    public static class FileReader
    {
        /// <summary>
        ///     Method gets content of the given file
        /// </summary>
        /// <param name="filePath">Path to file to read</param>
        /// <returns>ASCII form file</returns>
        public static string GetTextFromFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new ArgumentException($"{filePath} file is missing.");

            return File.ReadAllText(filePath);
        }

        /// <summary>
        ///     Method prints content of the given file
        /// </summary>
        /// <param name="fileName">Path to file to print</param>
        public static void PrintTextFromFile(string fileName)
        {
            var text = GetTextFromFile(fileName);
            Console.WriteLine(text);
        }

        /// <summary>
        ///     Getting height of logo
        /// </summary>
        /// <returns>Height of logo</returns>
        public static int GetLogoHeight()
        {
            return GetTextFromFile("logo.txt").Split("\n").Length;
        }

        /// <summary>
        ///     Getting width of logo
        /// </summary>
        /// <returns>Width of logo</returns>
        public static int GetLogoWidth()
        {
            return GetTextFromFile("logo.txt").Split("\n").ToArray()[3].Length;
        }
    }
}