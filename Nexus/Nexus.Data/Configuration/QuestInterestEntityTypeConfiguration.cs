
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Data.Models;

namespace Nexus.Data.Configuration
{
    public class QuestInterestEntityTypeConfiguration : IEntityTypeConfiguration<QuestInterest>
    {
        private readonly QuestInterest[] QuestInterests =
        {
            //Demo quest 1
            new QuestInterest()
            {
                QuestId = 1,
                InterestId = 6,

            },
            new QuestInterest()
            {

                QuestId = 1,
                InterestId = 15,
            },

            //Demo quest 2
            new QuestInterest()
            {

                QuestId = 2,
                InterestId = 4,
            },

            //Demo quest 3
            new QuestInterest()
            {

                QuestId = 3,
                InterestId = 21,
            },

            //Demo quest 4
            new QuestInterest()
            {
                QuestId = 4,
                InterestId = 5,
            },
            new QuestInterest()
            {
                QuestId = 4,
                InterestId = 16,
            }
        };


        public void Configure(EntityTypeBuilder<QuestInterest> entity)
        {
            //Fluent api
          
                entity.HasKey(qi => new { qi.QuestId, qi.InterestId });

                entity.HasOne(qi => qi.Quest)
                    .WithMany(q => q.QuestInterest)
                    .HasForeignKey(qi => qi.QuestId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(qi => qi.Interest)
                    .WithMany(i => i.QuestInterest)
                    .HasForeignKey(qi => qi.InterestId)
                    .OnDelete(DeleteBehavior.Cascade);


            // SEEDING OF DATA
            entity
                .HasData(this.QuestInterests);
        }
    }
}
