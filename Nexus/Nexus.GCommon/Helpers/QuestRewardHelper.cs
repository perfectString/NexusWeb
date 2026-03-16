using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Data.Models.Enums;

namespace Nexus.GCommon.Helpers
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
