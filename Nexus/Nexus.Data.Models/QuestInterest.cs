
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus.Data.Models
{
    public class QuestInterest
    {
        [ForeignKey(nameof(Quest))]
        public int QuestId { get; set; }
        public Quest Quest { get; set; } = null!;

        [ForeignKey(nameof(Interest))]
        public int InterestId { get; set; }
        public Interest Interest { get; set; } = null!;
    }
}
