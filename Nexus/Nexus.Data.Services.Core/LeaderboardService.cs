using Microsoft.EntityFrameworkCore;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.ViewModels.Leaderboard;
using Nexus.Data.Services.Core.Helpers;

namespace Nexus.Data.Services.Core
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly NexusDbContext dbContext;

        public LeaderboardService(NexusDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<LeaderboardViewModel>> TopFiveUsersByLevelAsync()
        {
            //Excluding admins from leaderboards
            List<Guid> getAdmins = await GetAdminUserIdsAsync();

            return await dbContext
                .Users
                .Where(u => !getAdmins.Contains(u.Id))
                .OrderByDescending(u => u.ExperiencePoints)
                .ThenBy(u => u.DisplayName)
                .Take(5)
                .Select(u => new LeaderboardViewModel
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    ExperiencePoints = u.ExperiencePoints,
                    Level = LevelHelper.GetLevel(u.ExperiencePoints),
                    CompletedQuests = u.JoinedQuests
                .Count(q => q.Quest.Status == GCommon.Enums.QuestStatus.Completed)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaderboardViewModel>> TopFiveUsersByCompletedQuestsAsync()
        {
            //Excluding admins from leaderboards
            List<Guid> getAdmins = await GetAdminUserIdsAsync();


            return await dbContext
                .Users
                .Where(u=> !getAdmins.Contains(u.Id))
                .Select(u => new LeaderboardViewModel
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    CompletedQuests = u.JoinedQuests
                .Count(u => u.Quest.Status == GCommon.Enums.QuestStatus.Completed)
                })
                .OrderByDescending(u => u.CompletedQuests)
                .ThenBy(u => u.DisplayName)
                .Take(5)
                .ToListAsync();
        }

        private async Task<List<Guid>> GetAdminUserIdsAsync()
        {
            Guid adminRoleId = await dbContext
                .Roles
                .Where(r => r.Name == "Admin")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            return await dbContext
                .UserRoles
                .Where(ur => ur.RoleId == adminRoleId)
                .Select(ur => ur.UserId)
                .ToListAsync();
        }
    }
}