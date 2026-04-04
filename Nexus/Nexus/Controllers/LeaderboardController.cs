using Microsoft.AspNetCore.Mvc;
using Nexus.Data.Services.Core.Interfaces;

namespace Nexus.Controllers
{
    public class LeaderboardController : BaseController
    {
        private readonly ILeaderboardService leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            this.leaderboardService = leaderboardService;
        }

        [HttpGet]
        public async Task<IActionResult> LevelLeaderboard()
        {
            var users = await leaderboardService.TopFiveUsersByLevelAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> QuestLeaderboard()
        {
            var users = await leaderboardService.TopFiveUsersByCompletedQuestsAsync();
            return View(users);
        }
    }
}
