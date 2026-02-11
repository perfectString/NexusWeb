using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Nexus.Common.ValidationConstants;

namespace Nexus.Models
{
    /* id like to implement a logic that connects quests with interests
     * and add an xp system in the long run but i feel like i wouldnt have time
     * or i will make this project too difficult 
     * for now im going to leave this like that and if i have time i will do it
     * if i dont i will leave it for the next course
     */
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
