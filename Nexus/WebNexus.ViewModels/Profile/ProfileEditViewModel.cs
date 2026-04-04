using System.ComponentModel.DataAnnotations;
using Nexus.GCommon.Enums;
using static Nexus.GCommon.ValidationConstants;
using static Nexus.GCommon.OutputMessages;

namespace Nexus.ViewModels.Profile
{
    public class ProfileEditViewModel
    {
        [Required(ErrorMessage = DisplayNameRequiredMessage)]
        [MinLength(DisplayNameMinLen, ErrorMessage = DisplayNameMinLenMessage)]
        [MaxLength(DisplayNameMaxLen, ErrorMessage = DisplayNameMaxLenMessage)]
        public string DisplayName { get; set; } = null!;

        [Range(AgeMinValue, AgeMaxValue,
            ErrorMessage = AgeRangeMessage)]
        public int Age { get; set; }

        [Required(ErrorMessage = CityMissingMessage)]
        [MinLength(CityMinLen, ErrorMessage = CityMinLenMessage)]
        [MaxLength(CityMaxLen, ErrorMessage = CityMaxLenMessage)]
        public string City { get; set; } = null!;

        [MaxLength(BioMaxLen, ErrorMessage = BioMaxLenMessage)]
        public string? Bio { get; set; }

        public ConnectionType DesiredConnection { get; set; }

        public List<int> InterestId { get; set; }
        = new List<int>();

        public List<AvailableInterestViewModel> AvailableInterests { get; set; }
            = new List<AvailableInterestViewModel>();
    }
}
