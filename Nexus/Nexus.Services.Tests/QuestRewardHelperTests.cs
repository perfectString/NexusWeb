using Nexus.Data.Services.Core.Helpers;
using Nexus.GCommon.Enums;

namespace Nexus.Services.Tests
{
    [TestFixture]
    public class QuestRewardHelperTests
    {
        [TestCase(QuestDifficulty.Easy, ExpectedResult = QuestRewardHelper.easyQuestReward)]
        [TestCase(QuestDifficulty.Medium, ExpectedResult = QuestRewardHelper.mediumQuestReward)]
        [TestCase(QuestDifficulty.Hard, ExpectedResult = QuestRewardHelper.hardQuestReward)]
        public int GetRewardXp_ReturnsCorrectXp(QuestDifficulty difficulty)
        {
            return QuestRewardHelper.GetRewardXp(difficulty);
        }

        [Test]
        public void GetRewardXp_InvalidDifficulty_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => QuestRewardHelper.GetRewardXp((QuestDifficulty)999));
        }
    }
}
