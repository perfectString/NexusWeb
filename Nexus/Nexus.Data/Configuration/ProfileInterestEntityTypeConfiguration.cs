using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Data.Models;

namespace Nexus.Data.Configuration
{
    public class ProfileInterestEntityTypeConfiguration : IEntityTypeConfiguration<ProfileInterests>
    {
        private readonly ProfileInterests[] profilesInterests =
        {
            //user1
            new ProfileInterests
            {
                ProfileId = "1",
                InterestId = 2,
            },
            new ProfileInterests
            {
                ProfileId = "1",
                InterestId = 5,
            },
            new ProfileInterests
            {
                ProfileId = "1",
                InterestId = 6,
            },

            //user 2
            new ProfileInterests
            {
                ProfileId = "2",
                InterestId = 6,
            },
            new ProfileInterests
            {
                ProfileId = "2",
                InterestId = 11,
            },
            new ProfileInterests
            {
                ProfileId = "2",
                InterestId = 3,
            },

            //user 3
            new ProfileInterests
            {
                ProfileId = "3",
                InterestId = 4,
            },
            new ProfileInterests
            {
                ProfileId = "3",
                InterestId = 16,
            },
            new ProfileInterests
            {
                ProfileId = "3",
                InterestId = 9,
            },

            //user 4
            new ProfileInterests
            {
                ProfileId = "4",
                InterestId = 7,
            },
            new ProfileInterests
            {
                ProfileId = "4",
                InterestId = 15,
            },
            new ProfileInterests
            {
                ProfileId = "4",
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
