using System;
using System.Linq;

namespace src
{
    public class Game
    {
        private int[] _answer;

        /// <summary>
        /// Method prints game title
        /// </summary>
        private static void PrintTitle()
        {
            try
            {
                FileReader.PrintTextFromFile(@"ASCII.txt");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Method starts a game
        /// </summary>
        public void Start()
        {
            // Show rules to user
            PrintRules();

            while (true)
            {
                // Clearing console
                ClearConsole();
                Console.WriteLine();

                // Getting answer length from user
                int answerLength = GetUserAnswerLength();

                // Generating random answer
                int[] randomAnswer = CreateRandomAnswer(answerLength);
                _answer = randomAnswer;

                // Clearing console
                ClearConsole();

                Console.WriteLine($"\t\tЯ загадал число длиной {answerLength}\n\n");
                DoGuessingLoop(); // Main game loop

                Console.WriteLine($"\t\tТы отгадал мое число!\n\n");

                // Ask player if wants to play again
                bool playAgain = AskForOneMoreGame();
                if (!playAgain)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Method asks users if wants to play again
        /// </summary>
        /// <returns>User's answer</returns>
        private static bool AskForOneMoreGame()
        {
            Console.WriteLine("\t\tНажми любую клавишу чтобы сыграть еще раз");
            Console.WriteLine("\t\tЧтобы выйти нажми ESC");

            var answer = Console.ReadKey();
            return answer.Key != ConsoleKey.Escape;
        }

        /// <summary>
        /// Method loop until user guesses correct
        /// </summary>
        private void DoGuessingLoop()
        {
            while (true)
            {
                int[] prediction = GetUserPrediction();

                int cows = GetCows(prediction);
                int bulls = GetBulls(prediction);

                if (bulls == _answer.Length)
                {
                    break;
                }

                Console.WriteLine($"\t\tКоров: {cows}");
                Console.WriteLine($"\t\tБыков: {bulls}\n\n");
            }
        }

        /// <summary>
        /// Method count how many cows given array has
        /// </summary>
        /// <param name="prediction"></param>
        /// <returns>Cows count</returns>
        private int GetCows(int[] prediction)
        {
            int bulls = GetBulls(prediction);
            return prediction.Where(number => _answer.Contains(number)).ToArray().Length - bulls;
        }

        /// <summary>
        /// Method count how many bulls given array has
        /// </summary>
        /// <param name="prediction"></param>
        /// <returns>Bulls count</returns>
        private int GetBulls(int[] prediction)
        {
            return Enumerable.Range(0, _answer.Length).Select(i => _answer[i] == prediction[i] ? 1 : 0).Sum();
        }

        /// <summary>
        /// Method clears console and prints title
        /// </summary>
        private static void ClearConsole()
        {
            Console.Clear();
            PrintTitle();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        /// <summary>
        /// Method asks user to type a number that will stand for prediction
        /// </summary>
        /// <returns>Prediction</returns>
        private int[] GetUserPrediction()
        {
            while (true)
            {
                ConsoleExtensions.PrintRenewableInputRequest("\t\t- Напиши свое предположение: ", placeholderLength: _answer.Length);

                // Getting User Prediction
                var userPredictionInput = Console.ReadLine();
                try
                {
                    var userPrediction = long.Parse(userPredictionInput);

                    // Does user typed negative number
                    if (userPrediction < 0)
                    {
                        continue;
                    }
                }
                catch (Exception)
                {
                    // Try again if userPredictionInput was not an integer
                    continue;
                }

                // Converting long to int[]
                var prediction = Array.ConvertAll(userPredictionInput.ToArray(), number => number - 48);

                // Validate userPrediction
                try
                {
                    ValidatePrediction(prediction);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                return prediction;
            }
        }

        /// <summary>
        /// Method asks user to type a number that will stand for answer length
        /// </summary>
        /// <returns>Answer length</returns>
        private static int GetUserAnswerLength()
        {
            while (true)
            {
                ConsoleExtensions.PrintRenewableInputRequest("\t\t- Напиши длину загадываемого числа: ");

                // Getting User Answer Length
                var userAnswerLengthInput = Console.ReadLine();
                var parseResult = int.TryParse(userAnswerLengthInput, out int userAnswerLength);
                if (!parseResult)
                {
                    // Try again if userAnswerLengthInput was not an integer
                    continue;
                }

                // Validate userAnswerLength
                try
                {
                    ValidateAnswerLength(userAnswerLength);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                return userAnswerLength;
            }
        }

        /// <summary>
        /// Method validates if answerLength is in valid range
        /// </summary>
        /// <param name="answerLength"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void ValidateAnswerLength(int answerLength)
        {
            if (answerLength is < 1 or > 10)
            {
                throw new ArgumentException("\t\tДлина числа должна быть в пределе [1...10]\n\n");
            }
        }

        /// <summary>
        /// Method validates if prediction is in valid range and unique
        /// </summary>
        /// <param name="prediction"></param>
        /// <exception cref="ArgumentException"></exception>
        private void ValidatePrediction(int[] prediction)
        {
            if (prediction.Length != _answer.Length)
            {
                throw new ArgumentException($"\t\tДлина числа должна быть равна {_answer.Length}\n\n");
            }

            if (prediction.Distinct().ToArray().Length != _answer.Length)
            {
                throw new ArgumentException("\t\tВсе цифры должны быть уникальные (не должны повторяться)\n\n");
            }
        }

        /// <summary>
        /// Method creates random answer with given length
        /// </summary>
        /// <param name="answerLength"></param>
        /// <returns>Random answer</returns>
        /// <exception cref="ArgumentException"></exception>
        private static int[] CreateRandomAnswer(int answerLength = 4)
        {
            // Validate answerLength
            ValidateAnswerLength(answerLength);


            // Creating random answer
            int[] availableNumbers = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            MixArray(availableNumbers); // Mixing numbers in array
            return availableNumbers.Take(answerLength).ToArray(); // Getting only first {answerLength} numbers of array
        }

        /// <summary>
        /// Method mixes items in giver array
        /// </summary>
        /// <param name="array"></param>
        private static void MixArray(int[] array)
        {
            var random = new Random();
            for (var index = 0; index < array.Length; index++)
            {
                int randomIndex = random.Next(array.Length);
                (array[randomIndex], array[index]) = (array[index], array[randomIndex]);
            }
        }

        /// <summary>
        /// Prints rules to console
        /// </summary>
        private static void PrintRules()
        {
            // Clearing console
            ClearConsole();

            // Printing rules
            try
            {
                FileReader.PrintTextFromFile(@"rules.txt");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            // Waiting till user presses anything
            Console.WriteLine("\t\tНажми любую клавишу чтобы начать");
            Console.ReadKey();
        }
    }
}