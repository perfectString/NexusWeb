using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Data.Models;
using Nexus.GCommon.Enums;
using static Nexus.Data.Seeding.GuidProfileSeeder;

namespace Nexus.Data.Configuration
{
   
    public class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
    {
        private readonly Profile[] profiles =
        {
            new Profile
            {
                Id = User1Id,
                DisplayName = "Alex",
                Age = 21,
                City = "Sofia",
                Bio = "New in the city and looking for new connections!",
                DesiredConnection = ConnectionType.Friends,
                Level = 9,
                ExperiencePoints = 800,
            },
            new Profile
            {
                Id = User2Id,
                DisplayName = "Lidya",
                Age = 30,
                City = "Berlin",
                Bio = "Looking for my person",
                DesiredConnection = ConnectionType.Romantic,
                Level = 4,
                ExperiencePoints = 350
            },
            new Profile
            {
                Id = User3Id,
                DisplayName = "Liam",
                Age = 19,
                City = "Madrid",
                Bio = "Im heavy into gaming, i'd like to find people to play CS with!!!", //add gaming as interest
                DesiredConnection = ConnectionType.Groups,
                Level = 13,
                ExperiencePoints = 1200
            },
            new Profile
            {
                Id = User4Id,
                DisplayName = "Dean",
                Age = 31,
                City = "London",
                Bio = "Work in the tech field.Into long night walks.", // add nature or smth as interest
                DesiredConnection = ConnectionType.Romantic,
                Level = 7,
                ExperiencePoints = 650
            },
        };

        public void Configure(EntityTypeBuilder<Profile> entity)
        {
            entity
                .HasData(this.profiles);
        }
    }
}
