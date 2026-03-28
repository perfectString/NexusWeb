using Microsoft.EntityFrameworkCore;
using Nexus.Data.Models;
using Nexus.Data.Services.Core.Helpers;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.ViewModels.Admin.Profile;

namespace Nexus.Data.Services.Core
{
    public class ProfileManagementService : IProfileManagementService
    {
        private readonly NexusDbContext dbContext;

        public ProfileManagementService(NexusDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> GetAllProfilesCountAsync()
        {
            return await dbContext
                .Users
                .CountAsync();
        }

        public async Task<IEnumerable<ProfileManagementViewModel>> GetAllProfilesAsAdminAsync(int page, int pageSize)
        {
            IEnumerable<ProfileManagementViewModel> allProfilesVm = await dbContext
                .Users
                .AsNoTracking()
                .OrderBy(p => p.DisplayName)
                .ThenBy(p => p.Age)
                .ThenBy(p => p.City)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new ProfileManagementViewModel
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    City = u.City,
                    Bio = u.Bio,
                    ExperiencePoints = u.ExperiencePoints,
                    Level = LevelHelper.GetLevel(u.ExperiencePoints),
                    XpIntoCurrentLevel = LevelHelper.GetXpIntoCurrentLevel(u.ExperiencePoints),
                    XpNeededPerLevel = LevelHelper.GetXpNeededPerLevel(),
                    ProgressPercentage = LevelHelper.GetProgressPercentage(u.ExperiencePoints),
                })
                .ToArrayAsync();

            return allProfilesVm;
        }

        public async Task<ProfileManagementViewModel> GetProfileForEditAsAdminAsync(Guid userId)
        {
            Profile? userFetch = await dbContext
                .Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (userFetch == null)
            {
                throw new ArgumentException("This profile was not found!");
            }

            ProfileManagementViewModel viewModel = new ProfileManagementViewModel()
            {
                Id = userId,
                DisplayName = userFetch.DisplayName,
                City = userFetch.City,
                Bio = userFetch.Bio,
                ExperiencePoints = userFetch.ExperiencePoints,
                Level = LevelHelper.GetLevel(userFetch.ExperiencePoints),
                XpIntoCurrentLevel = LevelHelper.GetXpIntoCurrentLevel(userFetch.ExperiencePoints),
                XpNeededPerLevel = LevelHelper.GetXpNeededPerLevel(),
                ProgressPercentage = LevelHelper.GetProgressPercentage(userFetch.ExperiencePoints),
            };

            return viewModel;
        }

        public async Task EditProfileAsAdminAsync(Guid userId, ProfileManagementViewModel viewModel)
        {
            Profile? userFetch = await dbContext
                .Users
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (userFetch == null)
            {
                throw new ArgumentException("This profile was not found!");
            }

            userFetch.DisplayName = viewModel.DisplayName;
            userFetch.City = viewModel.City;
            userFetch.Bio = viewModel.Bio;
            userFetch.ExperiencePoints = viewModel.ExperiencePoints;
            userFetch.Level = LevelHelper.GetLevel(viewModel.Level);

            await dbContext.SaveChangesAsync();
        }

        private async Task<bool> IsAdminAsync(Guid userId)
        {
            Guid adminRoleId = await dbContext
                .Roles
                .Where(r => r.Name == "Admin")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            return await dbContext
                .UserRoles
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == adminRoleId);
        }

        public async Task DeleteProfileAsAdminAsync(Guid userId)
        {
            if (await IsAdminAsync(userId))
            {
                throw new InvalidOperationException("Cannot delete an admin user!");
            }

            Profile? userFetch = await dbContext
                .Users
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (userFetch == null)
            {
                throw new ArgumentException("This profile was not found!");
            }

            List<ProfileInterest> profileInterests = await dbContext
                .ProfileInterests
                .Where(pi => pi.ProfileId == userId)
                .ToListAsync();
            dbContext.ProfileInterests.RemoveRange(profileInterests);

            List<QuestJoiner> questsJoined = await dbContext
                .QuestJoiners
                .Where(pi => pi.ProfileId == userId)
                .ToListAsync();
            dbContext.QuestJoiners.RemoveRange(questsJoined);

            List<Quest> questsInitiated = await dbContext
                .Quests
                .Include(q => q.QuestInterest)
                .Include(q => q.QuestJoiners)
                .Where(q => q.QuestInitiatorId == userId)
                .ToListAsync();

            foreach (Quest quest in questsInitiated)
            {
                dbContext.QuestInterests.RemoveRange(quest.QuestInterest);
                dbContext.QuestJoiners.RemoveRange(quest.QuestJoiners);
            }
            dbContext.Quests.RemoveRange(questsInitiated);

            dbContext.Users.Remove(userFetch);

            await dbContext.SaveChangesAsync();
        }
    }
}
