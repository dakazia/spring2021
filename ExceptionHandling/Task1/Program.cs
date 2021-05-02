using System;

namespace Task1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            bool correctLine;
            string userInput;

            do
            {
                Console.WriteLine($"Please, enter a line:");

                userInput = Console.ReadLine();
                correctLine = ValidateInput(userInput);

            } while (!correctLine);

            ShowFirstSymbol(userInput);
            Console.ReadKey();
        }

        private static void ShowFirstSymbol(string userInput)
        {
            Console.WriteLine($"First symbol is {userInput[0]}.");
        }

        private static bool ValidateInput(string userInput)
        {
            try
            {
                if (string.IsNullOrEmpty(userInput) || string.IsNullOrWhiteSpace(userInput))
                {
                    throw new ArgumentNullException(nameof(userInput));
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error! Entered string is empty. \n");
                Console.ResetColor();
                return false;
            }
           
            return true;
        }
    }
}

