using Microsoft.EntityFrameworkCore;
using Nexus.Data.Models;
using Nexus.Data.Services.Core.Helpers;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.GCommon.Enums;
using Nexus.GCommon.Exceptions;
using Nexus.ViewModels.Admin.Quest;
using Nexus.ViewModels.Profile;

namespace Nexus.Data.Services.Core
{
    public class QuestManagementService : IQuestManagementService
    {
    private readonly NexusDbContext dbContext;
    public QuestManagementService(NexusDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

        public async Task<int> GetAllQuestsCountAsync()
        {
            return await dbContext
               .Quests
               .CountAsync();
        }

        public async Task<List<AvailableInterestViewModel>> GetAllInterestsAsync()
        {
            return await dbContext
               .Interests
               .AsNoTracking()
               .Select(i => new AvailableInterestViewModel()
               {
                   Id = i.Id,
                   Name = i.Name,
               })
               .OrderBy(i => i.Name)
               .ToListAsync();
        }

        public async Task<IEnumerable<QuestManagementViewModel>> GetAllQuestsAsAdminAsync(int page, int pageSize)
        {
            List<Quest> allQuests = await dbContext
                .Quests
                .AsNoTracking()
                .Include(q => q.QuestInitiator)
                .Include(q => q.QuestInterest)
                .ThenInclude(q => q.Interest)
                .OrderBy(p => p.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            List<AvailableInterestViewModel> interests = await
                GetAllInterestsAsync();

            return allQuests.Select(q => new QuestManagementViewModel()
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                QuestInitiator = q.QuestInitiator.DisplayName,
                InitiatorId = q.QuestInitiatorId,
                Difficulty = q.Difficulty,
                RewardExperience = q.RewardXp,
                Status = q.Status,
                InterestIds = q.QuestInterest
                .Select(qi => qi.InterestId)
                .ToList(),
                AvailableInterests = interests,
                
            }).ToList();
        }

        public async Task<QuestManagementViewModel> GetQuestToEditAsAdminAsync(int questId)
        {
            Quest? quest = await dbContext
                .Quests
                .Include(q=> q.QuestInitiator)
                .Include(q => q.QuestInterest)
                .Include(q=> q.QuestJoiners)
                .FirstOrDefaultAsync(q=> q.Id == questId);

            if (quest == null)
                throw new EntityNotFoundException();

            var interests = await GetAllInterestsAsync();

            var questViewModel = new QuestManagementViewModel()
            {
                Id = questId,
                Title = quest.Title,
                Description = quest.Description,
                QuestInitiator = quest.QuestInitiator.DisplayName,
                InitiatorId = quest.QuestInitiator.Id,
                Difficulty = quest.Difficulty,
                RewardExperience = quest.RewardXp,
                Status = quest.Status,
                InterestIds = quest.QuestInterest
                .Select(qi => qi.InterestId).ToList(),
                AvailableInterests = interests

            };

            return questViewModel;
        }
        public async Task EditQuestAsAdminAsync(int questId, QuestManagementViewModel model)
        {
            var quest = await dbContext
                .Quests
                .Include(q => q.QuestInterest)
                .Include(q => q.QuestJoiners)
                .ThenInclude(qj => qj.Profile)
                .FirstOrDefaultAsync(q => q.Id == questId);

            if (quest == null)
                throw new EntityNotFoundException();

            if (model.InterestIds == null || model.InterestIds.Count > 3)
                throw new ArgumentException("You can select up to 3 interests.");

            bool wasActive = quest.Status == QuestStatus.Active;
            bool nowCompleted = model.Status == QuestStatus.Completed;

            quest.Title = model.Title;
            quest.Description = model.Description;
            quest.Difficulty = model.Difficulty;
            quest.Status = model.Status;

            quest.RewardXp = QuestRewardHelper.GetRewardXp(model.Difficulty);

            dbContext.QuestInterests.RemoveRange(quest.QuestInterest);

            if (model.InterestIds.Any())
            {
                foreach (var interestId in model.InterestIds.Distinct())
                {
                    await dbContext.QuestInterests.AddAsync(new QuestInterest
                    {
                        QuestId = questId,
                        InterestId = interestId
                    });
                }
            }

            if (wasActive && nowCompleted)
            {
                int rewardXp = quest.RewardXp;
                foreach (var joiner in quest.QuestJoiners)
                {
                    if (joiner.Profile != null)
                    {
                        
                        joiner.Profile.ExperiencePoints += rewardXp;
                        dbContext.Update(joiner.Profile);
                    }
                }
            }

            dbContext.Update(quest);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteQuestAsAdminAsync(int questId)
        {
            var quest = await dbContext
                .Quests
                .Include(q => q.QuestInterest)
                .Include(q => q.QuestJoiners)
                .FirstOrDefaultAsync(q => q.Id == questId);

            if (quest == null)
                throw new EntityNotFoundException();

            dbContext.QuestInterests.RemoveRange(quest.QuestInterest);
            dbContext.QuestJoiners.RemoveRange(quest.QuestJoiners);
            dbContext.Quests.Remove(quest);

            await dbContext.SaveChangesAsync();
        }
    }
}
