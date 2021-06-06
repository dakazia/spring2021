using NUnit.Framework;
using FizzBuzzKata;

namespace FizzBuzzTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Category("Successful tests.")]
        [TestCase(0, ExpectedResult = "0")]
        [TestCase(3, ExpectedResult = "Fizz")]
        [TestCase(5, ExpectedResult = "Buzz")]
        [TestCase(15, ExpectedResult = "FizzBuzz")]
        [TestCase(30, ExpectedResult = "FizzBuzz")]
        [TestCase(33, ExpectedResult = "Fizz")]
        [TestCase(28, ExpectedResult = "28")]
        [TestCase(100, ExpectedResult = "Buzz")]
        public string IsNumber_Multiples_TreeOrFiveOrBoth(int number)
        {
            return CheckNumber(number);
        }
    }
}