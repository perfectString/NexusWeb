using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Nexus.Models.Enums;

namespace Nexus.Models
{
    public class FriendRequest
    {
        // for now im making both FK and PK for more flexibility, planning to expand the need for it

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Sender))]
        public string SenderId { get; set; } = null!;
        public Profile Sender { get; set; } = null!;

        [ForeignKey(nameof(Reciever))]
        public string RecieverId { get; set; } = null!;
        public Profile Reciever { get; set; } = null!;
        public RequestStatus Status { get; set; }


    }
}
