using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.ViewModels;
using Nexus.ViewModels.Home;

namespace Nexus.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> logger;
        private readonly ILeaderboardService leaderboardService;

        public HomeController(ILogger<HomeController> homeLogger, ILeaderboardService leaderboardService)
        {
            this.logger = homeLogger;
            this.leaderboardService = leaderboardService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                HomeViewModel homeViewModel = new HomeViewModel
                {
                    TopByLevel = await leaderboardService.TopFiveUsersByLevelAsync(),
                    TopByQuests = await leaderboardService.TopFiveUsersByCompletedQuestsAsync()
                };

                return View(homeViewModel);
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Home/Error/{statusCode}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == StatusCodes.Status404NotFound)
            {
                return View("NotFound");
            }
            if (statusCode == StatusCodes.Status400BadRequest)
            {
                return View("BadRequest");
            }
            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                return View("InternalServerError");
            }
            if (statusCode == StatusCodes.Status401Unauthorized)
            {
                return View("Unauthorized");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
