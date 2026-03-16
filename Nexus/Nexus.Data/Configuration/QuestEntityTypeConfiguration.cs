using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Data.Models;
using Nexus.Data.Models.Enums;
using Nexus.GCommon.Enums;

namespace Nexus.Data.Configuration
{
    public class QuestEntityTypeConfiguration : IEntityTypeConfiguration<Quest>
    {
        private readonly Quest[] quests =
        {
            new Quest
            {
                Id = 1,
                Title = "Camping near the river",
                Description = "Join us for some fishing, cooking and camping near the river!",
                QuestInitiatorId = "4",
                Difficulty = QuestDifficulty.Hard,
                Status = QuestStatus.Active
            },
            new Quest
            {
                Id = 2,
                Title = "Gaming night",
                Description = "Im looking for people to play cs with so i'd love for us to form a squad!",
                QuestInitiatorId = "3",
                Difficulty = QuestDifficulty.Easy,
                Status = QuestStatus.Active
            },
            new Quest
            {
                Id = 3,
                Title = "Community Work",
                Description = "Let's clean the city!",
                QuestInitiatorId = "1",
                Difficulty = QuestDifficulty.Medium,
                Status = QuestStatus.Active
            },
        };

        public void Configure(EntityTypeBuilder<Quest> entity)
        {

            // SEEDING OF DATA
            entity
                .HasData(this.quests);
        }
    }
}
