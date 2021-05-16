using System;

namespace Task2
{
    public class NumberParser : INumberParser
    {
        public int Parse(string stringValue)
        {
            CheckStringValidation(stringValue);

            string absStringValue = GetAbsOfNumber(stringValue, out bool isPositiveNumber);

            int[] numberArray = GetNumberArray(absStringValue);

            long numberLong = ConvertArrayToNumber(numberArray);

            CheckOverflow(numberLong, isPositiveNumber);

            if (isPositiveNumber)
            {
                return (int)numberLong;
            }

            return -(int)numberLong;
        }

        private void CheckStringValidation(string stringValue)
        {
            switch (stringValue)
            {
                case null:
                    throw new ArgumentNullException(nameof(stringValue));
                case "":
                    throw new FormatException(nameof(stringValue));
                case "  ":
                    throw new FormatException(nameof(stringValue));
            }
        }

        private void CheckOverflow(long numberLong, bool isPositiveNumber)
        {
            if (isPositiveNumber && numberLong - int.MaxValue > 0)
            {
                throw new OverflowException();
            }

            if (!isPositiveNumber && int.MinValue + numberLong > 0)
            {
                throw new OverflowException();
            }
        }

        private static string GetAbsOfNumber(string stringValue, out bool isPositiveNumber)
        {
            isPositiveNumber = true;

            if (stringValue.Contains("-") || stringValue.Contains("+"))
            {
                if (stringValue[0] == '-')
                {
                    isPositiveNumber = false;
                }

                return stringValue.Remove(0, 1).Trim();
            }
            else
            {
                return stringValue.Trim();
            }
        }

        private static int[] GetNumberArray(string absStringValue)
        {
            int[] numberArray = new int[absStringValue.Length];
            for (int i = 0; i < absStringValue.Length; i++)
            {
                numberArray[i] = absStringValue[i] switch
                {
                    '0' => 0,
                    '1' => 1,
                    '2' => 2,
                    '3' => 3,
                    '4' => 4,
                    '5' => 5,
                    '6' => 6,
                    '7' => 7,
                    '8' => 8,
                    '9' => 9,
                    _ => throw new FormatException(nameof(absStringValue)),
                };
            }

            return numberArray;
        }

        private static long ConvertArrayToNumber(int[] numberArray)
        {
            long numberLong = 0;

            long pow = 1;
            for (int i = numberArray.Length - 1; i >= 0; i--)
            {
                numberLong += numberArray[i] * pow;
                pow *= 10;
            }

            return numberLong;
        }
    }
}