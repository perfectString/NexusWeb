using System.ComponentModel.DataAnnotations;
using static Nexus.Common.ValidationConstants;

namespace Nexus.ViewModels.Quest
{
    public class QuestAddViewModel
    {
        [Required]
        [MaxLength(TitleMaxLen)]
        [MinLength(TitleMinLen)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLen)]
        [MinLength(DescriptionMinLen)]
        public string Description { get; set; } = null!;
    }
}
