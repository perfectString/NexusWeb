using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus.Data.Models
{
    public class ProfileInterests
    {
        [ForeignKey(nameof(Profile))]
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; } = null!;

        [ForeignKey(nameof(Interest))]
        public int InterestId { get; set; } 
        public Interest Interest { get; set; } = null!;
    }
}
