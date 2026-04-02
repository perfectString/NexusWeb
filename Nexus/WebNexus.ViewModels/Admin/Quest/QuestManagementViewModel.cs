
using System.ComponentModel.DataAnnotations;
using Nexus.GCommon.Enums;
using Nexus.ViewModels.Profile;
using static Nexus.GCommon.ValidationConstants;

namespace Nexus.ViewModels.Admin.Quest
{
    public class QuestManagementViewModel
    {
        public int Id { get; set; }
        public string? QuestInitiator { get; set; } 
        public Guid InitiatorId { get; set; }

        [Required]
        [MaxLength(TitleMaxLen)]
        [MinLength(TitleMinLen)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLen)]
        [MinLength(DescriptionMinLen)]
        public string Description { get; set; } = null!;
        public QuestDifficulty Difficulty { get; set; }
        public int RewardExperience { get; set; }
        public QuestStatus Status { get; set; }
        public List<int> InterestIds { get; set; } = new();
        public List<AvailableInterestViewModel> AvailableInterests { get; set; }
            = new();



    }
}
