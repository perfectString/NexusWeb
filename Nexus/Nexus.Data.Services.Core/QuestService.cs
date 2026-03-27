using Microsoft.EntityFrameworkCore;
using Nexus.Data.Models;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.GCommon.Enums;
using Nexus.Data.Services.Core.Helpers;
using Nexus.ViewModels.Profile;
using Nexus.ViewModels.Quest;

namespace Nexus.Data.Services.Core
{
    public class QuestService : IQuestService
    {

        private readonly NexusDbContext dbContext;
        public QuestService(NexusDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<QuestViewModel>> GetAllQuestsOrderByTitleAsync()
        {
            IQueryable<Quest> fetchQuestsQuery = dbContext
              .Quests
              .AsNoTracking();

            IEnumerable<QuestViewModel> allQuestsVm = await fetchQuestsQuery
             .OrderBy(q => q.Title)
             .Select(q => new QuestViewModel()
             {
                 Id = q.Id,
                 Title = q.Title,
                 Description = q.Description,
                 QuestInitiator = q.QuestInitiator.DisplayName,
                 InitiatorId = q.QuestInitiatorId,
                 Difficulty = q.Difficulty,
                 RewardExperience = q.RewardXp,
                 Status = q.Status,
             })
                .ToArrayAsync();

            return allQuestsVm;
        }

        public async Task<QuestAddViewModel> GetEmptyQuestAddModelAsync()
        {
            QuestAddViewModel emptyFormModel = new QuestAddViewModel();

            return emptyFormModel;
        }

        public async Task AddQuestsAndJoinInitiatorAsync(Guid userId, QuestAddViewModel questModel)
        {

            Profile? userFetch = await dbContext
            .Users
            .SingleOrDefaultAsync(u => u.Id == userId);

            Quest newQuest = new Quest()
            {

                Title = questModel.Title,
                Description = questModel.Description,
                QuestInitiatorId = userFetch!.Id,
                Difficulty = questModel.Difficulty,
                RewardXp = QuestRewardHelper.GetRewardXp(questModel.Difficulty),
                Status = QuestStatus.Active,
            };


            //since i want the quest initiator to automatically join the quest
            QuestJoiner newJoiner = new QuestJoiner()
            {
                Quest = newQuest,
                ProfileId = userFetch.Id
            };

            await dbContext.Quests.AddAsync(newQuest);
            await dbContext.QuestJoiners.AddAsync(newJoiner);
            await dbContext.SaveChangesAsync();
        }

        public async Task<QuestAddViewModel> GetQuestToEditViewModelAsync(Guid userId, int questId)
        {
            Profile? userFetch = await dbContext
               .Users
               .AsNoTracking()
               .SingleOrDefaultAsync(u => u.Id == userId);

            Quest? questFetch = await dbContext
                .Quests
                .AsNoTracking()
                .SingleOrDefaultAsync(q => q.Id == questId);


            if (userFetch == null || questFetch == null)
            {
                throw new ArgumentException("Not found.");
            }

            if (questFetch.QuestInitiatorId != userFetch.Id)
            {
                throw new ArgumentException("Unauthorized");
            }

            if (questFetch.Status == QuestStatus.Completed)
            {
                throw new InvalidOperationException("Cannot edit a completed quest.");
            }

            QuestAddViewModel questModel = new QuestAddViewModel()
            {
                Id = questId,
                Title = questFetch.Title,
                Description = questFetch.Description,
                Difficulty = questFetch.Difficulty,
                
            };

            return questModel;
        }

        public async Task EditQuestAsync(Guid userId, int questId, QuestAddViewModel questViewModel)
        {
            Profile? userFetch = await dbContext
              .Users
              .AsNoTracking()
              .SingleOrDefaultAsync(u => u.Id == userId);

            Quest? questFetch = await dbContext
                .Quests
                .SingleOrDefaultAsync(q => q.Id == questId);

            if (userFetch == null || questFetch == null)
            {
                throw new ArgumentException("Not found.");
            }

            if (questFetch.QuestInitiatorId != userFetch.Id)
            {
                throw new ArgumentException("Unauthorized");
            }

            if (questFetch.Status == QuestStatus.Completed)
            {
                throw new InvalidOperationException("Cannot edit a completed quest.");
            }

            questFetch.Title = questViewModel.Title;
            questFetch.Description = questViewModel.Description;

            dbContext.Update(questFetch);
            await dbContext.SaveChangesAsync();
        }

        public async Task<QuestViewModel> GetQuestToDeleteAsync(Guid userId, int questId)
        { 

            Quest? fetchQuest = await dbContext
                .Quests
                .FirstOrDefaultAsync(q => q.Id == questId);

            if (fetchQuest == null)
            {
                throw new ArgumentException("Quest was not found");
            }

            if (fetchQuest.QuestInitiatorId != userId)
            {
                throw new ArgumentException("You are not the initiator of this quest!");
            }

            if (fetchQuest.Status == QuestStatus.Completed)
            {
                throw new InvalidOperationException("Cannot delete a completed quest.");
            }

            QuestViewModel questViewModel = new QuestViewModel()
            {
                Id = fetchQuest.Id,
                Title = fetchQuest.Title,
                Description = fetchQuest.Description
            };

            return questViewModel;
        }

        public async Task ConfirmQuestToDeleteAsync(Guid userId, int questId)
        {
            var questToDelete = await dbContext
                .Quests
                .FirstOrDefaultAsync(q => q.Id == questId);

            if (questToDelete == null)
            {
                throw new ArgumentException("Quest not found");
            }

            if (questToDelete.QuestInitiatorId != userId)
            {
                throw new ArgumentException("You are not the initiator of this quest!");
            }

            if (questToDelete.Status == QuestStatus.Completed)
            {
                throw new InvalidOperationException("Cannot delete a completed quest.");
            }

            List<QuestJoiner>? joinedUsers = await dbContext
                .QuestJoiners
                .Where(q => q.QuestId == questToDelete.Id)
                .ToListAsync();

           dbContext.QuestJoiners.RemoveRange(joinedUsers);
           dbContext.Quests.Remove(questToDelete);
           await dbContext.SaveChangesAsync();
        }
        public async Task<QuestDetailsViewModel?> GetQuestDetailsWithJoinersViewModelAsync(Guid userId, int QuestId)
        {
            Quest? quest = await dbContext
               .Quests
               .AsNoTracking()
               .Include(q => q.QuestJoiners)
                 .ThenInclude(j => j.Profile)
                   .ThenInclude(p => p.ProfileInterest)
                     .ThenInclude(pi => pi.Interest)
               .Include(q => q.QuestInitiator)
               .FirstOrDefaultAsync(q => q.Id == QuestId);

            if (quest == null)
            {
                throw new ArgumentException("NotFound");
            }

            List<Profile>? joinedProfiles = (quest.QuestJoiners ?? Enumerable.Empty<QuestJoiner>())
                .Where(jp => jp?.Profile != null)
                .Select(jp => jp.Profile!)
                .ToList();

            QuestDetailsViewModel detailsModel = new QuestDetailsViewModel()
            {
                Id = quest.Id,
                Title = quest.Title,
                Description = quest.Description,
                Difficulty = quest.Difficulty,
                RewardExperience = quest.RewardXp,
                Status = quest.Status,
                QuestInitiator = quest.QuestInitiator?.DisplayName ?? string.Empty,
                InitiatorId = quest.QuestInitiatorId,
                JoinedProfiles = joinedProfiles
                  .Select(p => new ProfileViewModel
                  {
                      Id = p.Id,
                      DisplayName = p.DisplayName,
                      Age = p.Age,
                      City = p.City,
                      Bio = p.Bio,
                      DesiredConnection = p.DesiredConnection,
                      Interests = (p.ProfileInterest ?? Enumerable.Empty<ProfileInterest>())
                                  .Where(pi => pi.Interest != null)
                                  .Select(pi => pi.Interest.Name)
                                  .ToList()
                  })
                  .ToList()
            };
            return detailsModel;
        }

        public async Task<IEnumerable<QuestViewModel>> GetAllJoinedQuestsByProfileIdAsync(Guid userId)
        {
            IEnumerable<QuestViewModel> allJoinedQuest = await dbContext
                .QuestJoiners
                .Include(qj => qj.Quest)
                .ThenInclude(qj => qj.QuestInitiator)
                .AsNoTracking()
                .Where(qj => qj.ProfileId == userId)
                .Select(qj => qj.Quest)
                .Select(q => new QuestViewModel()
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    QuestInitiator = q.QuestInitiator.DisplayName!,
                    InitiatorId = q.QuestInitiatorId,
                    Difficulty = q.Difficulty,
                    RewardExperience = q.RewardXp,
                    Status = q.Status
                })
                .OrderBy(q => q.Title)
                .ToListAsync();

            return allJoinedQuest;
        }

        public async Task<bool> IsJoinedAsync(Guid userId, int questId)
        {
            var questJoinFetch = await dbContext
              .Quests
              .Include(q => q.QuestJoiners)
              .FirstOrDefaultAsync(q => q.Id == questId);

            if (questJoinFetch == null)
            {
                return false;
            }

            bool isInitiator = questJoinFetch.QuestInitiatorId == userId;

            bool isJoined = questJoinFetch
                .QuestJoiners
                .Any(qj => qj.ProfileId == userId);

            if (isInitiator || isJoined)
            {
                return false;
            }

            QuestJoiner newJoiner = new QuestJoiner()
            {
                QuestId = questJoinFetch.Id,
                ProfileId = userId
            };

           await dbContext.QuestJoiners.AddAsync(newJoiner);
           await dbContext.SaveChangesAsync();

           return true;

        }

        public async Task MarkQuestCompletedAsync(Guid userId, int questId)
        {

            var quest = await dbContext
                .Quests
                .Include(q => q.QuestJoiners)
                    .ThenInclude(j => j.Profile)
                .FirstOrDefaultAsync(q => q.Id == questId);

            if (quest == null)
            {
                throw new ArgumentException("Quest not found.");
            }

            if (quest.QuestInitiatorId != userId)
            {
                throw new ArgumentException("You are not the initiator of this quest!");
            }

            if (quest.Status == QuestStatus.Completed)
            {
                throw new InvalidOperationException("Quest is already completed.");
            }

            quest.Status = QuestStatus.Completed;

            // award XP to all joined users (including initiator)
            var reward = quest.RewardXp;

            var joinedProfiles = quest.QuestJoiners?
                .Where(j => j?.Profile != null)
                .Select(j => j.Profile!)
                .ToList() ?? new List<Profile>();

            foreach (var profile in joinedProfiles)
            {
                profile.ExperiencePoints += reward;
                dbContext.Update(profile);
            }

            dbContext.Update(quest);
            await dbContext.SaveChangesAsync();
        }

    }
}
