using Microsoft.EntityFrameworkCore;
using Nexus.Data.Models;
using Nexus.Data.Services.Core.Helpers;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.GCommon.Enums;
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
            IEnumerable<QuestViewModel> allQuestsVm = await dbContext
              .Quests
              .AsNoTracking()
              .Include(q => q.QuestInterest)
                .ThenInclude(qi => qi.Interest)
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
                  Interests = q.QuestInterest
                      .Select(qi => qi.Interest.Name)
                      .ToList()
              })
              .ToArrayAsync();

            return allQuestsVm;
        }

        public async Task<QuestAddViewModel> GetEmptyQuestAddModelAsync()
        {
            QuestAddViewModel emptyFormModel = new QuestAddViewModel
            {
                AvailableInterests = await GetAllInterestsAsync()
            };

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

            // Add quest interests
            if (questModel.InterestIds != null && questModel.InterestIds.Any())
            {
                foreach (var interestId in questModel.InterestIds.Distinct())
                {
                    await dbContext.QuestInterests.AddAsync(new QuestInterest
                    {
                        Quest = newQuest,
                        InterestId = interestId
                    });
                }
            }

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
                .Include(q => q.QuestInterest)
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
                InterestIds = questFetch.QuestInterest
                    .Select(qi => qi.InterestId)
                    .ToList(),
                AvailableInterests = await GetAllInterestsAsync()
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
                .Include(q => q.QuestInterest)
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

            // Update quest interests
            dbContext.QuestInterests.RemoveRange(questFetch.QuestInterest);

            if (questViewModel.InterestIds != null && questViewModel.InterestIds.Any())
            {
                foreach (var interestId in questViewModel.InterestIds.Distinct())
                {
                    await dbContext.QuestInterests.AddAsync(new QuestInterest
                    {
                        QuestId = questId,
                        InterestId = interestId
                    });
                }
            }

            dbContext.Update(questFetch);
            await dbContext.SaveChangesAsync();
        }

        public async Task<QuestViewModel> GetQuestToDeleteAsync(Guid userId, int questId)
        {

            Quest? fetchQuest = await dbContext
                .Quests
                .Include(q => q.QuestInterest)
                    .ThenInclude(qi => qi.Interest)
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
                Description = fetchQuest.Description,
                Interests = fetchQuest.QuestInterest
                    .Select(qi => qi.Interest.Name)
                    .ToList()
            };

            return questViewModel;
        }

        public async Task ConfirmQuestToDeleteAsync(Guid userId, int questId)
        {
            var questToDelete = await dbContext
                .Quests
                .Include(q => q.QuestInterest)
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

            dbContext.QuestInterests.RemoveRange(questToDelete.QuestInterest);
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
               .Include(q => q.QuestInterest)
                 .ThenInclude(qi => qi.Interest)
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
                Interests = quest.QuestInterest
                    .Select(qi => qi.Interest.Name)
                    .ToList(),
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
                    .ThenInclude(q => q.QuestInitiator)
                .Include(qj => qj.Quest)
                    .ThenInclude(q => q.QuestInterest)
                        .ThenInclude(qi => qi.Interest)
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
                    Status = q.Status,
                    Interests = q.QuestInterest
                        .Select(qi => qi.Interest.Name)
                        .ToList()
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

        public async Task<List<AvailableInterestViewModel>> GetAllInterestsAsync()
        {
            return await dbContext
                .Interests
                .AsNoTracking()
                .Select(i => new AvailableInterestViewModel
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .OrderBy(i => i.Name)
                .ToListAsync();
        }
    }
}