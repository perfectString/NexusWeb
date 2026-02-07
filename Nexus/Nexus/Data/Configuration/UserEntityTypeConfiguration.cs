using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Models;
using Nexus.Models.Enums;

namespace Nexus.Data.Configuration
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
    {
        private readonly Profile[] _profiles =
        {
            new Profile
            {
                Id = 1,
                Name = "Alex",
                Age = 21,
                City = "Sofia",
                Bio = "New in the city and looking for new connections!",
                DesiredConnection = ConnectionType.Friends
            },
            new Profile
            {
                Id = 2,
                Name = "Lidya",
                Age = 30,
                City = "Berlin",
                Bio = "Looking for my person",
                DesiredConnection = ConnectionType.Romantic
            },
            new Profile
            {
                Id = 3,
                Name = "Liam",
                Age = 19,
                City = "Madrid",
                Bio = "Im heavy into gaming, i'd like to find people to play CS with!!!", //add gaming as interest
                DesiredConnection = ConnectionType.Groups
            },
            new Profile
            {
                Id = 4,
                Name = "Dean",
                Age = 31,
                City = "London",
                Bio = "Work in the tech field.Into long night walks.", // add nature or smth as interest
                DesiredConnection = ConnectionType.Romantic
            },
            new Profile
            {
                Id = 5,
                Name = "Peter",
                Age = 27,
                City = "Sofia",
                Bio = "Let's hang out?",
                DesiredConnection = ConnectionType.Friends 
            },
            new Profile
            {
                Id = 6,
                Name = "Emma",
                Age = 44,
                City = "Rome",
                DesiredConnection = ConnectionType.Romantic
            },
            new Profile
            {
                Id = 7,
                Name = "Luca",
                Age = 20,
                City = "Sofia",
                Bio = "Heavy metal!!",
                DesiredConnection = ConnectionType.Groups //music
            },
            new Profile
            {
                Id = 8,
                Name = "Alexandra",
                Age = 26,
                City = "Madrid",
                Bio = "Recommend me new music",
                DesiredConnection = ConnectionType.Friends //music
            },
            new Profile
            {
                Id = 9,
                Name = "Olivia",
                Age = 33,
                City = "London",
                Bio = "Lets travel the world together!", // travel
                DesiredConnection = ConnectionType.Romantic
            },
            new Profile
            {
                Id = 10,
                Name = "Dan",
                Age = 19,
                City = "London",
                Bio = "I would like to find a local band. Can play bass pretty good!",
                DesiredConnection = ConnectionType.Groups
            }
        };

        public void Configure(EntityTypeBuilder<Profile> entity)
        {
            entity
                .HasData(this._profiles);
        }
    }
}
