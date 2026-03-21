using System.ComponentModel.DataAnnotations;
using Nexus.GCommon.Enums;
using static Nexus.GCommon.ValidationConstants;

namespace Nexus.ViewModels.Quest
{
    public class QuestAddViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLen)]
        [MinLength(TitleMinLen)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLen)]
        [MinLength(DescriptionMinLen)]
        public string Description { get; set; } = null!;

        [Required]
        public QuestDifficulty Difficulty { get; set; }

    }
}
