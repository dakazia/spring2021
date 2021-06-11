using System;
using System.Collections.Generic;

namespace FizzBuzzKata
{
    public class FizzBuzz
    {
        private const int FizzNumber = 3;
        private const int BuzzNumber = 5;

        public static void PrintFizzBuzz(int firstNumber, int lastNumber)
        {
            var result = new List<string>();

            if (IsFirstNumberValid(firstNumber) && IsLastNumberValid(lastNumber))
            {
                for (int i = firstNumber; i <= lastNumber; i++)
                {
                    result.Add(IsNumberMultiplesTreeOrFiveOrBoth(i));
                }
            }

            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        public static string IsNumberMultiplesTreeOrFiveOrBoth(int number)
        {
            if (number % FizzNumber == 0 && number % BuzzNumber == 0)
                return "FizzBuzz";
            if (number % FizzNumber == 0)
                return "Fizz";
            if (number % BuzzNumber == 0)
                return "Buzz";
            return number.ToString();
        }

        public static bool IsFirstNumberValid(int firstNumber)
        {
            if (firstNumber != 1)
            {
                return false;
            }

            return true;
        }

        public static bool IsLastNumberValid(int lastNumber)
        {
            if (lastNumber != 100)
            {
                return false;
            }

            return true;
        }
    }
}