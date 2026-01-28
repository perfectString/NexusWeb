using System.ComponentModel.DataAnnotations;

namespace Nexus.Models
{
    public class Interest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public ICollection<UserInterest> UserInterest { get; set; }
        = new HashSet<UserInterest>();

    }
}
