using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus.Models
{
    public class QuestJoiner
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Quest))]
        public int QuestId { get; set; }
        public Quest Quest { get; set; } = null!;

        [ForeignKey(nameof(Profile))]
        public string ProfileId { get; set; } = null!;
        public Profile Profile { get; set; } = null!;

        // this will work with the user xp system // 
        // for now there is no real logic connected to JoinedAt //
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    }
}