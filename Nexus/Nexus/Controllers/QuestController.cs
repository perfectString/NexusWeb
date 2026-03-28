using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.ViewModels.Quest;

namespace Nexus.Controllers
{
    public class QuestController : BaseController
    {
        private const int PageSize = 8;

        private readonly IQuestService questService;
        private readonly ILogger<QuestController> logger;
        public QuestController(IQuestService questService, 
            ILogger<QuestController> questLogger)
        {
            this.questService = questService;
            this.logger = questLogger;
        }

        [HttpGet]
        public async Task<IActionResult> All(int page = 1)
        {
            int totalItems = await questService
                .GetAllQuestsCountAsync();

            int totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            page = Math.Clamp(page, 1, Math.Max(totalPages, 1));

            var pagedQuests = await questService
                .GetAllQuestsOrderByTitleAsync(page, PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedQuests);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            QuestAddViewModel questModel = await questService
                .GetEmptyQuestAddModelAsync();

            return View(questModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(QuestAddViewModel questModel)
        {
            if (!ModelState.IsValid)
            {
                questModel.AvailableInterests = await questService.GetAllInterestsAsync();
                return View(questModel);
            }

            Guid initiatorId = GetUserId();
            try
            {

                await questService
                    .AddQuestsAndJoinInitiatorAsync(initiatorId, questModel);

                return RedirectToAction(nameof(All));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding the quest!");

                ModelState.AddModelError(string.Empty,
                    "Saving changes failed. Please try again later.");
                questModel.AvailableInterests = await questService.GetAllInterestsAsync();
                return View(questModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Guid profileId = GetUserId();
            QuestDetailsViewModel? detailsViewModel = await questService
                .GetQuestDetailsWithJoinersViewModelAsync(profileId, id);


            if (detailsViewModel == null)
            {
                return NotFound();
            }

            return View(detailsViewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Joined(int page = 1)
        {
            Guid profileId = GetUserId();

            int totalItems = await questService
                .GetJoinedQuestsCountAsync(profileId);

            int totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            page = Math.Clamp(page, 1, Math.Max(totalPages, 1));

            var pagedQuests = await questService
                .GetAllJoinedQuestsByProfileIdAsync(profileId, page, PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedQuests);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {

            Guid profileId = GetUserId();

            try
            {
                bool joined = await questService
                    .IsJoinedAsync(profileId, id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while joining the quest!");
                return BadRequest();
            }
            return RedirectToAction(nameof(Joined));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Guid initiatorId = GetUserId();

            QuestAddViewModel questModel = await questService
                .GetQuestToEditViewModelAsync(initiatorId, id);

            return View(questModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, QuestAddViewModel questModel)
        {
            Guid initiatorId = GetUserId();

            if (!ModelState.IsValid)
            {
                questModel.AvailableInterests = await questService.GetAllInterestsAsync();
                return View(questModel);
            }

            try
            {
                await questService.EditQuestAsync(initiatorId, id, questModel);

                return RedirectToAction(nameof(All));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while editing changes!");
                ModelState.AddModelError(string.Empty, "Saving changes failed. Please try again later.");
                questModel.AvailableInterests = await questService.GetAllInterestsAsync();
                return View(questModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Guid initiatorId = GetUserId();

            QuestViewModel deleteQuest = await questService
                .GetQuestToDeleteAsync(initiatorId, id);


            return View(deleteQuest);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            Guid initiatorId = GetUserId();

            try
            {
                await questService
                      .ConfirmQuestToDeleteAsync(initiatorId, id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the quest!");
                ModelState.AddModelError(string.Empty, "Saving changes failed. Please try again later.");
            }
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            Guid initiatorId = GetUserId();

            try
            {
                await questService.MarkQuestCompletedAsync(initiatorId, id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while marking the quest as completed!");
                ModelState.AddModelError(string.Empty, "Completing quest failed. Please try again later.");
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
