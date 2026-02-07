using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus.Models
{
    public class ProfileInterests
    {
        [ForeignKey(nameof(Profile))]
        public string ProfileId { get; set; } = null!;
        public Profile Profile { get; set; } = null!;

        [ForeignKey(nameof(Interest))]
        public int InterestId { get; set; } 
        public Interest Interest { get; set; } = null!;

    }
}
