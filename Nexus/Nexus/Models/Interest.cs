using System.ComponentModel.DataAnnotations;
using static Nexus.Common.ValidationConstants;

namespace Nexus.Models
{
    public class Interest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(InterestMaxName)]
        public string Name { get; set; } = null!;
        public ICollection<UserInterest> UserInterest { get; set; }
        = new HashSet<UserInterest>();

    }
}
