using Microsoft.AspNetCore.Mvc;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.ViewModels.Profile;

namespace Nexus.Controllers
{
    public class ProfileController : BaseController
    {

        private readonly IProfileService profileService;
        private readonly ILogger<ProfileController> logger;
        public ProfileController(IProfileService profileService, ILogger<ProfileController> profileLogger)
        {
            this.profileService = profileService;
            this.logger = profileLogger;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<ProfileViewModel> allProfilesViewModel = await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync();

            return View(allProfilesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            string? userId = GetUserId();


            ProfileEditViewModel myProfileViewModel = await profileService
                .GetEditProfileViewModelWithAllInterestsAsync(userId!);

            return View(myProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileEditViewModel myProfileViewModel)
        {
            if (myProfileViewModel.InterestId.Count > 3)
            {
                logger.LogError("An error occurred while editing the profile!");
                ModelState.AddModelError(
                    nameof(myProfileViewModel.InterestId),
                    "You can select up to 3 interests."
                );
            }
            if (!ModelState.IsValid)
            {
                myProfileViewModel.AvailableInterests = await profileService
                    .GetAllInterestsAsync();
                return View(myProfileViewModel);
            }

            string? userId = GetUserId();

            try
            {
              await profileService
                    .EditProfileAsync(userId!, myProfileViewModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while editing the profile!");
                Console.WriteLine(ex);
            }

                return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string? userId = GetUserId();


            ProfileViewModel myProfileViewModel = await profileService
                .GetCurrentUserProfile(userId!);

            return View(myProfileViewModel);
        }

    }
}
