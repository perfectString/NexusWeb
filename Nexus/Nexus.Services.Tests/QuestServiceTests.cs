using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Data.Models;
using Nexus.Data.Services.Core;
using Nexus.GCommon.Enums;
using Nexus.GCommon.Exceptions;
using Nexus.ViewModels.Quest;


namespace Nexus.Services.Tests
{
    [TestFixture]
    public class QuestServiceTests
    {
        private NexusDbContext dbContext;
        private QuestService questService;
        private static readonly Guid initiatorId = Guid.Parse("68320c63-0ac9-40fb-ba2d-b69d63e43edd");
        private static readonly Guid otherUserId = Guid.Parse("eac88e60-c19d-4e5c-9dce-780846aafd37");

        [SetUp]
        public void Setup()
        {
            //Arrange for most test cases
            var options = new DbContextOptionsBuilder<NexusDbContext>()
                .UseInMemoryDatabase("DummyNexusDb" + Guid.NewGuid())
                .Options;

            this.dbContext = new NexusDbContext(options);

            this.dbContext.Users.Add(new Profile { Id = initiatorId, DisplayName = "InitiatorDummy", City = "NightCity" });
            this.dbContext.Users.Add(new Profile { Id = otherUserId, DisplayName = "UserDummy", City = "Los Santos" });

            this.dbContext.SaveChanges();

            questService = new QuestService(dbContext);
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
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.Quests.Add(new Quest
            {
                Id = 2,
                Title = "B Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            // Act
            var count = await questService.GetAllQuestsCountAsync();

            // Assert
            Assert.AreEqual(2, count);
        }

        [Test]
        public async Task GetAllQuestsCountAsync_WhenNoQuests_ReturnsZero()
        {
            // Arrange

            // Act
            var count = await questService.GetAllQuestsCountAsync();

            // Assert
            Assert.AreEqual(0, count);
        }

        [Test]
        public async Task GetAllQuestsOrderByTitleAsync_ReturnsQuestsOrderedByTitle()
        {
            // Arrange
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "B Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.Quests.Add(new Quest
            {
                Id = 2,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            // Act
            var result = (await questService
                .GetAllQuestsOrderByTitleAsync(1, 10)).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            var ordered = result.OrderBy(q => q.Title).ToList();
            CollectionAssert.AreEqual(ordered, result);
        }

        [Test]
        public async Task GetAllQuestsOrderByTitleAsync_Pagination_Works()
        {
            // Arrange
            for (int i = 1; i <= 8; i++)
            {
                this.dbContext.Quests.Add(new Quest
                {
                    Id = i,
                    Title = $"Quest {i}",
                    Description = "Desc",
                    QuestInitiatorId = initiatorId,
                    QuestInterest = new List<QuestInterest>()
                });
            }
            this.dbContext.SaveChanges();

            // Act
            var page1 = (await questService
                .GetAllQuestsOrderByTitleAsync(1, 5))
                .ToList();

            var page2 = (await questService
                .GetAllQuestsOrderByTitleAsync(2, 5))
                .ToList();

            // Assert
            Assert.AreEqual(5, page1.Count);
            Assert.AreEqual(3, page2.Count);
        }

        [Test]
        public async Task GetAllQuestsOrderByTitleAsync_WhenNoQuests_ReturnsEmpty()
        {
            // Arrange

            // Act
            var result = (await questService.GetAllQuestsOrderByTitleAsync(1, 10)).ToList();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetEmptyQuestAddModelAsync_ReturnsAllAvailableInterests()
        {
            // Arrange
            this.dbContext.Interests.Add(new Interest { Id = 1, Name = "Chess" });
            this.dbContext.Interests.Add(new Interest { Id = 2, Name = "Coding" });
            this.dbContext.SaveChanges();

            // Act
            var result = await questService.GetEmptyQuestAddModelAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.AvailableInterests);
            Assert.AreEqual(2, result.AvailableInterests.Count);
            CollectionAssert.AreEquivalent(
                new[] { "Chess", "Coding" },
                result.AvailableInterests.ConvertAll(i => i.Name)
            );
        }

        [Test]
        public async Task GetEmptyQuestAddModelAsync_WhenNoInterests_ReturnsEmptyList()
        {
            // Arrange: No interests added

            // Act
            var result = await questService.GetEmptyQuestAddModelAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.AvailableInterests);
            Assert.IsEmpty(result.AvailableInterests);
        }

        [Test]
        public async Task AddQuestsAndJoinInitiatorAsync_CreatesQuestWithInterests_InitiatorShouldBeJoined()
        {
            // Arrange
            this.dbContext.Interests.Add(new Interest { Id = 1, Name = "Chess" });
            this.dbContext.Interests.Add(new Interest { Id = 2, Name = "Coding" });
            this.dbContext.SaveChanges();

            QuestAddViewModel questModel = new()
            {
                Title = "New Quest",
                Description = "Test Desc",
                Difficulty = QuestDifficulty.Medium,
                InterestIds = new List<int> { 1, 2 }
            };

            // Act
            await questService.AddQuestsAndJoinInitiatorAsync(initiatorId, questModel);

            // Assert
            var quest = dbContext
                .Quests
                .FirstOrDefault(q => q.Title == "New Quest");

            Assert.IsNotNull(quest);
            Assert.AreEqual("Test Desc", quest.Description);
            Assert.AreEqual(QuestDifficulty.Medium, quest.Difficulty);

            var joiner = dbContext
                .QuestJoiners
                .FirstOrDefault(j => j.QuestId == quest.Id && j.ProfileId == initiatorId);

            Assert.IsNotNull(joiner);

            var questInterests = dbContext.QuestInterests.Where(qi => qi.QuestId == quest.Id).ToList();
            Assert.AreEqual(2, questInterests.Count);
            CollectionAssert.AreEquivalent(new[] { 1, 2 }, questInterests.Select(qi => qi.InterestId));
        }

        [Test]
        public async Task AddQuestsAndJoinInitiatorAsync_CreatesQuestWithNoInterests()
        {
            // Arrange
            var questModel = new QuestAddViewModel
            {
                Title = "No Interest Quest",
                Description = "No interests",
                Difficulty = QuestDifficulty.Easy,
                InterestIds = new List<int>()
            };

            // Act
            await questService.AddQuestsAndJoinInitiatorAsync(initiatorId, questModel);

            // Assert
            var quest = dbContext.Quests.FirstOrDefault(q => q.Title == "No Interest Quest");
            Assert.IsNotNull(quest);

            var questInterests = dbContext.QuestInterests.Where(qi => qi.QuestId == quest.Id).ToList();
            Assert.IsEmpty(questInterests);
        }

        [Test]
        public void AddQuestsAndJoinInitiatorAsync_WhenUserNotFound_ThrowsException()
        {
            // Arrange
            var questModel = new QuestAddViewModel
            {
                Title = "Should Fail",
                Description = "No user",
                Difficulty = QuestDifficulty.Easy,
                InterestIds = new List<int>()
            };

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await questService.AddQuestsAndJoinInitiatorAsync(Guid.NewGuid(), questModel);
            });
        }

        [Test]
        public async Task GetQuestToEditViewModelAsync_ReturnsCorrectModel()
        {
            // Arrange
            this.dbContext.Interests.Add(new Interest { Id = 10, Name = "TestInterest" });
            this.dbContext.SaveChanges();

            this.dbContext.QuestInterests.Add(new QuestInterest { QuestId = 1, InterestId = 10 });
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            // Act
            var result = await questService
                .GetQuestToEditViewModelAsync(initiatorId, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("A Quest", result.Title);
            Assert.AreEqual("Description", result.Description);
            Assert.AreEqual(QuestDifficulty.Easy, result.Difficulty);
            Assert.That(result.InterestIds, Is.EquivalentTo(new[] { 10 }));
            Assert.That(result.AvailableInterests.Any(i => i.Id == 10 && i.Name == "TestInterest"));
        }

        [Test]
        public void GetQuestToEditViewModelAsync_IfUserNotFound_ThrowsException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await questService.GetQuestToEditViewModelAsync(Guid.NewGuid(), 1);
            });
        }

        [Test]
        public void GetQuestToEditViewModelAsync_IfQuestNotFound_ThrowsException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await questService.GetQuestToEditViewModelAsync(initiatorId, 999);
            });
        }

        [Test]
        public void GetQuestToEditViewModelAsync_IfUserNotInitiator_ThrowsException()
        {
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            Assert.ThrowsAsync<UnauthorizedException>(async () =>
            {
                await questService.GetQuestToEditViewModelAsync(otherUserId, 1);
            });
        }

        [Test]
        public async Task GetQuestToEditViewModelAsync_IfQuestCompleted_ThrowsException()
        {
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                Status = QuestStatus.Completed,
                QuestInterest = new List<QuestInterest>()
            });
            dbContext.SaveChanges();

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await questService.GetQuestToEditViewModelAsync(initiatorId, 1);
            });
            Assert.That(ex.Message, Is.EqualTo(GCommon.OutputMessages.CompletedQuestFailedMessage));
        }

        [Test]
        public async Task EditQuestAsync_UpdatesQuestAndInterests()
        {
            // Arrange
            this.dbContext.Interests.Add(new Interest { Id = 2, Name = "Coding" });
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            var editModel = new QuestAddViewModel
            {
                Title = "Updated Title",
                Description = "Updated Desc",
                Difficulty = QuestDifficulty.Hard,
                InterestIds = new List<int> { 2 }
            };

            // Act
            await questService
                .EditQuestAsync(initiatorId, 1, editModel);

            // Assert
            var quest = dbContext
                .Quests
                .Include(q => q.QuestInterest)
                .First(q => q.Id == 1);

            Assert.AreEqual("Updated Title", quest.Title);
            Assert.AreEqual("Updated Desc", quest.Description);
            Assert.AreEqual(QuestDifficulty.Easy, quest.Difficulty);
            Assert.AreEqual(1, quest.QuestInterest.Count);
            Assert.AreEqual(2, quest.QuestInterest.First().InterestId);
        }

        [Test]
        public void EditQuestAsync_IfUserNotFound_ThrowsException()
        {
            var editModel = new QuestAddViewModel
            {
                Title = "Should Fail",
                Description = "No user",
                Difficulty = QuestDifficulty.Easy,
                InterestIds = new List<int>()
            };

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await questService.EditQuestAsync(Guid.NewGuid(), 1, editModel);
            });
        }

        [Test]
        public void EditQuestAsync_IfQuestNotFound_ThrowsException()
        {
            var editModel = new QuestAddViewModel
            {
                Title = "Should Fail",
                Description = "No quest",
                Difficulty = QuestDifficulty.Easy,
                InterestIds = new List<int>()
            };

            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await questService.EditQuestAsync(initiatorId, 999, editModel);
            });
        }

        [Test]
        public void EditQuestAsync_IfUserNotInitiator_ThrowsException()
        {
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            var editModel = new QuestAddViewModel
            {
                Title = "Should Fail",
                Description = "Not initiator",
                Difficulty = QuestDifficulty.Easy,
                InterestIds = new List<int>()
            };

            Assert.ThrowsAsync<UnauthorizedException>(async () =>
            {
                await questService.EditQuestAsync(otherUserId, 1, editModel);
            });
        }

        [Test]
        public async Task EditQuestAsync_IfQuestCompleted_ThrowsException()
        {
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                Status = QuestStatus.Completed,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            var editModel = new QuestAddViewModel
            {
                Title = "Should Fail",
                Description = "Completed",
                Difficulty = QuestDifficulty.Easy,
                InterestIds = new List<int>()
            };

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await questService.EditQuestAsync(initiatorId, 1, editModel);
            });
            Assert.That(ex.Message, Is.EqualTo(GCommon.OutputMessages.CompletedQuestFailedMessage));
        }

        [Test]
        public async Task GetQuestToDeleteAsync_ReturnsQuestViewModel()
        {
            // Arrange
            this.dbContext.Interests.Add(new Interest { Id = 1, Name = "Chess" });
            this.dbContext.SaveChanges();

            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.QuestInterests.Add(new QuestInterest { QuestId = 1, InterestId = 1 });
            this.dbContext.SaveChanges();

            // Act
            var result = await questService.GetQuestToDeleteAsync(initiatorId, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("A Quest", result.Title);
            Assert.AreEqual("Description", result.Description);
            Assert.That(result.Interests, Is.EquivalentTo(new[] { "Chess" }));
        }

        [Test]
        public void GetQuestToDeleteAsync_IfQuestNotFound_ThrowsException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await questService.GetQuestToDeleteAsync(initiatorId, 999);
            });
        }

        [Test]
        public void GetQuestToDeleteAsync_IfUserNotInitiator_ThrowsException()
        {
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            Assert.ThrowsAsync<UnauthorizedException>(async () =>
            {
                await questService.GetQuestToDeleteAsync(otherUserId, 1);
            });
        }

        [Test]
        public async Task GetQuestToDeleteAsync_ThrowsIfQuestCompleted()
        {
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                Status = QuestStatus.Completed,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await questService.GetQuestToDeleteAsync(initiatorId, 1);
            });
            Assert.That(ex.Message, Is.EqualTo(GCommon.OutputMessages.CompletedQuestFailedMessage));
        }

        [Test]
        public async Task ConfirmQuestToDeleteAsync_DeletesQuestAndRelatedEntities()
        {
            // Arrange
            this.dbContext.Interests.Add(new Interest { Id = 1, Name = "Chess" });
            this.dbContext.SaveChanges();

            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.QuestInterests.Add(new QuestInterest { QuestId = 1, InterestId = 1 });
            this.dbContext.QuestJoiners.Add(new QuestJoiner { QuestId = 1, ProfileId = initiatorId });
            this.dbContext.SaveChanges();

            // Act
            await questService.ConfirmQuestToDeleteAsync(initiatorId, 1);

            // Assert
            Assert.IsNull(dbContext.Quests.FirstOrDefault(q => q.Id == 1));
            Assert.IsEmpty(dbContext.QuestInterests.Where(qi => qi.QuestId == 1));
            Assert.IsEmpty(dbContext.QuestJoiners.Where(qj => qj.QuestId == 1));
        }

        [Test]
        public void ConfirmQuestToDeleteAsync_IfQuestNotFound_ThrowsException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await questService.ConfirmQuestToDeleteAsync(initiatorId, 999);
            });
        }

        [Test]
        public void ConfirmQuestToDeleteAsync_IfUserNotInitiator_ThrowsException()
        {
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            Assert.ThrowsAsync<UnauthorizedException>(async () =>
            {
                await questService.ConfirmQuestToDeleteAsync(otherUserId, 1);
            });
        }

        [Test]
        public async Task ConfirmQuestToDeleteAsync_IfQuestCompleted_ThrowsException()
        {
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                Status = QuestStatus.Completed,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await questService.ConfirmQuestToDeleteAsync(initiatorId, 1);
            });
            Assert.That(ex.Message, Is.EqualTo(GCommon.OutputMessages.CompletedQuestFailedMessage));
        }

        [Test]
        public async Task GetQuestDetailsWithJoinersViewModelAsync_WhenQuestExistsAndUserIsInitiatorOrJoiner_ReturnsDetails()
        {
            // Arrange
            var quest = new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            };
            this.dbContext.Quests.Add(quest);
            this.dbContext.QuestJoiners.Add(new QuestJoiner { QuestId = 1, ProfileId = initiatorId });
            this.dbContext.SaveChanges();

            // Act
            var result = await questService
                .GetQuestDetailsWithJoinersViewModelAsync(initiatorId, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("A Quest", result.Title);
            Assert.AreEqual("Description", result.Description);
        }

        [Test]
        public async Task GetQuestDetailsWithJoinersViewModelAsync_WhenQuestDoesNotExist_ThrowsException()
        {
           
            // Act & Assert
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await questService.GetQuestDetailsWithJoinersViewModelAsync(initiatorId, 999);
            });
            
        }

        [Test]
        public async Task GetJoinedQuestsCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.Quests.Add(new Quest
            {
                Id = 2,
                Title = "B Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext
                .QuestJoiners
                .Add(new QuestJoiner { QuestId = 1, ProfileId = initiatorId });

            this.dbContext
                .QuestJoiners
                .Add(new QuestJoiner { QuestId = 2, ProfileId = initiatorId });
            this.dbContext.SaveChanges();

            // Act
            var count = await questService.GetJoinedQuestsCountAsync(initiatorId);

            // Assert
            Assert.AreEqual(2, count);
        }

        [Test]
        public async Task GetJoinedQuestsCountAsync_WhenUserHasNotJoinedAny_ReturnsZero()
        {

            // Act
            var count = await questService.GetJoinedQuestsCountAsync(initiatorId);

            // Assert
            Assert.AreEqual(0, count);
        }

        [Test]
        public async Task GetAllJoinedQuestsByProfileIdAsync_ReturnsJoinedQuestsPaged()
        {
            // Arrange
            for (int i = 1; i <= 6; i++)
            {
                this.dbContext.Quests.Add(new Quest
                {
                    Id = i,
                    Title = $"Quest {i}",
                    Description = "Desc",
                    QuestInitiatorId = initiatorId,
                    QuestInterest = new List<QuestInterest>()
                });
                this.dbContext.QuestJoiners.Add(new QuestJoiner { QuestId = i, ProfileId = initiatorId });
            }
            this.dbContext.SaveChanges();

            // Act
            var page1 = (await questService
                .GetAllJoinedQuestsByProfileIdAsync(initiatorId, 1, 4))
                .ToList();

            var page2 = (await questService
                .GetAllJoinedQuestsByProfileIdAsync(initiatorId, 2, 4))
                .ToList();

            // Assert
            Assert.AreEqual(4, page1.Count);
            Assert.AreEqual(2, page2.Count);
            Assert.IsTrue(page1.All(q => q.Title.StartsWith("Quest")));
        }

        [Test]
        public async Task GetAllJoinedQuestsByProfileIdAsync_WhenUserHasNoJoinedQuests_ReturnsEmpty()
        {

            // Act
            var result = (await questService.GetAllJoinedQuestsByProfileIdAsync(initiatorId, 1, 10)).ToList();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetCreatedQuestsCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.Quests.Add(new Quest
            {
                Id = 2,
                Title = "B Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.Quests.Add(new Quest
            {
                Id = 3,
                Title = "Other User Quest",
                Description = "Description",
                QuestInitiatorId = otherUserId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            // Act
            var count = await questService.GetCreatedQuestsCountAsync(initiatorId);

            // Assert
            Assert.AreEqual(2, count);
        }

        [Test]
        public async Task GetCreatedQuestsCountAsync_WhenUserHasNoCreatedQuests_ReturnsZero()
        {
            

            // Act
            var count = await questService.GetCreatedQuestsCountAsync(Guid.NewGuid());

            // Assert
            Assert.AreEqual(0, count);
        }

        [Test]
        public async Task IsJoinedAsync_WhenUserIsJoined_ReturnsTrue()
        {
            // Arrange
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext
                .QuestJoiners
                .Add(new QuestJoiner { QuestId = 1, ProfileId = initiatorId });
            this.dbContext.SaveChanges();

            // Act
            var result = await questService
                .IsJoinedAsync(initiatorId, 1);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsJoinedAsync_WhenUserIsNotJoined_RetrunsFalse()
        {
            // Arrange
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            // Act
            var result = await questService.IsJoinedAsync(otherUserId, 1);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsJoinedAsync_WhenQuestDoesNotExist_ReturnsFalse()
        {
            // Act
            var result = await questService.IsJoinedAsync(initiatorId, 999);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task MarkQuestCompletedAsync_MarksQuestAsCompleted()
        {
            // Arrange
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                Status = QuestStatus.Active,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.QuestJoiners
                .Add(new QuestJoiner { QuestId = 1, ProfileId = initiatorId });
            this.dbContext.SaveChanges();

            // Act
            await questService
                .MarkQuestCompletedAsync(initiatorId, 1);

            // Assert
            var quest = dbContext
                .Quests
                .First(q => q.Id == 1);

            Assert.AreEqual(QuestStatus.Completed, quest.Status);
        }

        [Test]
        public void MarkQuestCompletedAsync_WhenQuestNotFound_ThrowsException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await questService.MarkQuestCompletedAsync(initiatorId, 999);
            });
        }

        [Test]
        public void MarkQuestCompletedAsync_WhenUserNotInitiator_ThrowsException()
        {
            this.dbContext.Quests.Add(new Quest
            {
                Id = 1,
                Title = "A Quest",
                Description = "Description",
                QuestInitiatorId = initiatorId,
                Status = QuestStatus.Active,
                QuestInterest = new List<QuestInterest>()
            });
            this.dbContext.SaveChanges();

            Assert.ThrowsAsync<UnauthorizedException>(async () =>
            {
                await questService.MarkQuestCompletedAsync(otherUserId, 1);
            });
        }

        [Test]
        public async Task GetAllInterestsAsync_ReturnsAllInterests()
        {
            // Arrange
            this.dbContext.Interests.Add(new Interest { Id = 1, Name = "Chess" });
            this.dbContext.Interests.Add(new Interest { Id = 2, Name = "Coding" });
            this.dbContext.SaveChanges();

            // Act
            var result = await questService.GetAllInterestsAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEquivalent(new[] { "Chess", "Coding" }, result.Select(i => i.Name));
        }

        [Test]
        public async Task GetAllInterestsAsync_WhenNoInterests_ReturnsEmptyList()
        {
            // Act
            var result = await questService.GetAllInterestsAsync();

            // Assert
            Assert.IsEmpty(result);
        }
    }
}



