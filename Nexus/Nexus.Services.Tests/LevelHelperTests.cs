
using Nexus.Data.Services.Core.Helpers;

namespace Nexus.Services.Tests
{
    [TestFixture]
    public class LevelHelperTests
    {
        //Handling all edge and mid cases for all tests
        [TestCase(0, ExpectedResult = 1)]
        [TestCase(99, ExpectedResult = 1)]
        [TestCase(100, ExpectedResult = 2)]
        [TestCase(250, ExpectedResult = 3)]
        [TestCase(999, ExpectedResult = 10)]
        public int GetLevel_UnderDifferentCases_ReturnsCorrectLevel(int xp)
        {
            return LevelHelper.GetLevel(xp);
        }

        [TestCase(0, ExpectedResult = 0)]
        [TestCase(99, ExpectedResult = 99)]
        [TestCase(100, ExpectedResult = 0)]
        [TestCase(250, ExpectedResult = 50)]
        [TestCase(199, ExpectedResult = 99)]
        public int GetXpIntoCurrentLevel_UnderDifferentCases_ReturnsCorrectValue(int xp)
        {
            return LevelHelper.GetXpIntoCurrentLevel(xp);
        }

        [Test]
        public void GetXpNeededPerLevel_AlwaysReturns100()
        {
            Assert.AreEqual(100, LevelHelper.GetXpNeededPerLevel());
        }

        [TestCase(0, ExpectedResult = 100)]
        [TestCase(99, ExpectedResult = 1)]
        [TestCase(100, ExpectedResult = 100)]
        [TestCase(250, ExpectedResult = 50)]
        [TestCase(199, ExpectedResult = 1)]
        public int GetXpNeededToNextLevel_UnderDifferentCases_ReturnsCorrectValue(int xp)
        {
            return LevelHelper.GetXpNeededToNextLevel(xp);
        }

        [TestCase(0, ExpectedResult = 0)]
        [TestCase(99, ExpectedResult = 99)]
        [TestCase(100, ExpectedResult = 0)]
        [TestCase(250, ExpectedResult = 50)]
        [TestCase(199, ExpectedResult = 99)]
        public int GetProgressPercentage_UnderDifferentCases_ReturnsCorrectValue(int xp)
        {
            return LevelHelper.GetProgressPercentage(xp);
        }
    }
}
