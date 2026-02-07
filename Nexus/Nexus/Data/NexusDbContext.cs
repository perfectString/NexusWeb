using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nexus.Models;

namespace Nexus.Data
{
    public class NexusDbContext : IdentityDbContext
    {
        public NexusDbContext(DbContextOptions<NexusDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Profile> Profiles { get; set; } = null!;
        public virtual DbSet<Interest> Interests { get; set; } = null!;
        public virtual DbSet<ProfileInterests> ProfileInterests { get; set; } = null!;
        public virtual DbSet<FriendRequest> FriendRequests { get; set; } = null!;


        //fluent api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Control of navigational properties for users and their interests

            modelBuilder.Entity<ProfileInterests>()
                .HasKey(x => new
                {
                    x.ProfileId,
                    x.InterestId
                });

            modelBuilder.Entity<ProfileInterests>()
                .HasOne(u => u.Profile)
                .WithMany(ui => ui.ProfileInterest)
                .HasForeignKey(u => u.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProfileInterests>()
               .HasOne(i => i.Interest)
               .WithMany(ui => ui.ProfileInterest)
               .HasForeignKey(i => i.InterestId)
               .OnDelete(DeleteBehavior.Restrict);

            //Control of navigational properties for friend requests

            modelBuilder.Entity<FriendRequest>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<FriendRequest>()
               .HasOne(i => i.ProfileSender)
               .WithMany()
               .HasForeignKey(i => i.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRequest>()
               .HasOne(i => i.ProfileReciever)
               .WithMany()
               .HasForeignKey(i => i.RecieverId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(NexusDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

        }


    }
}

