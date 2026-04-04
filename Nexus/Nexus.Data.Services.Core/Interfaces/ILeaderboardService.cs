
using Nexus.ViewModels.Leaderboard;

namespace Nexus.Data.Services.Core.Interfaces
{
    public interface ILeaderboardService
    {
        /*Leaderboards*/
        // By Level
        Task<IEnumerable<LeaderboardViewModel>> TopFiveUsersByLevelAsync();

        // By Quests
        Task<IEnumerable<LeaderboardViewModel>> TopFiveUsersByCompletedQuestsAsync();
    }
}

