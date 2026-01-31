using Microsoft.EntityFrameworkCore;
using Nexus.Models;

namespace Nexus.Data
{
    public class NexusDbContext : DbContext
    {
        // generic constuctor in case i would be working with other databases at later date
        public NexusDbContext(DbContextOptions<NexusDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Interest> Interests { get; set; } = null!;
        public virtual DbSet<UserInterest> UsersInterests { get; set; } = null!;
        public virtual DbSet<FriendRequest> FriendRequests { get; set; } = null!;


        //fluent api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Control of navigational properties for users and their interests

            modelBuilder.Entity<UserInterest>()
                .HasKey(x => new
                {
                    x.UserId,
                    x.InterestId
                });

            modelBuilder.Entity<UserInterest>()
                .HasOne(u => u.User)
                .WithMany(ui => ui.UserInterest)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserInterest>()
               .HasOne(i => i.Interest)
               .WithMany(ui => ui.UserInterest)
               .HasForeignKey(i => i.InterestId)
               .OnDelete(DeleteBehavior.Restrict);

            //Control of navigational properties for friend requests

            modelBuilder.Entity<FriendRequest>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<FriendRequest>()
               .HasOne(i => i.UserSender)
               .WithMany()
               .HasForeignKey(i => i.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRequest>()
               .HasOne(i => i.UserReciever)
               .WithMany()
               .HasForeignKey(i => i.RecieverId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(NexusDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

        }

    }
}
