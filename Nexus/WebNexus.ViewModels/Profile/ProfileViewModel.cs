using Nexus.Data.Models.Enums;

namespace Nexus.ViewModels.Profile
{
    public class ProfileViewModel
    {

        public string Id { get; set; } = null!;
        public string DisplayName { get; set; } = null!;

        public int Age { get; set; }

        public string City { get; set; } = null!;

        public string? Bio { get; set; }

        public int Level { get; set; }
        public int ExperiencePoints { get; set; }
        public int XpIntoCurrentLevel { get; set; }
        public int XpNeededPerLevel { get; set; }
        public int XpNeededToNextLevel { get; set; }
        public int ProgressPercentage { get; set; }

        public ConnectionType DesiredConnection { get; set; }
        public List<string> Interests { get; set; }
            = new List<string>();
    }
}
