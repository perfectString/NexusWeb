
namespace Nexus.ViewModels.Admin.Profile
{
    public class ProfileManagementViewModel
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; } = null!;

        public string City { get; set; } = null!;

        public string? Bio { get; set; }

        public int ExperiencePoints { get; set; }

        public int Level { get; set; }

        public int XpIntoCurrentLevel { get; set; }

        public int XpNeededPerLevel { get; set; }

        public int ProgressPercentage { get; set; }
    }
}
