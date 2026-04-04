using Microsoft.AspNetCore.Mvc;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.GCommon.Exceptions;
using Nexus.ViewModels.Admin.Quest;
using static Nexus.GCommon.OutputMessages;

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
            try
            {
                QuestManagementViewModel questModel = await questManagementService
                    .GetQuestToEditAsAdminAsync(id);
                return View(questModel);
            }
            catch (UnauthorizedException ex)
            {
                logger.LogError(ex, UnauthorizedErrorMessage);
                return Unauthorized();
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogError(ex, NotFoundErrorMessage);
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, BadRequestErrorMessage);
                return BadRequest();
            }

            catch (Exception ex)
            {
                logger.LogError(ex, UnexpectedErrorMessage);
                ModelState.AddModelError(string.Empty, SavingChangesFailMessage);
                return View("InternalServerError");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
            catch (UnauthorizedException ex)
            {
                logger.LogError(ex, UnauthorizedErrorMessage);
                return Unauthorized();
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogError(ex, NotFoundErrorMessage);
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, BadRequestErrorMessage);
                return BadRequest();
            }

            catch (Exception ex)
            {
                logger.LogError(ex, UnexpectedErrorMessage);
                ModelState.AddModelError(string.Empty, SavingChangesFailMessage);
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
        [ValidateAntiForgeryToken]
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
