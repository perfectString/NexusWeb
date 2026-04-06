using Nexus.ViewModels.Leaderboard;

namespace Nexus.ViewModels.Home
{
    public class HomeViewModel
    {
        public IEnumerable<LeaderboardViewModel> TopByLevel { get; set; } = [];
        public IEnumerable<LeaderboardViewModel> TopByQuests { get; set; } = [];
    }
}