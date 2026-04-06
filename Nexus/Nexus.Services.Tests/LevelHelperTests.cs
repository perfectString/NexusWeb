using Nexus.Data.Services.Core.Helpers;

namespace Nexus.Services.Tests
{
    [TestFixture]
    public class LevelHelperTests
    {
        [TestCase(0, ExpectedResult = 1, Description = "Zero XP is level 1")]
        [TestCase(1, ExpectedResult = 1, Description = "Min non-zero XP is still level 1")]
        [TestCase(99, ExpectedResult = 1, Description = "Just below level-up threshold")]
        [TestCase(100, ExpectedResult = 2, Description = "Exact level-up boundary")]
        [TestCase(101, ExpectedResult = 2, Description = "Just above level-up boundary")]
        [TestCase(250, ExpectedResult = 3, Description = "Mid-range XP")]
        [TestCase(999, ExpectedResult = 10, Description = "High XP")]
        public int GetLevel_ReturnsCorrectLevel(int xp)
        {
            // Arrange 

            // Act & Assert
            return LevelHelper.GetLevel(xp);
        }


        [TestCase(0, ExpectedResult = 0, Description = "Zero XP means zero progress")]
        [TestCase(1, ExpectedResult = 1, Description = "Min non-zero progress")]
        [TestCase(99, ExpectedResult = 99, Description = "Just below level-up threshold")]
        [TestCase(100, ExpectedResult = 0, Description = "Exact boundary resets to zero")]
        [TestCase(101, ExpectedResult = 1, Description = "One XP into new level")]
        [TestCase(199, ExpectedResult = 99, Description = "Just below next boundary")]
        [TestCase(250, ExpectedResult = 50, Description = "Mid-range within level")]
        public int GetXpIntoCurrentLevel_ReturnsProgressWithinLevel(int xp)
        {
            // Arrange 

            // Act & Assert 
            return LevelHelper.GetXpIntoCurrentLevel(xp);
        }


        [Test]
        public void GetXpNeededPerLevel_AlwaysReturns100()
        {
            // Arrange 

            // Act
            var result = LevelHelper.GetXpNeededPerLevel();

            // Assert
            Assert.That(result, Is.EqualTo(100));
        }

        // GetXpNeededToNextLevel

        [TestCase(0, ExpectedResult = 100, Description = "Full level needed from zero")]
        [TestCase(1, ExpectedResult = 99, Description = "Almost full level remaining")]
        [TestCase(99, ExpectedResult = 1, Description = "One XP to next level")]
        [TestCase(100, ExpectedResult = 100, Description = "Exact boundary resets to full")]
        [TestCase(101, ExpectedResult = 99, Description = "Just past boundary")]
        [TestCase(199, ExpectedResult = 1, Description = "One XP to next level again")]
        [TestCase(250, ExpectedResult = 50, Description = "Mid-range remaining")]
        public int GetXpNeededToNextLevel_ReturnsRemainingXp(int xp)
        {

            // Act & Assert
            return LevelHelper.GetXpNeededToNextLevel(xp);
        }


        [TestCase(0, ExpectedResult = 0, Description = "Zero progress")]
        [TestCase(1, ExpectedResult = 1, Description = "Min non-zero progress")]
        [TestCase(50, ExpectedResult = 50, Description = "Halfway through level")]
        [TestCase(99, ExpectedResult = 99, Description = "Almost complete")]
        [TestCase(100, ExpectedResult = 0, Description = "Exact boundary resets to zero")]
        [TestCase(199, ExpectedResult = 99, Description = "Almost complete at higher level")]
        [TestCase(250, ExpectedResult = 50, Description = "Mid-range at higher level")]
        public int GetProgressPercentage_ReturnsPercentWithinLevel(int xp)
        {
            // Arrange 

            // Act & Assert
            return LevelHelper.GetProgressPercentage(xp);
        }
    }
}