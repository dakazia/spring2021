using System;

namespace Task1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            do
            {
                string userInput = EnterText();
                ShowFirstSymbol(userInput);

            } while (ContinueEnter());

            Console.ReadKey();
        }

        private static bool ContinueEnter()
        {
            string userSelect;

            do
            {
                Console.WriteLine("Repeat enter one more time? Press Yes: Y, No: N");
                userSelect = Console.ReadLine();
               
            } while (!ValidateInput(userSelect));

            if (userSelect.Equals("N"))
            {
                return false;
            }

            return true;
        }

        private static string EnterText()
        {
            bool correctLine;
            string userInput;

            do
            {
                Console.WriteLine("Please, enter a line:");
                userInput = Console.ReadLine();
                correctLine = ValidateInput(userInput);

            } while (!correctLine);

            return userInput;
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
                    throw new InputException(nameof(userInput));
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

