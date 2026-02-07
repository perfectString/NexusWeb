using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.ComponentModel.DataAnnotations;
using static Nexus.Common.ValidationConstants;
using Nexus.Models.Enums;

namespace Nexus.Models
{
    public class Profile
    {
        
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(NameMinLen)]
        [MaxLength(NameMaxLen)]
        
        public string Name { get; set; } = null!;

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
