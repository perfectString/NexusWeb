using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nexus.Models.Enums;

namespace Nexus.Models
{
    public class FriendRequest
    {
        // for now im making both FK and PK for more flexibility, planning to expand the need for it

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ProfileSender))]
        public int SenderId { get; set; }
        public Profile ProfileSender { get; set; } = null!;

        [ForeignKey(nameof(ProfileReciever))]
        public int RecieverId { get; set; }
        public Profile ProfileReciever { get; set; } = null!;
        public RequestStatus Status { get; set; }


    }
}
