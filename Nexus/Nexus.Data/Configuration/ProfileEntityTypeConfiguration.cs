using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Data.Models;
using Nexus.Data.Models.Enums;

namespace Nexus.Data.Configuration
{
    // For testing purposes the id of the profiles is beeing seeded 
    // with numbers regardless of the fact im using identity
    public class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
    {
        private readonly Profile[] profiles =
        {
            new Profile
            {
                Id = "1",
                DisplayName = "Alex",
                Age = 21,
                City = "Sofia",
                Bio = "New in the city and looking for new connections!",
                DesiredConnection = ConnectionType.Friends
            },
            new Profile
            {
                Id = "2",
                DisplayName = "Lidya",
                Age = 30,
                City = "Berlin",
                Bio = "Looking for my person",
                DesiredConnection = ConnectionType.Romantic
            },
            new Profile
            {
                Id = "3",
                DisplayName = "Liam",
                Age = 19,
                City = "Madrid",
                Bio = "Im heavy into gaming, i'd like to find people to play CS with!!!", //add gaming as interest
                DesiredConnection = ConnectionType.Groups
            },
            new Profile
            {
                Id = "4",
                DisplayName = "Dean",
                Age = 31,
                City = "London",
                Bio = "Work in the tech field.Into long night walks.", // add nature or smth as interest
                DesiredConnection = ConnectionType.Romantic
            },
        };

        public void Configure(EntityTypeBuilder<Profile> entity)
        {
            entity
                .HasData(this.profiles);
        }
    }
}
