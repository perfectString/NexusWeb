using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Data.Models;
using Nexus.ViewModels.Profile;
using Nexus.ViewModels.Quest;
using System.Linq;
using Nexus.Data.Services.Core.Interfaces;

namespace Nexus.Controllers
{
    public class QuestController : BaseController
    {
        private readonly NexusDbContext dbContext;
        private readonly IQuestService questService;
        public QuestController(NexusDbContext dbContext, IQuestService questService)
        {
            this.dbContext = dbContext;
            this.questService = questService;

        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var allQuestsViewModel = await questService
                .GetQuestsOrderByTitleAsync();

            return View(allQuestsViewModel);
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
                var initiatorId = GetUserId();

                Quest newQuest = new Quest()
                {

                    Title = questModel.Title,
                    Description = questModel.Description,
                    QuestInitiatorId = initiatorId!
                };


                //since i want the quest initiator to automatically join the quest
                QuestJoiner newJoiner = new QuestJoiner()
                {
                    Quest = newQuest,
                    ProfileId = initiatorId!
                };

                dbContext.Quests.Add(newQuest);
                dbContext.QuestJoiners.Add(newJoiner);
                dbContext.SaveChanges();

                return RedirectToAction(nameof(All));
            }
            catch
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
                    .Select(p => new ProfileViewModel
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
            IEnumerable<QuestViewModel> allJoinedQuest = dbContext
                  .QuestJoiners
                  .Include(qj => qj.Quest)
                  .ThenInclude(qj => qj.QuestInitiator)
                  .AsNoTracking()
                  .Where(qj => qj.ProfileId.ToLower() == initiatorId.ToLower())
                  .Select(qj => qj.Quest)
                  .Select(q => new QuestViewModel()
                  {
                      Id = q.Id,
                      Title = q.Title,
                      Description = q.Description,
                      QuestInitiator = q.QuestInitiator.DisplayName!,
                      InitiatorId = q.QuestInitiatorId
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var initiatorId = GetUserId();

            Quest? editQuest = dbContext
                .Quests
                .FirstOrDefault(q => q.Id == id);

            if (editQuest == null) return NotFound();

            if (editQuest.QuestInitiatorId.ToLower() != initiatorId!.ToLower())
            {
                return Unauthorized();
            }

            QuestViewModel questModel = new QuestViewModel()
            {
                Id = editQuest.Id,
                Title = editQuest.Title,
                Description = editQuest.Description,
                InitiatorId = editQuest.QuestInitiatorId
            };

            return View(questModel);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, QuestViewModel questModel)
        {
            var initiatorId = GetUserId();

            Quest? editQuest = dbContext
                .Quests
                .FirstOrDefault(q => q.Id == id);

            if (editQuest == null) return NotFound();

            if (editQuest.QuestInitiatorId.ToLower() != initiatorId!.ToLower())
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(questModel);
            }

            try
            {
                editQuest.Title = questModel.Title;
                editQuest.Description = questModel.Description;

                dbContext.Quests.Update(editQuest);
                dbContext.SaveChanges();

                return RedirectToAction("Quest/All");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Saving changes failed. Please try again later.");
                return View(questModel);
            }

          
            }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var initiatorId = GetUserId();

            Quest? deleteQuest = dbContext
                .Quests
                .FirstOrDefault(q => q.Id == id);

            if (deleteQuest == null) return NotFound();

            if (deleteQuest.QuestInitiatorId.ToLower() != initiatorId!.ToLower())
            {
                return Unauthorized();
            }

            return View(deleteQuest);
        }


        [HttpPost]
        public IActionResult DeleteConfirm(int id)
        {
            var initiatorId = GetUserId();

            Quest? deleteQuest = dbContext
                .Quests
                .FirstOrDefault(q => q.Id == id);

            if (deleteQuest == null) return NotFound();

            if (deleteQuest.QuestInitiatorId.ToLower() != initiatorId!.ToLower())
            {
                return Unauthorized();
            }

            var joinedUsers = dbContext
                .QuestJoiners
                .Where(q => q.QuestId == id)
                .ToList();

            dbContext.QuestJoiners.RemoveRange(joinedUsers);

            dbContext.Quests.Remove(deleteQuest);

            dbContext.SaveChanges();

            return RedirectToAction(nameof(All));
        }
    }

    }
