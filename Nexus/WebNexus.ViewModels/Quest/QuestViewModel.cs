
using Nexus.Data.Models.Enums;
using Nexus.GCommon.Enums;

namespace Nexus.ViewModels.Quest
{
    public class QuestViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string QuestInitiator { get; set; } = null!;
        public QuestDifficulty Difficulty { get; set; }
        public int RewardExperience { get; set; }
        public QuestStatus Status { get; set; }

        public string InitiatorId { get; set; } = null!;
    }
}
