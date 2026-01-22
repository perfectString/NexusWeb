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

        [ForeignKey(nameof(UserSender))]
        public int SenderId { get; set; }
        public User UserSender { get; set; } = null!;

        [ForeignKey(nameof(UserReciever))]
        public int RecieverId { get; set; }
        public User UserReciever { get; set; } = null!;
        public RequestStatus Status { get; set; }


    }
}
