using System;
using static MatrixCalc.src.Settings;

namespace MatrixCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            // Array.ForEach(System.IO.File.ReadAllText(@"lol.txt").Split(", "), Console.WriteLine);
            // Console.ReadKey();
            
            // MatrixCreator.CreateUserMatrix();
            Console.WriteLine(MatrixCreator.CreateRandomMatrix(3, 3));
        }
    }
}