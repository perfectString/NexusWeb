using Microsoft.AspNetCore.Mvc;
using Nexus.Data.Services.Core;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.ViewModels.Admin.Quest;

namespace Nexus.Areas.Admin.Controllers
{
    public class QuestManagementController : BaseController
    {
        private const int PageSize = 7;

        private readonly IQuestManagementService questManagementService;
        private readonly ILogger<QuestManagementController> logger;

        public QuestManagementController(IQuestManagementService questManagementService,
            ILogger<QuestManagementController> logger)
        {
            this.questManagementService = questManagementService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            int totalItems = await questManagementService
                .GetAllQuestsCountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            var pagedQuests = await questManagementService.GetAllQuestsAsAdminAsync(page, PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalQuests = totalItems;
            return View(pagedQuests);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            QuestManagementViewModel questModel = await questManagementService
                .GetQuestToEditAsAdminAsync(id);
            return View(questModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, QuestManagementViewModel questModel)
        {

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                logger.LogWarning("ModelState error: {Error}", error.ErrorMessage);
            }

            if (!ModelState.IsValid)
            {
                questModel.AvailableInterests = await questManagementService.GetAllInterestsAsync();
                return View(questModel);
            }

            try
            {
                await questManagementService
                    .EditQuestAsAdminAsync(id, questModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while editing the quest!");
                ModelState.AddModelError(string.Empty, "Saving changes failed. Please try again later.");
                questModel.AvailableInterests = await questManagementService
                    .GetAllInterestsAsync();
                return View(questModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            QuestManagementViewModel questModel = await questManagementService
                .GetQuestToEditAsAdminAsync(id);
            return View(questModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            try
            {
                await questManagementService
                    .DeleteQuestAsAdminAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the quest!");
                ModelState.AddModelError(string.Empty, "Deleting quest failed. Please try again later.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
