using System.ComponentModel.DataAnnotations;
using Nexus.Models.Enums;

namespace Nexus.ViewModels.Profile
{
    public class ProfileAllViewModel
    {

        public string Id { get; set; } = null!;
        public string DisplayName { get; set; } = null!;

        public int Age { get; set; }

        public string City { get; set; } = null!;

        public string? Bio { get; set; }

        public ConnectionType DesiredConnection { get; set; }

        public List<string> Interests { get; set; }
            = new List<string>();
    }
}
