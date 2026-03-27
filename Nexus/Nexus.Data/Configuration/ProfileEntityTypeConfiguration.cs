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
            new Profile
            {
                Id = User5Id,
                DisplayName = "Maya",
                Age = 24,
                City = "Paris",
                Bio = "Love art galleries and coffee spots. Always up for creative activities!",
                DesiredConnection = ConnectionType.Friends,
                Level = 6,
                ExperiencePoints = 520
            },

            new Profile()
            {
                 Id = User6Id,
                 DisplayName = "Noah",
                 Age = 27,
                 City = "Amsterdam",
                 Bio = "Cycling enthusiast and fitness lover. Looking for active people.",
                 DesiredConnection = ConnectionType.Groups,
                 Level = 10,
                 ExperiencePoints = 950
            },
            new Profile()
            {
                Id =User7Id,
                DisplayName = "Elena",
                Age = 28,
                City = "Amsterdam",
                Bio = "Foodie and traveler. Let’s explore new places together!", 
                DesiredConnection = ConnectionType.Friends,
                Level = 5,
                ExperiencePoints = 430
            },
            new Profile
            {
                Id = User8Id,
                DisplayName = "Victor",
                Age = 29,
                City = "Sofia",
                Bio = "Into tech, startups and hackathons. Building cool stuff.",
                DesiredConnection = ConnectionType.Groups,
                Level = 12,
                ExperiencePoints = 1100
            },

        };

        public void Configure(EntityTypeBuilder<Profile> entity)
        {
            entity
                .HasData(this.profiles);
        }
    }
}
