using System.ComponentModel.DataAnnotations;
using Nexus.Data.Models.Enums; 
using static Nexus.GCommon.ValidationConstants;

namespace Nexus.ViewModels.Profile
{
    public class ProfileEditViewModel
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

        public List<int> InterestId { get; set; }
        = new List<int>();

        public List<AvailableInterestViewModel> AvailableInterests { get; set; }
            = new List<AvailableInterestViewModel>();
    }
}
