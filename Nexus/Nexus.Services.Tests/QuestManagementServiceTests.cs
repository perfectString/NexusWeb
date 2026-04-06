using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Data.Models;
using Nexus.Data.Services.Core;
using Nexus.Data.Services.Core.Helpers;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.GCommon.Enums;
using Nexus.GCommon.Exceptions;
using Nexus.ViewModels.Admin.Quest;

namespace Nexus.Services.Tests
{
    [TestFixture]
    public class QuestManagementServiceTests
    {
        private NexusDbContext dbContext;
        private IQuestManagementService questManagementService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NexusDbContext>()
                .UseInMemoryDatabase("DummyNexusDb" + Guid.NewGuid())
                .Options;

            this.dbContext = new NexusDbContext(options);
            this.questManagementService = new QuestManagementService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        [Test]
        public async Task GetAllQuestsCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            this.dbContext
                .Quests
                .Add(new Quest { Id = 1, Title = "Quest1", Description = "Desc1" });

            this.dbContext
                .Quests
                .Add(new Quest { Id = 2, Title = "Quest2", Description = "Desc2" });

            this.dbContext.SaveChanges();

            // Act
            var count = await questManagementService.GetAllQuestsCountAsync();

            // Assert
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllQuestsCountAsync_WhenNoQuests_ReturnsZero()
        {
            // Act
            var count = await questManagementService.GetAllQuestsCountAsync();

            // Assert
            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllQuestsAsAdminAsync_ReturnsPagedQuests()
        {
            // Arrange
            Profile initiator = new()
            {
                Id = Guid.NewGuid(),
                DisplayName = "Admin",
                City = "None"
            };

            this.dbContext.Users.Add(initiator);
            this.dbContext.SaveChanges();

            for (int i = 1; i <= 7; i++)
            {
                this.dbContext.Quests.Add(new Quest
                {
                    Id = i,
                    Title = $"Quest {i}",
                    Description = $"Desc {i}",
                    QuestInitiatorId = initiator.Id,
                    QuestInitiator = initiator,

                });
            }
            this.dbContext.SaveChanges();

            // Act
            var page1 = (await questManagementService.GetAllQuestsAsAdminAsync(1, 5)).ToList();
            var page2 = (await questManagementService.GetAllQuestsAsAdminAsync(2, 5)).ToList();

            // Assert
            Assert.That(page1.Count, Is.EqualTo(5));
            Assert.That(page2.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllQuestsAsAdminAsync_WhenNoQuests_ReturnsEmpty()
        {
            // Act
            var result = (await questManagementService.GetAllQuestsAsAdminAsync(1, 10)).ToList();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllInterestsAsync_ReturnsAllInterests()
        {
            // Arrange
            this.dbContext.Interests.Add(new Interest { Id = 1, Name = "Chess" });
            this.dbContext.Interests.Add(new Interest { Id = 2, Name = "Coding" });
            this.dbContext.SaveChanges();

            // Act
            var result = await questManagementService.GetAllInterestsAsync();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            CollectionAssert.AreEquivalent(new[] { "Chess", "Coding" }, result.Select(i => i.Name));
        }

        [Test]
        public async Task GetAllInterestsAsync_WhenNoInterests_ReturnsEmptyList()
        {
            // Act
            var result = await questManagementService
                .GetAllInterestsAsync();

            // Assert
            Assert.IsEmpty(result);
        }


        [Test]
        public async Task GetQuestToEditAsAdminAsync_WhenQuestExists_ReturnsQuest()
        {
            // Arrange
            var initiator = new Profile
            {
                Id = Guid.NewGuid(),
                DisplayName = "User",
                City = "None"
            };

            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "Quest1",
                Description = "Desc1",
                QuestInitiator = initiator,
                QuestInitiatorId = initiator.Id
            });
            this.dbContext.SaveChanges();

            // Act
            var result = await questManagementService.GetQuestToEditAsAdminAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Title, Is.EqualTo("Quest1"));
            Assert.That(result.Description, Is.EqualTo("Desc1"));
        }

        [Test]
        public void GetQuestToEditAsAdminAsync_WhenQuestNotFound_ThrowsException()
        {
            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await questManagementService.GetQuestToEditAsAdminAsync(999);
            });
        }

        [Test]
        public async Task EditQuestAsAdminAsync_UpdatesQuest()
        {
            // Arrange
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "Quest1",
                Description = "Desc1"
            });
            this.dbContext.SaveChanges();

            var editModel = new QuestManagementViewModel
            {
                Id = 1,
                Title = "Updated Quest",
                Description = "Updated Desc"
            };

            // Act
            await questManagementService.EditQuestAsAdminAsync(1, editModel);

            // Assert
            var updated = dbContext.Quests.First(q => q.Id == 1);
            Assert.That(updated.Title, Is.EqualTo("Updated Quest"));
            Assert.That(updated.Description, Is.EqualTo("Updated Desc"));
        }

        [Test]
        public void EditQuestAsAdminAsync_WhenQuestNotFound_ThrowsException()
        {
            var editModel = new QuestManagementViewModel
            {
                Id = 999,
                Title = "ShouldNotExist",
                Description = "NoDesc"
            };

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await questManagementService.EditQuestAsAdminAsync(999, editModel);
            });
        }

        [Test]
        public async Task EditQuestAsAdminAsync_WithNoInterests_DoesNotAddQuestInterests()
        {
            // Arrange
            var quest = new Quest
            {
                Id = 1,
                Title = "Quest",
                Description = "Desc",
                QuestInterest = new List<QuestInterest>(),
                QuestJoiners = new List<QuestJoiner>()
            };
            this.dbContext.Quests.Add(quest);
            this.dbContext.SaveChanges();

            var model = new QuestManagementViewModel
            {
                Id = 1,
                Title = "Quest",
                Description = "Desc",
                InterestIds = new List<int>(),
                Status = quest.Status
            };

            // Act
            await questManagementService.EditQuestAsAdminAsync(1, model);

            // Assert
            Assert.IsEmpty(dbContext
                .QuestInterests
                .Where(qi => qi.QuestId == 1));
        }

        [Test]
        public async Task EditQuestAsAdminAsync_WithDuplicateInterests_AddsDistinctQuestInterests()
        {
            // Arrange
            var quest = new Quest
            {
                Id = 2,
                Title = "Quest",
                Description = "Desc",
                QuestInterest = new List<QuestInterest>(),
                QuestJoiners = new List<QuestJoiner>()
            };
            this.dbContext.Quests.Add(quest);
            this.dbContext.SaveChanges();

            var model = new QuestManagementViewModel
            {
                Id = 2,
                Title = "Quest",
                Description = "Desc",
                InterestIds = new List<int> { 1, 1, 2 },
                Status = quest.Status
            };

            // Act
            await questManagementService.EditQuestAsAdminAsync(2, model);

            // Assert
            var interests = dbContext.QuestInterests.Where(qi => qi.QuestId == 2).ToList();
            Assert.That(interests.Count, Is.EqualTo(2));
            CollectionAssert.AreEquivalent(new[] { 1, 2 },
                interests.Select(i => i.InterestId));
        }

        [Test]
        public void EditQuestAsAdminAsync_WithMoreThanThreeInterestIds_ThrowsException()
        {
            // Arrange
            var quest = new Quest
            {
                Id = 11,
                Title = "Quest",
                Description = "Desc"
            };
            this.dbContext.Quests.Add(quest);
            this.dbContext.SaveChanges();

            var model = new QuestManagementViewModel
            {
                Id = 11,
                Title = "Quest",
                Description = "Desc",
                InterestIds = new List<int> { 1, 2, 3, 4 }
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await questManagementService.EditQuestAsAdminAsync(11, model);
            });
        }

        [Test]
        public async Task EditQuestAsAdminAsync_QuestNotActiveOrNotCompleted_DoesNotUpdateJoinerXP()
        {
            // Arrange
            Profile profile = new()
            {
                Id = Guid.NewGuid(),
                DisplayName = "User",
                City = "Null",
                ExperiencePoints = 0
            };
            this.dbContext.Users.Add(profile);
            this.dbContext.SaveChanges();

            Quest quest = new()
            {
                Id = 3,
                Title = "Quest",
                Description = "Desc",
                Status = QuestStatus.Completed,
                QuestJoiners = new List<QuestJoiner> { new QuestJoiner { Profile = profile } }
            };
            this.dbContext.Quests.Add(quest);
            this.dbContext.SaveChanges();

            var model = new QuestManagementViewModel
            {
                Id = 3,
                Title = "Quest",
                Description = "Desc",
                InterestIds = new List<int>(),
                Status = QuestStatus.Completed
            };

            // Act
            await questManagementService.EditQuestAsAdminAsync(3, model);

            // Assert
            Assert.That(profile.ExperiencePoints, Is.EqualTo(0));
        }

        [Test]
        public async Task EditQuestAsAdminAsync_QuestCompleted_OnlyJoinersWithProfileGetXP()
        {
            // Arrange
            Profile profile = new Profile
            {
                Id = Guid.NewGuid(),
                DisplayName = "User",
                City = "Null",
                ExperiencePoints = 0
            };
            this.dbContext.Users.Add(profile);
            this.dbContext.SaveChanges();

            Quest quest = new Quest
            {
                Id = 4,
                Title = "Quest",
                Description = "Desc",
                Status = QuestStatus.Active,
                QuestJoiners = new List<QuestJoiner>
                {
                  new QuestJoiner { Profile = profile }
                }
            };
            this.dbContext.Quests.Add(quest);
            this.dbContext.SaveChanges();

            var model = new QuestManagementViewModel
            {
                Id = 4,
                Title = "Quest",
                Description = "Desc",
                InterestIds = new List<int>(),
                Status = QuestStatus.Completed,
                Difficulty = QuestDifficulty.Easy
            };

            // Act
            await questManagementService.EditQuestAsAdminAsync(4, model);

            // Assert
            Assert.That(profile.ExperiencePoints,
                Is.EqualTo(QuestRewardHelper.GetRewardXp(QuestDifficulty.Easy)));
        }

        [Test]
        public async Task DeleteQuestAsAdminAsync_DeletesQuestAndRelatedData()
        {
            // Arrange
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "Quest1",
                Description = "Desc1",
                QuestInterest = new List<QuestInterest>(),
                QuestJoiners = new List<QuestJoiner>()
            });
            this.dbContext
                .QuestInterests
                .Add(new QuestInterest { QuestId = 1, InterestId = 1 });

            this.dbContext
                .QuestJoiners
                .Add(new QuestJoiner { QuestId = 1, ProfileId = Guid.NewGuid() });

            this.dbContext.SaveChanges();

            // Act
            await questManagementService.DeleteQuestAsAdminAsync(1);

            // Assert
            Assert.IsNull(dbContext.Quests.FirstOrDefault(q => q.Id == 1));
            Assert.IsEmpty(dbContext.QuestInterests.Where(qi => qi.QuestId == 1));
            Assert.IsEmpty(dbContext.QuestJoiners.Where(qj => qj.QuestId == 1));
        }

        [Test]
        public void DeleteQuestAsAdminAsync_WhenQuestNotFound_ThrowsException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await questManagementService.DeleteQuestAsAdminAsync(999);
            });
        }
    }
}
