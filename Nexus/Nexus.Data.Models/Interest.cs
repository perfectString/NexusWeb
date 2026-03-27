using System.ComponentModel.DataAnnotations;
using static Nexus.GCommon.ValidationConstants;

namespace Nexus.Data.Models
{
    public class Interest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(InterestMaxName)]
        public string Name { get; set; } = null!;
        public ICollection<ProfileInterest> ProfileInterest { get; set; }
        = new HashSet<ProfileInterest>();

        public ICollection<QuestInterest> QuestInterest { get; set; }
        = new HashSet<QuestInterest>();


        //Connect to Quests in the future
    }
}
