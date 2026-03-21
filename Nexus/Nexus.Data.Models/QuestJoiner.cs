using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus.Data.Models
{
    public class QuestJoiner
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Quest))]
        public int QuestId { get; set; }
        public Quest Quest { get; set; } = null!;

        [ForeignKey(nameof(Profile))]
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; } = null!;

    }
}