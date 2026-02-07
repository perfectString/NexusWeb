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
                .HasKey(x => x.Id);

            entity
               .HasOne(i => i.Sender)
               .WithMany()
               .HasForeignKey(i => i.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

            entity
               .HasOne(i => i.Reciever)
               .WithMany()
               .HasForeignKey(i => i.RecieverId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
