using System.ComponentModel.DataAnnotations;
using static Nexus.GCommon.ValidationConstants;
using Nexus.GCommon.Enums;
using Microsoft.AspNetCore.Identity;

namespace Nexus.Data.Models
{
    public class Profile : IdentityUser<Guid>
    {

        [Required]
        [MinLength(DisplayNameMinLen)]
        [MaxLength(DisplayNameMaxLen)]

        public string DisplayName { get; set; } = null!;

        [Range(AgeMinValue, AgeMaxValue)]
        public int Age { get; set; }

        [Required]
        [MinLength(CityMinLen)]
        [MaxLength(CityMaxLen)]
        public string City { get; set; } = null!;

        [MaxLength(BioMaxLen)]
        public string? Bio { get; set; }

        public int ExperiencePoints { get; set; }
        public int Level { get; set; }

        public ConnectionType DesiredConnection { get; set; }

        public ICollection<ProfileInterest> ProfileInterest { get; set; }
        = new HashSet<ProfileInterest>();
        public ICollection<QuestJoiner> JoinedQuests { get; set; } 
            = new HashSet<QuestJoiner>();
    }

}
