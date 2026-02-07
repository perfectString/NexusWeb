using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nexus.Models;

namespace Nexus.Data
{
    public class NexusDbContext : IdentityDbContext<Profile>
    {
        public NexusDbContext(DbContextOptions<NexusDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Profile> Profiles { get; set; } = null!;
        public virtual DbSet<Interest> Interests { get; set; } = null!;
        public virtual DbSet<ProfileInterests> ProfileInterests { get; set; } = null!;
        public virtual DbSet<FriendRequest> FriendRequests { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          

            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(NexusDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

        }


    }
}

