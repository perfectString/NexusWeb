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

    }
}
