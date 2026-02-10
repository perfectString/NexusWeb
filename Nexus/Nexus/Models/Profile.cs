using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.ComponentModel.DataAnnotations;
using static Nexus.Common.ValidationConstants;
using Nexus.Models.Enums;
using Microsoft.AspNetCore.Identity;
using NuGet.Configuration;

namespace Nexus.Models
{
    public class Profile : IdentityUser
    {

        [Required]
        [MinLength(DisplayNameMinLen)]
        [MaxLength(DisplayNameMaxLen)]
        
        public string DisplayName { get; set; } = "no display name";

        [Range(AgeMinValue, AgeMaxValue)]
        public int Age { get; set; }

        [Required]
        [MinLength(CityMinLen)]
        [MaxLength(CityMaxLen)]
        public string City { get; set; } = "unknown";

        [MaxLength(BioMaxLen)]
        public string? Bio { get; set; }

        public ConnectionType DesiredConnection { get; set; }

        public ICollection<ProfileInterests> ProfileInterest { get; set; }
        = new HashSet<ProfileInterests>();
        public ICollection<FriendRequest> SentFriendRequests { get; set; } 
            = new HashSet<FriendRequest>();
        public ICollection<FriendRequest> ReceivedFriendRequests { get; set; }
            = new HashSet<FriendRequest>();
    }

}
