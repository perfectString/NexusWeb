
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Data.Models;
using static Nexus.Data.Seeding.GuidProfileSeeder;

namespace Nexus.Data.Configuration
{
    public class ProfileInterestEntityTypeConfiguration : IEntityTypeConfiguration<ProfileInterests>
    {
        private readonly ProfileInterests[] profilesInterests =
        {
            //user1
            new ProfileInterests
            {
                ProfileId =  User1Id,
                InterestId = 2,
            },
            new ProfileInterests
            {
                ProfileId =  User1Id,
                InterestId = 5,
            },
            new ProfileInterests
            {
                ProfileId =  User1Id,
                InterestId = 6,
            },

            //user 2
            new ProfileInterests
            {
                ProfileId =  User2Id,
                InterestId = 6,
            },
            new ProfileInterests
            {
                ProfileId = User2Id,
                InterestId = 11,
            },
            new ProfileInterests
            {
                ProfileId = User2Id,
                InterestId = 3,
            },

            //user 3
            new ProfileInterests
            {
                ProfileId = User3Id,
                InterestId = 4,
            },
            new ProfileInterests
            {
                ProfileId = User3Id,
                InterestId = 16,
            },
            new ProfileInterests
            {
                ProfileId = User3Id,
                InterestId = 9,
            },

            //user 4
            new ProfileInterests
            {
                ProfileId = User4Id,
                InterestId = 7,
            },
            new ProfileInterests
            {
                ProfileId = User4Id,
                InterestId = 15,
            },
            new ProfileInterests
            {
                ProfileId = User4Id,
                InterestId = 17,
            },


        };
        public void Configure(EntityTypeBuilder<ProfileInterests> entity)
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
