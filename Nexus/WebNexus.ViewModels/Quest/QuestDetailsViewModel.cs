using Nexus.ViewModels.Profile;
using Nexus.GCommon.Enums;

namespace Nexus.ViewModels.Quest
{
    public class QuestDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string QuestInitiator { get; set; } = null!;
        public Guid InitiatorId { get; set; }
        public QuestDifficulty Difficulty { get; set; }
        public int RewardExperience { get; set; }
        public QuestStatus Status { get; set; }

        public List<string> Interests { get; set; } = new();
        public List<ProfileViewModel> JoinedProfiles { get; set; } = new();
    }
}
