using System.ComponentModel.DataAnnotations;

namespace Nexus.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(85)]
        
        public string Name { get; set; } = null!;

        [Range(18, 99)]
        public int Age { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(85)]
        public string City { get; set; } = null!;

        public ICollection<UserInterest> UserInterest { get; set; }
        = new HashSet<UserInterest>();
    }
}
