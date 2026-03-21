
using Nexus.GCommon.Enums;

namespace Nexus.Data.Services.Core.Helpers
{
    public static class QuestRewardHelper
    {
        public static int GetRewardXp(QuestDifficulty difficutly)
        {
            return difficutly switch
            {
                QuestDifficulty.Easy => 75,
                QuestDifficulty.Medium => 125,
                QuestDifficulty.Hard => 200,

            };
        }
    }
}
