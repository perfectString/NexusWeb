using Nexus.GCommon.Enums;

namespace Nexus.Data.Services.Core.Helpers
{
    public static class QuestRewardHelper
    {
        public const int EasyQuestReward = 75;
        public const int MediumQuestReward = 125;
        public const int HardQuestReward = 200;

        public static int GetRewardXp(QuestDifficulty difficutly)
        {
            return difficutly switch
            {
                QuestDifficulty.Easy => EasyQuestReward,
                QuestDifficulty.Medium => MediumQuestReward,
                QuestDifficulty.Hard => HardQuestReward,
                _ => throw new ArgumentOutOfRangeException(nameof(difficutly),
                "Invalid quest difficulty.")
            };
        }
    }
}
