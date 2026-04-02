using Microsoft.AspNetCore.Mvc;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.ViewModels.Admin.Profile;

namespace Nexus.Areas.Admin.Controllers
{
    public class ProfileManagementController : BaseController
    {
        private const int PageSize = 8;

        private readonly IProfileManagementService profileManagementService;
        private readonly ILogger<ProfileManagementController> logger;

        public ProfileManagementController(
            IProfileManagementService profileManagementService,
            ILogger<ProfileManagementController> logger)
        {
            this.profileManagementService = profileManagementService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            int totalItems = await profileManagementService
                .GetAllProfilesCountAsync();

            int totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            page = Math.Clamp(page, 1, Math.Max(totalPages, 1));

            var pagedProfiles = await profileManagementService
                .GetAllProfilesAsAdminAsync(page, PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalUsers = totalItems;

            return View(pagedProfiles);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            ProfileManagementViewModel profileModel = await profileManagementService
                .GetProfileToEditAsAdminAsync(id);

            return View(profileModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ProfileManagementViewModel profileModel)
        {
            if (!ModelState.IsValid)
            {
                return View(profileModel);
            }

            try
            {
                await profileManagementService
                    .EditProfileAsAdminAsync(id, profileModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while editing the profile!");
                ModelState.AddModelError(string.Empty, "Saving changes failed. Please try again later.");
                return View(profileModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            ProfileManagementViewModel profileModel =
                await profileManagementService
                .GetProfileToEditAsAdminAsync(id);
            return View(profileModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            try
            {
                await profileManagementService
                    .DeleteProfileAsAdminAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the profile!");
                ModelState.AddModelError(string.Empty,
                    "Deleting profile failed. Please try again later.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
