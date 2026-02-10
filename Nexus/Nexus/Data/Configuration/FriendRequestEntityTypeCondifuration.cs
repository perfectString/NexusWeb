using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Models;

namespace Nexus.Data.Configuration
{
    public class FriendRequestEntityTypeCondifuration : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> entity)
        {
            //FLUENT API
            //Control of navigational properties for friend requests

            entity
               .HasOne(fr => fr.Sender)
               .WithMany(fr => fr.SentFriendRequests)
               .HasForeignKey(fr => fr.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

            entity
               .HasOne(fr => fr.Receiver)
               .WithMany(fr => fr.ReceivedFriendRequests)
               .HasForeignKey(fr => fr.ReceiverId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
