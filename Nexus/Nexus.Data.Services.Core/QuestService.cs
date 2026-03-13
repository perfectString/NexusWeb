using Microsoft.EntityFrameworkCore;
using Nexus.Data.Models;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.ViewModels.Profile;
using Nexus.ViewModels.Quest;

namespace Nexus.Data.Services.Core
{

    // i need to update the view model so it displays quest rewards and etc
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
                 InitiatorId = q.QuestInitiatorId
             })
                .ToArrayAsync();

            return allQuestsVm;
        }

        public async Task<QuestAddViewModel> GetEmptyQuestAddModelAsync()
        {
            QuestAddViewModel emptyFormModel = new QuestAddViewModel();

            return emptyFormModel;
        }

        public async Task AddQuestsAndJoinInitiatorAsync(string userId, QuestAddViewModel questModel)
        {

            Profile? userFetch = await dbContext
            .Users
            .SingleOrDefaultAsync(u => u.Id == userId);

            Quest newQuest = new Quest()
            {

                Title = questModel.Title,
                Description = questModel.Description,
                QuestInitiatorId = userFetch!.Id
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

        public async Task<QuestAddViewModel> GetQuestToEditViewModelAsync(string userId, int questId)
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

            if (questFetch.QuestInitiatorId.ToLower() != userFetch.Id!.ToLower())
            {
                throw new ArgumentException("Unauthorized");
            }

            QuestAddViewModel questModel = new QuestAddViewModel()
            {
                Id = questId,
                Title = questFetch.Title,
                Description = questFetch.Description
            };

            return questModel;
        }

        public async Task EditQuestAsync(string userId, int questId, QuestAddViewModel questViewModel)
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

            if (questFetch.QuestInitiatorId.ToLower() != userFetch.Id!.ToLower())
            {
                throw new ArgumentException("Unauthorized");
            }

            questFetch.Title = questViewModel.Title;
            questFetch.Description = questViewModel.Description;

            dbContext.Update(questFetch);
            await dbContext.SaveChangesAsync();
        }

        public async Task<QuestViewModel> GetQuestToDeleteAsync(string userId, int questId)
        { 

            Quest? fetchQuest = await dbContext
                .Quests
                .FirstOrDefaultAsync(q => q.Id == questId);

            if (fetchQuest == null)
            {
                throw new ArgumentException("Quest was not found");
            }

            if (fetchQuest.QuestInitiatorId.ToLower() != userId!.ToLower())
            {
                throw new ArgumentException("You are not the initiator of this quest!");
            }

            QuestViewModel questViewModel = new QuestViewModel()
            {
                Id = fetchQuest.Id,
                Title = fetchQuest.Title,
                Description = fetchQuest.Description
            };

            return questViewModel;
        }

        public async Task ConfirmQuestToDeleteAsync(string userId, int questId)
        {
            var questToDelete = await dbContext
                .Quests
                .FirstOrDefaultAsync(q => q.Id == questId);

            if (questToDelete == null)
            {
                throw new ArgumentException("Quest not found");
            }

            if (questToDelete.QuestInitiatorId.ToLower() != userId!.ToLower())
            {
                throw new ArgumentException("You are not the initiator of this quest!");
            }

            List<QuestJoiner>? joinedUsers = await dbContext
                .QuestJoiners
                .Where(q => q.QuestId == questToDelete.Id)
                .ToListAsync();

           dbContext.QuestJoiners.RemoveRange(joinedUsers);
           dbContext.Quests.Remove(questToDelete);
           await dbContext.SaveChangesAsync();
        }
        public async Task<QuestDetailsViewModel?> GetQuestDetailsWithJoinersViewModelAsync(string userId, int QuestId)
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
                      Interests = (p.ProfileInterest ?? Enumerable.Empty<ProfileInterests>())
                                  .Where(pi => pi.Interest != null)
                                  .Select(pi => pi.Interest.Name)
                                  .ToList()
                  })
                  .ToList()
            };
            return detailsModel;
        }

        public async Task<IEnumerable<QuestViewModel>> GetAllJoinedQuestsByProfileIdAsync(string userId)
        {
            IEnumerable<QuestViewModel> allJoinedQuest = await dbContext
                .QuestJoiners
                .Include(qj => qj.Quest)
                .ThenInclude(qj => qj.QuestInitiator)
                .AsNoTracking()
                .Where(qj => qj.ProfileId.ToLower() == userId.ToLower())
                .Select(qj => qj.Quest)
                .Select(q => new QuestViewModel()
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    QuestInitiator = q.QuestInitiator.DisplayName!,
                    InitiatorId = q.QuestInitiatorId
                })
                .OrderBy(q => q.Title)
                .ToListAsync();

            return allJoinedQuest;
        }

        public async Task<bool> IsJoinedAsync(string userId, int questId)
        {
            var questJoinFetch = await dbContext
              .Quests
              .Include(q => q.QuestJoiners)
              .FirstOrDefaultAsync(q => q.Id == questId);

            if (questJoinFetch == null)
            {
                return false;
            }

            bool isInitiator = questJoinFetch
             .QuestInitiatorId.ToLower() == userId.ToLower();

            bool isJoined = questJoinFetch
                .QuestJoiners
                .Any(qj => qj.ProfileId.ToLower() == userId.ToLower());

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

    }
}
