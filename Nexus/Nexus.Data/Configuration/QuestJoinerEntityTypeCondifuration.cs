using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Data.Models;

namespace Nexus.Data.Configuration
{
    public class QuestJoinersEntityTypeCondifuration : IEntityTypeConfiguration<QuestJoiner>
    {
    


        public void Configure(EntityTypeBuilder<QuestJoiner> entity)
        {
            //FLUENT API
            //Control of navigational properties for participants of the quests

          

            entity
               .HasOne(qj => qj.Quest)
               .WithMany(qj => qj.QuestJoiners)
               .HasForeignKey(qj => qj.QuestId)
               .OnDelete(DeleteBehavior.Cascade);

            entity
               .HasOne(qj => qj.Profile)
               .WithMany(qj => qj.JoinedQuests)
               .HasForeignKey(qj => qj.ProfileId)
               .OnDelete(DeleteBehavior.NoAction);


        }
    }
    }
