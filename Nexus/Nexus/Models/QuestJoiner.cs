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

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    }
}