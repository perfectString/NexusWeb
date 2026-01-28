using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Models;

namespace Nexus.Data.Configuration
{
    public class UserInterestEntityTypeConfiguration : IEntityTypeConfiguration<UserInterest>
    {
        private readonly UserInterest[] _usersInterests =
        {
            //user1
            new UserInterest
            {
                UserId = 1,
                InterestId = 2,
            },
            new UserInterest
            {
                UserId = 1,
                InterestId = 5,
            },
            new UserInterest
            {
                UserId = 1,
                InterestId = 6,
            },

            //user 2
            new UserInterest
            {
                UserId = 2,
                InterestId = 6,
            },
            new UserInterest
            {
                UserId = 2,
                InterestId = 11,
            },
            new UserInterest
            {
                UserId = 2,
                InterestId = 3,
            },
            //user 3
            new UserInterest
            {
                UserId = 3,
                InterestId = 4,
            },
            new UserInterest
            {
                UserId = 3,
                InterestId = 16,
            },
            new UserInterest
            {
                UserId = 3,
                InterestId = 9,
            },
            //user 4
            new UserInterest
            {
                UserId = 4,
                InterestId = 7,
            },
            new UserInterest
            {
                UserId = 4,
                InterestId = 15,
            },
            new UserInterest
            {
                UserId = 4,
                InterestId = 17,
            },
            //user 5
            new UserInterest
            {
                UserId = 5,
                InterestId = 23,
            },
            new UserInterest
            {
                UserId = 5,
                InterestId = 22,
            },
            new UserInterest
            {
                UserId = 5,
                InterestId = 21,
            },
            //user 6
            new UserInterest
            {
                UserId = 6,
                InterestId = 14,
            },
            new UserInterest
            {
                UserId = 6,
                InterestId = 13,
            },
            new UserInterest
            {
                UserId = 6,
                InterestId = 26,
            },
            //user 7
            new UserInterest
            {
                UserId = 7,
                InterestId = 2,
            },
            new UserInterest
            {
                UserId = 7,
                InterestId = 24,
            },
            new UserInterest
            {
                UserId = 7,
                InterestId = 22,
            },
            //user 8
            new UserInterest
            {
                UserId = 8,
                InterestId = 2,
            },
            new UserInterest
            {
                UserId = 8,
                InterestId = 19,
            },
            new UserInterest
            {
                UserId = 8,
                InterestId = 15,
            },
            //user 9
            new UserInterest
            {
                UserId = 9,
                InterestId = 17,
            },
            new UserInterest
            {
                UserId = 9,
                InterestId = 16,
            },
            new UserInterest
            {
                UserId = 9,
                InterestId = 19,
            },
            //user 10
            new UserInterest
            {
                UserId = 10,
                InterestId = 29,
            },
            new UserInterest
            {
                UserId = 10,
                InterestId = 27,
            },
            new UserInterest
            {
                UserId = 10,
                InterestId = 30,
            },

        };
        public void Configure(EntityTypeBuilder<UserInterest> entity)
        {
            entity
                .HasData(this._usersInterests);
        }
    }
}
