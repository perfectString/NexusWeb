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
        public ICollection<ProfileInterests> ProfileInterest { get; set; }
        = new HashSet<ProfileInterests>();


        //Connect to Quests in the future
    }
}
