using System;

namespace Task2
{
    public class NumberParser : INumberParser
    {
        public int Parse(string stringValue)
        {
            if (stringValue is null)
            {
                throw new ArgumentNullException(nameof(stringValue));
            }

            if (stringValue == string.Empty)
            {
                throw new FormatException(nameof(stringValue));
            }

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                throw new FormatException(nameof(stringValue));
            }

            string absStringValue;
            bool isPositiveNumber = true;

            if (stringValue.Contains("-") || stringValue.Contains("+"))
            {
                if (stringValue[0] == '-')
                {
                    isPositiveNumber = false;
                }

                absStringValue = stringValue.Remove(0, 1).Trim();
            }
            else
            {
                absStringValue = stringValue.Trim();
            }

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
                    _ => throw new FormatException(nameof(stringValue)),

                };
            }

            long numberLong = 0;

            long pow = 1;
            for (int i = numberArray.Length - 1; i >= 0; i--)
            {
                numberLong += numberArray[i] * pow;
                pow *= 10;
            }

            long test = numberLong;

            if (isPositiveNumber && numberLong - int.MaxValue > 0)
            {
                throw new OverflowException();
            }

            if (!isPositiveNumber && int.MinValue + numberLong > 0)
            {
                throw new OverflowException();
            }

            if (isPositiveNumber)
            {
                return (int) numberLong;
            }

            return -(int)numberLong;
        }
    }
}