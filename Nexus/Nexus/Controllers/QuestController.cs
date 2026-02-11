using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Models;
using Nexus.ViewModels.Profile;
using Nexus.ViewModels.Quest;
using System.Linq;

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
            var allQuests = dbContext
                .Quests
                .AsNoTracking()
                .OrderBy(q => q.Title)
                .Select(q => new QuestAllViewModel()
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    QuestInitiator = q.QuestInitiator.DisplayName,
                    InitiatorId = q.QuestInitiatorId
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

        [HttpGet]
        public IActionResult Details(int id)
        {
            var quest = dbContext
                .Quests
                .AsNoTracking()
                .Include(q => q.QuestJoiners)
                    .ThenInclude(j => j.Profile)
                        .ThenInclude(p => p.ProfileInterest)
                            .ThenInclude(pi => pi.Interest)
                .Include(q => q.QuestInitiator)
                .FirstOrDefault(q => q.Id == id);

            if (quest == null)
            {
                return NotFound();
            }

            var joinedProfiles = (quest.QuestJoiners ?? Enumerable.Empty<QuestJoiner>())
                .Where(jp => jp?.Profile != null)
                .Select(jp => jp.Profile!)
                .ToList();

            QuestDetailsViewModel detailsModel = new QuestDetailsViewModel()
            {
                Id = quest.Id,
                Title = quest.Title,
                Description = quest.Description,
                QuestInitiator = quest.QuestInitiator?.DisplayName ?? string.Empty,
                InitiatorId = quest.QuestInitiatorId,
                JoinedProfiles = joinedProfiles
                    .Select(p => new ProfileAllViewModel
                    {
                        Id = p.Id,
                        DisplayName = p.DisplayName,
                        Age = p.Age,
                        City = p.City,
                        Bio = p.Bio,
                        DesiredConnection = p.DesiredConnection,
                        Interests = (p.ProfileInterest ?? Enumerable.Empty<ProfileInterests>())
                                    .Where(pi => pi.Interest != null)
                                    .Select(pi => pi.Interest.Name)
                                    .ToList()
                    })
                    .ToList()
            };

            return View(detailsModel);

        }

        [HttpGet]
        public IActionResult Joined()
        {
            var initiatorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            IEnumerable<QuestAllViewModel> allJoinedQuest = dbContext
                  .QuestJoiners
                  .Include(qj => qj.Quest)
                  .ThenInclude(qj => qj.QuestInitiator)
                  .AsNoTracking()
                  .Where(qj => qj.ProfileId.ToLower() == initiatorId.ToLower())
                  .Select(q => q.Quest)
                  .Select(q => new QuestAllViewModel()
                  {
                      Id = q.Id,
                      Title = q.Title,
                      Description = q.Description,
                      QuestInitiator = q.QuestInitiator.DisplayName!
                  })
                  .OrderBy(q => q.Title)
                  .ToList();

            return View(allJoinedQuest);
        }

        [HttpPost]
        public IActionResult Joined(int id)
        {
            var questJoin = dbContext
                .Quests
                .Include(q => q.QuestJoiners)
                .FirstOrDefault(q => q.Id == id);

            if (questJoin == null) return BadRequest();

            var profileId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            bool isInitiator = questJoin
                .QuestInitiatorId.ToLower() == profileId.ToLower();
            if (isInitiator) return BadRequest();


            bool isJoined = questJoin
                .QuestJoiners
                .Any(qj => qj.ProfileId.ToLower() == profileId.ToLower());

            if (!isJoined)
            {
                try
                {
                    QuestJoiner newJoiner = new QuestJoiner()
                    {
                        QuestId = questJoin.Id,
                        ProfileId = profileId
                    };

                    dbContext.QuestJoiners.Add(newJoiner);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest();
                }

            }
            return RedirectToAction(nameof(Joined));
        }
    }
}
