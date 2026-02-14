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

        // im not adding a navigational prop for Profiles here since
        // it inherits from IdentityUser so the data will be stored in
        // AspNetUsers in the db 
        public virtual DbSet<Interest> Interests { get; set; } = null!;
        public virtual DbSet<ProfileInterests> ProfileInterests { get; set; } = null!;

        public virtual DbSet<Quest> Quests { get; set; } = null!;
        public virtual DbSet<QuestJoiner> QuestJoiners { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(NexusDbContext).Assembly);
        }


    }
}

