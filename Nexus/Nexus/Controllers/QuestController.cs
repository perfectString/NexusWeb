using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Models;
using Nexus.ViewModels.Quest;

namespace Nexus.Controllers
{
    [Authorize]
    public class QuestController : Controller
    {
        private readonly NexusDbContext dbContext;

        public QuestController(NexusDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        [HttpGet]
        public IActionResult All()
        {
            IEnumerable<QuestAllViewModel> allQuests = dbContext
                .Quests
                .AsNoTracking()
                .OrderBy(q => q.Title)
                .Select(q => new QuestAllViewModel()
                {
                    Title = q.Title,
                    Description = q.Description,
                    QuestInitiator = q.QuestInitiator.DisplayName
                })
                .ToList();

            return View(allQuests);
        }

        [HttpGet]
        public IActionResult Add()
        {
            QuestAddViewModel questModel = new QuestAddViewModel();

            return View(questModel);
        }

        [HttpPost]
        public IActionResult Add(QuestAddViewModel questModel)
        {
            if (!ModelState.IsValid)
            {
                return View(questModel);
            }

            try
            {
                Quest newQuest = new Quest()
                {
                    Title = questModel.Title,
                    Description = questModel.Description,
                    QuestInitiatorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!
                };

                dbContext.Quests.Add(newQuest);
                dbContext.SaveChanges();

                return RedirectToAction(nameof(All));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Saving changes failed. Please try again later.");
                return View(questModel);
            }
        }
    }
}
