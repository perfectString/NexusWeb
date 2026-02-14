using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Models;

namespace Nexus.Data.Configuration
{
    public class InterestEntityTypeConfiguration : IEntityTypeConfiguration<Interest>
    {
        private readonly Interest[] interests =
        {
            new Interest
            {
                Id = 1,
                Name = "Reading",
            },
            new Interest
            {
                Id = 2,
                Name = "Music",
            },
            new Interest
            {
                Id = 3,
                Name = "Movies",
            },
            new Interest
            {
                Id = 4,
                Name = "Gaming",
            },
            new Interest
            {
                Id = 5,
                Name = "Fitness",
            },
            new Interest
            {
                Id = 6,
                Name = "Cooking",
            },
            new Interest
            {
                Id = 7,
                Name = "Travelling",
            },
            new Interest
            {
                Id = 8,
                Name = "Photography",
            },
            new Interest
            {
                Id = 9,
                Name = "Animals",
            },
            new Interest
            {
                Id = 10,
                Name = "Board Games",
            },
            new Interest
            {
                Id = 11,
                Name = "Meditation",
            },
            new Interest
            {
                Id = 12,
                Name = "Writing",
            },
            new Interest
            {
                Id = 13,
                Name = "Education",
            },
            new Interest
            {
                Id = 14,
                Name = "Languages",
            },
            new Interest
            {
                Id = 15,
                Name = "Nature",
            },
            new Interest
            {
                Id = 16,
                Name = "Hiking",
            },
            new Interest
            {
                Id = 17,
                Name = "Camping",
            },
            new Interest
            {
                Id = 18,
                Name = "Gardening",
            },
            new Interest
            {
                Id = 19,
                Name = "Family",
            },
            new Interest
            {
                Id = 20,
                Name = "Socializing",
            },
            new Interest
            {
                Id = 21,
                Name = "Volunteering",
            },
            new Interest
            {
                Id = 22,
                Name = "Technology",
            },
            new Interest
            {
                Id = 23,
                Name = "News",
            },
            new Interest
            {
                Id = 24,
                Name = "Politics",
            },
            new Interest
            {
                Id = 25,
                Name = "Crafts",
            },
            new Interest
            {
                Id = 26,
                Name = "Art",
            },
            new Interest
            {
                Id = 27,
                Name = "Reading",
            },
            new Interest
            {
                Id = 28,
                Name = "Drawing",
            },
            new Interest
            {
                Id = 29,
                Name = "Fashion",
            },
            new Interest
            {
                Id = 30,
                Name = "Driving",
            }
        };



        public void Configure(EntityTypeBuilder<Interest> entity)
        {
            entity
                .HasData(this.interests);
        }
    }


}
