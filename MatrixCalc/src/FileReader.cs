using System;
using System.IO;

namespace src
{
    public static class FileReader
    {
        /// <summary>
        /// Method gets content of the given file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>ASCII form file</returns>
        private static string GetTextFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"{filePath} file is missing.");
            }

            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// Method prints content of the given file
        /// </summary>
        /// <param name="fileName"></param>
        public static void PrintTextFromFile(string fileName)
        {
            string text = GetTextFromFile(fileName);
            Console.WriteLine(text);
        }
    }
}