using Microsoft.AspNetCore.Mvc;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.ViewModels.Profile;

namespace Nexus.Controllers
{
    public class ProfileController : BaseController
    {
        private const int PageSize = 8;

        private readonly IProfileService profileService;
        private readonly ILogger<ProfileController> logger;
        public ProfileController(IProfileService profileService, ILogger<ProfileController> profileLogger)
        {
            this.profileService = profileService;
            this.logger = profileLogger;
        }

        [HttpGet]
        public async Task<IActionResult> All(int page = 1)
        {
            IEnumerable<ProfileViewModel> allProfilesViewModel = await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync();

            int totalItems = allProfilesViewModel.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            page = Math.Clamp(page, 1, Math.Max(totalPages, 1));

            var pagedProfiles = allProfilesViewModel
                .Skip((page - 1) * PageSize)
                .Take(PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedProfiles);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            Guid userId = GetUserId();

            ProfileEditViewModel profileModel = await profileService
                .GetEditProfileViewModelWithAllInterestsAsync(userId);

            return View(profileModel);
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

            Guid userId = GetUserId();

            try
            {
              await profileService
                    .EditProfileAsync(userId!, myProfileViewModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while editing the profile!");
                ModelState.AddModelError(string.Empty, "Saving changes failed. Please try again later.");
                myProfileViewModel.AvailableInterests = await profileService.GetAllInterestsAsync();
                return View(myProfileViewModel);
            }
                return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Guid userId = GetUserId();

            ProfileViewModel profileViewModel = await profileService
                .GetCurrentUserProfile(userId);

            return View(profileViewModel);
        }
    }
}
