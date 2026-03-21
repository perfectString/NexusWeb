using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nexus.GCommon.Enums;
using static Nexus.GCommon.ValidationConstants;

namespace Nexus.Data.Models
{
    // id like to implement a logic that connects quests with interests
    // and add an xp system in the long run 
    public class Quest
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(TitleMaxLen)]
        [MinLength(TitleMinLen)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLen)]
        [MinLength(DescriptionMinLen)]
        public string Description { get; set; } = null!;
        public QuestStatus Status { get; set; } = QuestStatus.Active;

        public QuestDifficulty Difficulty { get; set; }

        public int RewardXp { get; set; }

        [ForeignKey(nameof(QuestInitiator))]
        public Guid QuestInitiatorId { get; set; }

        public Profile QuestInitiator { get; set; } = null!;

        public ICollection<QuestJoiner> QuestJoiners =
            new HashSet<QuestJoiner>();
    }
}
