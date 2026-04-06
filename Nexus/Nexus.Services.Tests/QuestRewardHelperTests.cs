using Nexus.Data.Services.Core.Helpers;
using Nexus.GCommon.Enums;

namespace Nexus.Services.Tests
{
    [TestFixture]
    public class QuestRewardHelperTests
    {
        [TestCase(QuestDifficulty.Easy, ExpectedResult = QuestRewardHelper.EasyQuestReward)]
        [TestCase(QuestDifficulty.Medium, ExpectedResult = QuestRewardHelper.MediumQuestReward)]
        [TestCase(QuestDifficulty.Hard, ExpectedResult = QuestRewardHelper.HardQuestReward)]
        public int GetRewardXp_ForValidDifficulty_ReturnsMatchingReward(QuestDifficulty difficulty)
        {
            // Arrange

            // Act & Assert (ExpectedResult)
            return QuestRewardHelper.GetRewardXp(difficulty);
        }


        [Test]
        public void GetRewardXp_ForInvalidDifficulty_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var invalidDifficulty = (QuestDifficulty)999;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => QuestRewardHelper.GetRewardXp(invalidDifficulty));

            Assert.That(ex!.ParamName, Is.EqualTo("difficutly"));
        }

        [Test]
        public void GetRewardXp_RewardValues_ArePositiveAndIncreaseWithDifficulty()
        {
            // Arrange
            var easyXp = QuestRewardHelper.GetRewardXp(QuestDifficulty.Easy);
            var mediumXp = QuestRewardHelper.GetRewardXp(QuestDifficulty.Medium);
            var hardXp = QuestRewardHelper.GetRewardXp(QuestDifficulty.Hard);

            // Act & Assert
            Assert.That(easyXp, Is.GreaterThan(0));
            Assert.That(mediumXp, Is.GreaterThan(easyXp));
            Assert.That(hardXp, Is.GreaterThan(mediumXp));
        }
    }
}