using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus.Models
{
    public class UserInterest
    {
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [ForeignKey(nameof(Interest))]
        public int InterestId { get; set; }
        public Interest Interest { get; set; } = null!;

    }
}
