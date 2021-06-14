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
        public int WhenGetMinimumValue_ReturnValidInt(int[] sequence)
        {
            return CalcStats.GetMinimumValue(sequence);
        }

        [TestCase(arg: new int[] { 12, 2, 150, -2, -92, 151 }, ExpectedResult = 151)]
        [TestCase(arg: new int[] { 50, 105, 23, 6 }, ExpectedResult = 105)]
        [TestCase(arg: new int[] { -2147483648, 4, 2147483647, 10, 9, 0, 101 }, ExpectedResult = 2147483647)]
        public int WhenGetMaximumValue_ReturnValidInt(int[] sequence)
        {
            return CalcStats.GetMaximumValue(sequence);
        }

        [TestCase(arg: new int[] { }, ExpectedResult = 0)]
        [TestCase(arg: new int[] { 0 }, ExpectedResult = 1)]
        [TestCase(arg: new int[] { 12, 2, 150, -2, -92, 151 }, ExpectedResult = 6)]
        [TestCase(arg: new int[] { 50, 105, 23, 6 }, ExpectedResult = 4)]
        public int WhenGetLengthOfSequence_ReturnValidInt(int[] sequence)
        {
            return CalcStats.GetLengthOfSequence(sequence);
        }

        [TestCase(new int[] { 12, 2, 150, -2, -92, 151 }, 36.83d)]
        [TestCase(new int[] { 50, 105, 23, 6 }, 46d)]
        [TestCase(new int[] { -2147483648, 4, 2147483647, 10, 9, 0, 101 }, 17.57d)]
        public void WhenGetAverageOfSequence_ReturnValidInt(int[] sequence, double expectedAverage)
        {
            var delta = 0.01d;
            Assert.AreEqual(expectedAverage, CalcStats.GetAverageOfSequence(sequence), delta);
        }

        [Test]
        public void WhenSequenceIsNull_ReturnThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => CalcStats.IsSequenceValid(null));
        }
    }
}