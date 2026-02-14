using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Nexus.Common.ValidationConstants;

namespace Nexus.Models
{
    /* id like to implement a logic that connects quests with interests
       and add an xp system in the long run */
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

        [ForeignKey(nameof(QuestInitiator))]
        public string QuestInitiatorId { get; set; } = null!;

        public Profile QuestInitiator { get; set; } = null!;

        public ICollection<QuestJoiner> QuestJoiners =
            new HashSet<QuestJoiner>();
    }
}
