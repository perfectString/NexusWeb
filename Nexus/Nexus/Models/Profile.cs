using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.ComponentModel.DataAnnotations;
using static Nexus.Common.ValidationConstants;
using Nexus.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Nexus.Models
{
    public class Profile : IdentityUser
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

        public ConnectionType DesiredConnection { get; set; }

        public ICollection<ProfileInterests> ProfileInterest { get; set; }
        = new HashSet<ProfileInterests>();
    }

}
