
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Data.Models;
using static Nexus.Data.Seeding.GuidProfileSeeder;

namespace Nexus.Data.Configuration
{
    public class ProfileInterestEntityTypeConfiguration : IEntityTypeConfiguration<ProfileInterest>
    {
        private readonly ProfileInterest[] profilesInterests =
        {
            //user1
            new ProfileInterest
            {
                ProfileId =  User1Id,
                InterestId = 2,
            },
            new ProfileInterest
            {
                ProfileId =  User1Id,
                InterestId = 5,
            },
            new ProfileInterest
            {
                ProfileId =  User1Id,
                InterestId = 6,
            },

            //user 2
            new ProfileInterest
            {
                ProfileId =  User2Id,
                InterestId = 6,
            },
            new ProfileInterest
            {
                ProfileId = User2Id,
                InterestId = 11,
            },
            new ProfileInterest
            {
                ProfileId = User2Id,
                InterestId = 3,
            },

            //user 3
            new ProfileInterest
            {
                ProfileId = User3Id,
                InterestId = 4,
            },
            new ProfileInterest
            {
                ProfileId = User3Id,
                InterestId = 16,
            },
            new ProfileInterest
            {
                ProfileId = User3Id,
                InterestId = 9,
            },

            //user 4
            new ProfileInterest
            {
                ProfileId = User4Id,
                InterestId = 7,
            },
            new ProfileInterest
            {
                ProfileId = User4Id,
                InterestId = 15,
            },
            new ProfileInterest
            {
                ProfileId = User4Id,
                InterestId = 17,
            },
            //user 5
            new ProfileInterest
            {
                ProfileId = User5Id,
                InterestId = 8,
            },
            new ProfileInterest
            {
                ProfileId = User5Id,
                InterestId = 26,
            },
            //user 6
            new ProfileInterest
            {
                ProfileId = User6Id,
                InterestId = 5,
            },
            new ProfileInterest
            {
                ProfileId = User6Id,
                InterestId = 15,
            },
            new ProfileInterest
            {
                ProfileId = User6Id,
                InterestId = 16,
            },
            //user 7
            new ProfileInterest
            {
                ProfileId = User7Id,
                InterestId = 7,
            },
            new ProfileInterest
            {
                ProfileId = User7Id,
                InterestId = 20,
            },
            //user 8
            new ProfileInterest
            {
                ProfileId = User8Id,
                InterestId = 22,
            },
         

        };
        public void Configure(EntityTypeBuilder<ProfileInterest> entity)
        {
            // FLUENT API
            //Control of navigational properties for users and their interests

            entity
                .HasKey(x => new
                {
                    x.ProfileId,
                    x.InterestId
                });

            entity
                 .HasOne(u => u.Profile)
                .WithMany(ui => ui.ProfileInterest)
                .HasForeignKey(u => u.ProfileId);

            entity
                .HasOne(i => i.Interest)
               .WithMany(ui => ui.ProfileInterest)
               .HasForeignKey(i => i.InterestId);

            // SEEDING OF DATA
            entity
                .HasData(this.profilesInterests);
        }
    }
}
