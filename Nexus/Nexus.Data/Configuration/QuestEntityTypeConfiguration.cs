using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexus.Data.Models;
using Nexus.GCommon.Enums;
using static Nexus.Data.Seeding.GuidProfileSeeder;

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
                QuestInitiatorId = User4Id,
                Difficulty = QuestDifficulty.Hard,
                Status = QuestStatus.Active,
                RewardXp = 200
            },
            new Quest
            {
                Id = 2,
                Title = "Gaming night",
                Description = "Im looking for people to play cs with so i'd love for us to form a squad!",
                QuestInitiatorId = User3Id,
                Difficulty = QuestDifficulty.Easy,
                Status = QuestStatus.Active,
                RewardXp = 50
            },
            new Quest
            {
                Id = 3,
                Title = "Community Work",
                Description = "Let's clean the city!",
                QuestInitiatorId = User1Id,
                Difficulty = QuestDifficulty.Medium,
                Status = QuestStatus.Active,
                RewardXp = 125
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
