using Nexus.GCommon.Enums;

namespace Nexus.Data.Services.Core.Helpers
{
    public static class QuestRewardHelper
    {
        public const int easyQuestReward = 75;
        public const int mediumQuestReward = 125;
        public const int hardQuestReward = 200;

        public static int GetRewardXp(QuestDifficulty difficutly)
        {
            return difficutly switch
            {
                QuestDifficulty.Easy => easyQuestReward,
                QuestDifficulty.Medium => mediumQuestReward,
                QuestDifficulty.Hard => hardQuestReward,
                _ => throw new ArgumentOutOfRangeException(nameof(difficutly),
                "Invalid quest difficulty.")
            };
        }
    }
}
