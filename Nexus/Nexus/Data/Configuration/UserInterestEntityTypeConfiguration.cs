using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Models;

namespace Nexus.Data.Configuration
{
    public class UserInterestEntityTypeConfiguration : IEntityTypeConfiguration<ProfileInterests>
    {
        private readonly ProfileInterests[] _profilesInterests =
        {
            //user1
            new ProfileInterests
            {
                ProfileId = 1,
                InterestId = 2,
            },
            new ProfileInterests
            {
                ProfileId = 1,
                InterestId = 5,
            },
            new ProfileInterests
            {
                ProfileId = 1,
                InterestId = 6,
            },

            //user 2
            new ProfileInterests
            {
                ProfileId = 2,
                InterestId = 6,
            },
            new ProfileInterests
            {
                ProfileId = 2,
                InterestId = 11,
            },
            new ProfileInterests
            {
                ProfileId = 2,
                InterestId = 3,
            },
            //user 3
            new ProfileInterests
            {
                ProfileId = 3,
                InterestId = 4,
            },
            new ProfileInterests
            {
                ProfileId = 3,
                InterestId = 16,
            },
            new ProfileInterests
            {
                ProfileId = 3,
                InterestId = 9,
            },
            //user 4
            new ProfileInterests
            {
                ProfileId = 4,
                InterestId = 7,
            },
            new ProfileInterests
            {
                ProfileId = 4,
                InterestId = 15,
            },
            new ProfileInterests
            {
                ProfileId = 4,
                InterestId = 17,
            },
            //user 5
            new ProfileInterests
            {
                ProfileId = 5,
                InterestId = 23,
            },
            new ProfileInterests
            {
                ProfileId = 5,
                InterestId = 22,
            },
            new ProfileInterests
            {
                ProfileId = 5,
                InterestId = 21,
            },
            //user 6
            new ProfileInterests
            {
                ProfileId = 6,
                InterestId = 14,
            },
            new ProfileInterests
            {
                ProfileId = 6,
                InterestId = 13,
            },
            new ProfileInterests
            {
                ProfileId = 6,
                InterestId = 26,
            },
            //user 7
            new ProfileInterests
            {
                ProfileId = 7,
                InterestId = 2,
            },
            new ProfileInterests
            {
                ProfileId = 7,
                InterestId = 24,
            },
            new ProfileInterests
            {
                ProfileId = 7,
                InterestId = 22,
            },
            //user 8
            new ProfileInterests
            {
                ProfileId = 8,
                InterestId = 2,
            },
            new ProfileInterests
            {
                ProfileId = 8,
                InterestId = 19,
            },
            new ProfileInterests
            {
                ProfileId = 8,
                InterestId = 15,
            },
            //user 9
            new ProfileInterests
            {
                ProfileId = 9,
                InterestId = 17,
            },
            new ProfileInterests
            {
                ProfileId = 9,
                InterestId = 16,
            },
            new ProfileInterests
            {
                ProfileId = 9,
                InterestId = 19,
            },
            //user 10
            new ProfileInterests
            {
                ProfileId = 10,
                InterestId = 29,
            },
            new ProfileInterests
            {
                ProfileId = 10,
                InterestId = 27,
            },
            new ProfileInterests
            {
                ProfileId = 10,
                InterestId = 30,
            },

        };
        public void Configure(EntityTypeBuilder<ProfileInterests> entity)
        {
            entity
                .HasData(this._profilesInterests);
        }
    }
}
