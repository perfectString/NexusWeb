
using System.ComponentModel.DataAnnotations;
using static Nexus.GCommon.ValidationConstants;
using static Nexus.GCommon.OutputMessages;

namespace Nexus.ViewModels.Admin.Profile
{
    public class ProfileManagementViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = DisplayNameRequiredMessage)]
        [MinLength(DisplayNameMinLen, ErrorMessage = DisplayNameMinLenMessage)]
        [MaxLength(DisplayNameMaxLen, ErrorMessage = DisplayNameMaxLenMessage)]
        public string DisplayName { get; set; } = null!;

        [Required(ErrorMessage = CityMissingMessage)]
        [MinLength(CityMinLen, ErrorMessage = CityMinLenMessage)]
        [MaxLength(CityMaxLen, ErrorMessage = CityMaxLenMessage)]
        public string City { get; set; } = null!;

        [MaxLength(BioMaxLen, ErrorMessage = BioMaxLenMessage)]
        public string? Bio { get; set; }

        public int ExperiencePoints { get; set; }

        public int Level { get; set; }

        public int XpIntoCurrentLevel { get; set; }

        public int XpNeededPerLevel { get; set; }

        public int ProgressPercentage { get; set; }
    }
}
