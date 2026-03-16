using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.ViewModels.Quest;

namespace Nexus.Controllers
{
    public class QuestController : BaseController
    {
        private readonly IQuestService questService;
        private readonly ILogger<QuestController> logger;
        public QuestController(IQuestService questService, 
            ILogger<QuestController> questLogger)
        {
            this.questService = questService;
            this.logger = questLogger;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<QuestViewModel> allQuestsViewModel = await questService
                .GetAllQuestsOrderByTitleAsync();

            return View(allQuestsViewModel);
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
                return View(questModel);
            }

                string? initiatorId = GetUserId();
            try
            {

               await questService
                    .AddQuestsAndJoinInitiatorAsync(initiatorId!, questModel);

                return RedirectToAction(nameof(All));
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding the quest!");

                ModelState.AddModelError(string.Empty,
                    "Saving changes failed. Please try again later.");
                return View(questModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            string? profileId = GetUserId();
            QuestDetailsViewModel? detailsViewModel = await questService
                .GetQuestDetailsWithJoinersViewModelAsync(profileId!, id);


            if (detailsViewModel == null)
            {
                return NotFound();
            }

            return View(detailsViewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            string? profileId = GetUserId();

            IEnumerable<QuestViewModel> allJoinedQuest = await questService
                .GetAllJoinedQuestsByProfileIdAsync(profileId!);

            return View(allJoinedQuest);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {

            string? profileId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            try
            {
                bool joined = await questService
                    .IsJoinedAsync(profileId, id);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "An error occurred while joining the quest!");
                return BadRequest();
            }
            return RedirectToAction(nameof(Joined));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string? initiatorId = GetUserId();

            QuestAddViewModel questModel = await questService
                .GetQuestToEditViewModelAsync(initiatorId!, id);

            return View(questModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, QuestAddViewModel questModel)
        {
            string? initiatorId = GetUserId();

            if (!ModelState.IsValid)
            {
                return View(questModel);
            }

            try
            {
                await questService.EditQuestAsync(initiatorId!, id, questModel);

                return RedirectToAction(nameof(All));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while editing changes!");
                ModelState.AddModelError(string.Empty, "Saving changes failed. Please try again later.");
                return View(questModel);
            }

          
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string? initiatorId = GetUserId();

            QuestViewModel deleteQuest = await questService
                .GetQuestToDeleteAsync(initiatorId!, id);


            return View(deleteQuest);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            string? initiatorId = GetUserId();

            try
            {
                await questService
                      .ConfirmQuestToDeleteAsync(initiatorId!, id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the quest!");
                ModelState.AddModelError(string.Empty, "Saving changes failed. Please try again later.");
            }
            return RedirectToAction(nameof(All));
        }
    }

}
