using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nexus.Data.Models;

namespace Nexus.Data
{
    public class NexusDbContext : IdentityDbContext<Profile, IdentityRole<Guid>, Guid>
    {
        public NexusDbContext(DbContextOptions<NexusDbContext> options)
            : base(options)
        {

        }

        // im not adding a navigational prop for Profiles
        // it inherits from IdentityUser the data will be stored in
        // AspNetUsers in the DB
        public virtual DbSet<Interest> Interests { get; set; } = null!;
        public virtual DbSet<ProfileInterest> ProfileInterests { get; set; } = null!;

        public virtual DbSet<Quest> Quests { get; set; } = null!;
        public virtual DbSet<QuestJoiner> QuestJoiners { get; set; } = null!;
        public virtual DbSet<QuestInterest> QuestInterests { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(NexusDbContext).Assembly);
        }


    }
}

