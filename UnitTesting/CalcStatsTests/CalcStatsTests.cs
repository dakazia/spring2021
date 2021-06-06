using System;
using NUnit.Framework;
using CalcStatsKata;

namespace CalcStatsTests
{
    public class CalcStatsTests
    {
        [TestCase(arg: new int[] { 12, 2, 150, -2, -92, 151 }, ExpectedResult = -92)]
        [TestCase(arg: new int[] { 50, 105, 23, 6 }, ExpectedResult = 6)]
        [TestCase(arg: new int[] { -2147483648, 4, 2147483647, 10, 9, 0, 101 }, ExpectedResult = -2147483648)]
        public int GetMinimumValue_WithValidInt(int[] sequence)
        {
            return CalcStats.GetMinimumValue(sequence);
        }

        [TestCase(arg: new int[] { 12, 2, 150, -2, -92, 151 }, ExpectedResult = 92)]
        [TestCase(arg: new int[] { 50, 105, 23, 6 }, ExpectedResult = 123)]
        [TestCase(arg: new int[] { -2147483648, 4, 2147483647, 10, 9, 0, 101 }, ExpectedResult = 2147483647)]
        public int GetMaximumValue_WithValidInt(int[] sequence)
        {
            return CalcStats.GetMaximumValue(sequence);
        }

        [TestCase(arg: new int[] { }, ExpectedResult = 0)]
        [TestCase(arg: new int[] { 0 }, ExpectedResult = 1)]
        [TestCase(arg: new int[] { 12, 2, 150, -2, -92, 151 }, ExpectedResult = 6)]
        [TestCase(arg: new int[] { 50, 105, 23, 6 }, ExpectedResult = 4)]
        public int Get_WithValidInt(int[] sequence)
        {
            return CalcStats.GetLengthOfSequence(sequence);
        }

        [TestCase(new int[] { 12, 2, 150, -2, -92, 151 },  75.78d)]
        [TestCase(new int[] { 50, 105, 23, 6 }, 30.75d)]
        [TestCase(new int[] { -2147483648, 4, 2147483647, 10, 9, 0, 101 }, 40.75d)]
        public void GetAverageOfSequence_WithValidInt(int[] sequence, double expectedAverage)
        {
            var delta = 0.01d;
            Assert.AreEqual(expectedAverage, CalcStats.GetAverageOfSequence(sequence), delta);
        }

        [Test]
        public void IsSequenceValid_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => CalcStats.IsSequenceValid(null));
        }
    }
}