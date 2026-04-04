using System.ComponentModel.DataAnnotations;
using Nexus.GCommon.Enums;
using Nexus.ViewModels.Profile;
using static Nexus.GCommon.ValidationConstants;
using static Nexus.GCommon.OutputMessages;

namespace Nexus.ViewModels.Quest
{
    public class QuestAddViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = QuestTitleRequiredMessage)]
        [MaxLength(TitleMaxLen, ErrorMessage = QuestTitleMaxLenMessage)]
        [MinLength(TitleMinLen, ErrorMessage = QuestTitleMinLenMessage)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLen, ErrorMessage = QuestDescriptionMaxLenMessage)]
        [MinLength(DescriptionMinLen, ErrorMessage = QuestDescriptionMinLenMessage)]
        public string Description { get; set; } = null!;

        [Required]
        public QuestDifficulty Difficulty { get; set; }

        public List<int> InterestIds { get; set; } = new();

        public List<AvailableInterestViewModel> AvailableInterests { get; set; } 
            = new();
    }
}
