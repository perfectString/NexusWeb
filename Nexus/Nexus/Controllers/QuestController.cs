using System.Security.Claims;
using System.Text;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nexus.Data.Models;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.GCommon.Exceptions;
using Nexus.ViewModels.Quest;
using static Nexus.GCommon.OutputMessages;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(QuestAddViewModel questModel)
        {
            if (!ModelState.IsValid)
            {
                questModel.AvailableInterests = await questService.GetAllInterestsAsync();
                return View(questModel);
            }

            Guid initiatorId = GetUserId();


            if (initiatorId == Guid.Empty)
            {
                logger.LogError(BadRequestErrorMessage);
                return BadRequest();
            }
            try
            {

                await questService
                    .AddQuestsAndJoinInitiatorAsync(initiatorId, questModel);

                return RedirectToAction(nameof(All));
            }
            catch (EntityFailureException ex)
            {
                logger.LogError(ex, AddQuestFailedMessage);

                ModelState.AddModelError(string.Empty,
                    AddQuestFailedMessage);
                questModel.AvailableInterests = await questService.GetAllInterestsAsync();
                return View(questModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, UnexpectedErrorMessage);

                ModelState.AddModelError(string.Empty,
                    UnexpectedErrorMessage);
                questModel.AvailableInterests = await questService.GetAllInterestsAsync();
                return View(questModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            
            Guid profileId = GetUserId();

            if (profileId == Guid.Empty)
            {
                logger.LogError(BadRequestErrorMessage);
                return BadRequest();
            }

            try
            {

                QuestDetailsViewModel? detailsViewModel = await questService
                   .GetQuestDetailsWithJoinersViewModelAsync(profileId, id);
                return View(detailsViewModel);

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
            }

            return RedirectToAction(nameof(Details), new{ id });
        }

        [HttpGet]
        public async Task<IActionResult> Joined(int page = 1)
        {
            Guid profileId = GetUserId();

            if (profileId == Guid.Empty)
            {
                logger.LogError(BadRequestErrorMessage);
                return BadRequest();
            }

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id)
        {

            Guid profileId = GetUserId();

            if (profileId == Guid.Empty)
            {
                logger.LogError(BadRequestErrorMessage);
                return BadRequest();
            }

            try
            {
                bool joined = await questService
                    .IsJoinedAsync(profileId, id);
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
            }
                return RedirectToAction(nameof(Joined));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Guid initiatorId = GetUserId();
            if (initiatorId == Guid.Empty)
            {
                logger.LogError(BadRequestErrorMessage);
                return BadRequest();
            }

            try
            {

            QuestAddViewModel questModel = await questService
                .GetQuestToEditViewModelAsync(initiatorId, id);
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
        public async Task<IActionResult> Edit([FromRoute] int id, QuestAddViewModel questModel)
        {
            Guid initiatorId = GetUserId();
            if (initiatorId == Guid.Empty)
            {
                logger.LogError(BadRequestErrorMessage);
                return BadRequest();
            }

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
            catch(EntityNotFoundException ex)
            {
                logger.LogError(ex, NotFoundErrorMessage);
                return NotFound();
            }
            catch (UnauthorizedException ex)
            {
                logger.LogError(ex, UnauthorizedErrorMessage);
                return Unauthorized();
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, BadRequestErrorMessage);
                return BadRequest();
            }
            catch (EntityFailureException ex)
            {
                logger.LogError(ex, string.Format(CrudExceptionMessage, nameof(Edit)));
                ModelState.AddModelError(string.Empty, string.Format(CrudExceptionMessage,
                    "edit the quest"));
                questModel.AvailableInterests = await questService.GetAllInterestsAsync();
                return View(questModel);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, UnexpectedErrorMessage);
                ModelState.AddModelError(string.Empty, SavingChangesFailMessage);
                return View("InternalServerError");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Guid initiatorId = GetUserId();
            if (initiatorId == Guid.Empty)
            {
                logger.LogError(BadRequestErrorMessage);
                return BadRequest();
            }

            try
            {
            QuestViewModel deleteQuest = await questService
                .GetQuestToDeleteAsync(initiatorId, id);
            return View(deleteQuest);

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
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            Guid initiatorId = GetUserId();
            if (initiatorId == Guid.Empty)
            {
                logger.LogError(BadRequestErrorMessage);
                return BadRequest();
            }

            try
            {
                await questService
                      .ConfirmQuestToDeleteAsync(initiatorId, id);
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogError(ex, NotFoundErrorMessage);
                return NotFound();
            }
            catch (EntityFailureException ex)
            {
                logger.LogError(ex, string.Format(CrudExceptionMessage, nameof(Delete)));
                ModelState.AddModelError(string.Empty, string.Format(CrudExceptionMessage,
                    "deleting the quest"));

            }
            catch (UnauthorizedException ex)
            {
                logger.LogError(ex, UnauthorizedErrorMessage);
                return Unauthorized();
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
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            Guid initiatorId = GetUserId();
            if (initiatorId == Guid.Empty)
            {
                logger.LogError(BadRequestErrorMessage);
                return BadRequest();
            }

            try
            {
                await questService.MarkQuestCompletedAsync(initiatorId, id);
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogError(ex,NotFoundErrorMessage);
                return NotFound();
            }
            catch (EntityFailureException ex)
            {
                logger.LogError(ex, string.Format(CrudExceptionMessage, nameof(Complete)));
                ModelState.AddModelError(string.Empty, string.Format(CrudExceptionMessage,
                    "mark quest as complete"));

            }
            catch (UnauthorizedException ex)
            {
                logger.LogError(ex, UnauthorizedErrorMessage);
                return Unauthorized();
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


            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
