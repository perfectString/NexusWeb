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
        private static readonly Guid unknownUserId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

        [SetUp]
        public void Setup()
        {
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

        private Quest CreateQuest(
            int id,
            string title = "Test Quest",
            string description = "Description",
            Guid? initiator = null,
            QuestStatus status = QuestStatus.Active,
            QuestDifficulty difficulty = QuestDifficulty.Easy)
        {
            return new Quest
            {
                Id = id,
                Title = title,
                Description = description,
                QuestInitiatorId = initiator ?? initiatorId,
                Status = status,
                Difficulty = difficulty,
                QuestInterest = new List<QuestInterest>()
            };
        }

        private void SeedQuest(
            int id,
            string title = "Test Quest",
            string description = "Description",
            Guid? initiator = null,
            QuestStatus status = QuestStatus.Active,
            QuestDifficulty difficulty = QuestDifficulty.Easy)
        {
            this.dbContext.Quests.Add(CreateQuest(id, title, description, initiator, status, difficulty));
            this.dbContext.SaveChanges();
        }

        private void SeedQuests(int count, Guid? initiator = null)
        {
            for (int i = 1; i <= count; i++)
            {
                this.dbContext.Quests.Add(CreateQuest(i, $"Quest {i}", initiator: initiator));
            }
            this.dbContext.SaveChanges();
        }

        private void SeedInterests(params (int Id, string Name)[] interests)
        {
            foreach (var (id, name) in interests)
            {
                this.dbContext.Interests.Add(new Interest { Id = id, Name = name });
            }
            this.dbContext.SaveChanges();
        }

        private void SeedQuestInterests(params (int QuestId, int InterestId)[] links)
        {
            foreach (var (questId, interestId) in links)
            {
                this.dbContext.QuestInterests.Add(new QuestInterest { QuestId = questId, InterestId = interestId });
            }
            this.dbContext.SaveChanges();
        }

        private void SeedJoiners(params (int QuestId, Guid ProfileId)[] joiners)
        {
            foreach (var (questId, profileId) in joiners)
            {
                this.dbContext.QuestJoiners.Add(new QuestJoiner { QuestId = questId, ProfileId = profileId });
            }
            this.dbContext.SaveChanges();
        }

        private static QuestAddViewModel BuildQuestAddModel(
            string title = "New Quest",
            string description = "Test Desc",
            QuestDifficulty difficulty = QuestDifficulty.Medium,
            List<int>? interestIds = null)
        {
            return new QuestAddViewModel
            {
                Title = title,
                Description = description,
                Difficulty = difficulty,
                InterestIds = interestIds ?? new List<int>()
            };
        }

        [Test]
        public async Task GetAllQuestsCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            SeedQuests(2);

            // Act
            var count = await questService.GetAllQuestsCountAsync();

            // Assert
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllQuestsCountAsync_WhenNoQuests_ReturnsZero()
        {
            // Arrange 

            // Act
            var count = await questService.GetAllQuestsCountAsync();

            // Assert
            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllQuestsOrderByTitleAsync_ReturnsQuestsOrderedByTitle()
        {
            // Arrange
            SeedQuest(1, title: "B Quest");
            SeedQuest(2, title: "A Quest");

            // Act
            var result = (await questService.GetAllQuestsOrderByTitleAsync(1, 10)).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].Title, Is.EqualTo("A Quest"));
            Assert.That(result[1].Title, Is.EqualTo("B Quest"));
        }

        [TestCase(1, 5, 5)]
        [TestCase(2, 5, 3)]
        public async Task GetAllQuestsOrderByTitleAsync_Pagination_ReturnsExpectedPageSize(
            int page, int pageSize, int expectedCount)
        {
            // Arrange
            SeedQuests(8);

            // Act
            var result = (await questService.GetAllQuestsOrderByTitleAsync(page, pageSize)).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(expectedCount));
        }

        [Test]
        public async Task GetAllQuestsOrderByTitleAsync_WhenNoQuests_ReturnsEmpty()
        {
            // Arrange

            // Act
            var result = (await questService.GetAllQuestsOrderByTitleAsync(1, 10)).ToList();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetEmptyQuestAddModelAsync_ReturnsAllAvailableInterests()
        {
            // Arrange
            SeedInterests((1, "Chess"), (2, "Coding"));

            // Act
            var result = await questService.GetEmptyQuestAddModelAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AvailableInterests, Has.Count.EqualTo(2));
            Assert.That(
                result.AvailableInterests.Select(i => i.Name),
                Is.EquivalentTo(new[] { "Chess", "Coding" }));
        }

        [Test]
        public async Task GetEmptyQuestAddModelAsync_WhenNoInterests_ReturnsEmptyList()
        {
            // Arrange

            // Act
            var result = await questService.GetEmptyQuestAddModelAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AvailableInterests, Is.Empty);
        }

        [Test]
        public async Task AddQuestsAndJoinInitiatorAsync_CreatesQuestWithInterests_AndJoinsInitiator()
        {
            // Arrange
            SeedInterests((1, "Chess"), (2, "Coding"));
            var model = BuildQuestAddModel(interestIds: new List<int> { 1, 2 });

            // Act
            await questService.AddQuestsAndJoinInitiatorAsync(initiatorId, model);

            // Assert
            var quest = dbContext.Quests.FirstOrDefault(q => q.Title == "New Quest");
            Assert.That(quest, Is.Not.Null);
            Assert.That(quest!.Description, Is.EqualTo("Test Desc"));
            Assert.That(quest.Difficulty, Is.EqualTo(QuestDifficulty.Medium));

            var joiner = dbContext.QuestJoiners
                .FirstOrDefault(j => j.QuestId == quest.Id && j.ProfileId == initiatorId);
            Assert.That(joiner, Is.Not.Null);

            var questInterestIds = dbContext.QuestInterests
                .Where(qi => qi.QuestId == quest.Id)
                .Select(qi => qi.InterestId)
                .ToList();
            Assert.That(questInterestIds, Is.EquivalentTo(new[] { 1, 2 }));
        }

        [Test]
        public async Task AddQuestsAndJoinInitiatorAsync_WithNoInterests_CreatesQuestWithoutInterests()
        {
            // Arrange
            var model = BuildQuestAddModel(
                title: "No Interest Quest",
                description: "No interests",
                difficulty: QuestDifficulty.Easy);

            // Act
            await questService.AddQuestsAndJoinInitiatorAsync(initiatorId, model);

            // Assert
            var quest = dbContext.Quests.FirstOrDefault(q => q.Title == "No Interest Quest");
            Assert.That(quest, Is.Not.Null);
            Assert.That(
                dbContext.QuestInterests.Where(qi => qi.QuestId == quest!.Id).ToList(),
                Is.Empty);
        }

        [Test]
        public void AddQuestsAndJoinInitiatorAsync_WhenUserNotFound_ThrowsNullReferenceException()
        {
            // Arrange
            var model = BuildQuestAddModel();

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(
                () => questService.AddQuestsAndJoinInitiatorAsync(unknownUserId, model));
        }

        [Test]
        public async Task GetQuestToEditViewModelAsync_ReturnsCorrectModel()
        {
            // Arrange
            SeedInterests((10, "TestInterest"));
            SeedQuest(1, title: "A Quest");
            SeedQuestInterests((1, 10));

            // Act
            var result = await questService.GetQuestToEditViewModelAsync(initiatorId, 1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Title, Is.EqualTo("A Quest"));
            Assert.That(result.Description, Is.EqualTo("Description"));
            Assert.That(result.Difficulty, Is.EqualTo(QuestDifficulty.Easy));
            Assert.That(result.InterestIds, Is.EquivalentTo(new[] { 10 }));
            Assert.That(result.AvailableInterests.Any(i => i.Id == 10 && i.Name == "TestInterest"), Is.True);
        }

        [Test]
        public void GetQuestToEditViewModelAsync_WhenUserNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => questService.GetQuestToEditViewModelAsync(unknownUserId, 1));
        }

        [Test]
        public void GetQuestToEditViewModelAsync_WhenQuestNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => questService.GetQuestToEditViewModelAsync(initiatorId, 999));
        }

        [Test]
        public void GetQuestToEditViewModelAsync_WhenUserNotInitiator_ThrowsUnauthorizedException()
        {
            // Arrange
            SeedQuest(1, title: "A Quest");

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedException>(
                () => questService.GetQuestToEditViewModelAsync(otherUserId, 1));
        }

        [Test]
        public void GetQuestToEditViewModelAsync_WhenQuestCompleted_ThrowsInvalidOperationException()
        {
            // Arrange
            SeedQuest(1, title: "A Quest", status: QuestStatus.Completed);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(
                () => questService.GetQuestToEditViewModelAsync(initiatorId, 1));

            Assert.That(ex!.Message, Is.EqualTo(GCommon.OutputMessages.CompletedQuestFailedMessage));
        }

        [Test]
        public async Task EditQuestAsync_UpdatesQuestAndInterests()
        {
            // Arrange
            SeedInterests((2, "Coding"));
            SeedQuest(1, title: "A Quest");
            var editModel = BuildQuestAddModel(
                title: "Updated Title",
                description: "Updated Desc",
                difficulty: QuestDifficulty.Hard,
                interestIds: new List<int> { 2 });

            // Act
            await questService.EditQuestAsync(initiatorId, 1, editModel);

            // Assert
            var quest = dbContext.Quests
                .Include(q => q.QuestInterest)
                .First(q => q.Id == 1);

            Assert.That(quest.Title, Is.EqualTo("Updated Title"));
            Assert.That(quest.Description, Is.EqualTo("Updated Desc"));
            Assert.That(quest.Difficulty, Is.EqualTo(QuestDifficulty.Easy));
            Assert.That(quest.QuestInterest, Has.Count.EqualTo(1));
            Assert.That(quest.QuestInterest.First().InterestId, Is.EqualTo(2));
        }

        [Test]
        public void EditQuestAsync_WhenUserNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var editModel = BuildQuestAddModel();

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => questService.EditQuestAsync(unknownUserId, 1, editModel));
        }

        [Test]
        public void EditQuestAsync_WhenQuestNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var editModel = BuildQuestAddModel();

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => questService.EditQuestAsync(initiatorId, 999, editModel));
        }

        [Test]
        public void EditQuestAsync_WhenUserNotInitiator_ThrowsUnauthorizedException()
        {
            // Arrange
            SeedQuest(1, title: "A Quest");
            var editModel = BuildQuestAddModel();

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedException>(
                () => questService.EditQuestAsync(otherUserId, 1, editModel));
        }

        [Test]
        public void EditQuestAsync_WhenQuestCompleted_ThrowsInvalidOperationException()
        {
            // Arrange
            SeedQuest(1, title: "A Quest", status: QuestStatus.Completed);
            var editModel = BuildQuestAddModel();

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(
                () => questService.EditQuestAsync(initiatorId, 1, editModel));

            Assert.That(ex!.Message, Is.EqualTo(GCommon.OutputMessages.CompletedQuestFailedMessage));
        }

        [Test]
        public async Task GetQuestToDeleteAsync_ReturnsQuestViewModel()
        {
            // Arrange
            SeedInterests((1, "Chess"));
            SeedQuest(1, title: "A Quest");
            SeedQuestInterests((1, 1));

            // Act
            var result = await questService.GetQuestToDeleteAsync(initiatorId, 1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Title, Is.EqualTo("A Quest"));
            Assert.That(result.Description, Is.EqualTo("Description"));
            Assert.That(result.Interests, Is.EquivalentTo(new[] { "Chess" }));
        }

        [Test]
        public void GetQuestToDeleteAsync_WhenQuestNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => questService.GetQuestToDeleteAsync(initiatorId, 999));
        }

        [Test]
        public void GetQuestToDeleteAsync_WhenUserNotInitiator_ThrowsUnauthorizedException()
        {
            // Arrange
            SeedQuest(1, title: "A Quest");

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedException>(
                () => questService.GetQuestToDeleteAsync(otherUserId, 1));
        }

        [Test]
        public void GetQuestToDeleteAsync_WhenQuestCompleted_ThrowsInvalidOperationException()
        {
            // Arrange
            SeedQuest(1, title: "A Quest", status: QuestStatus.Completed);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(
                () => questService.GetQuestToDeleteAsync(initiatorId, 1));

            Assert.That(ex!.Message, Is.EqualTo(GCommon.OutputMessages.CompletedQuestFailedMessage));
        }

        [Test]
        public async Task ConfirmQuestToDeleteAsync_DeletesQuestAndRelatedEntities()
        {
            // Arrange
            SeedInterests((1, "Chess"));
            SeedQuest(1, title: "A Quest");
            SeedQuestInterests((1, 1));
            SeedJoiners((1, initiatorId));

            // Act
            await questService.ConfirmQuestToDeleteAsync(initiatorId, 1);

            // Assert
            Assert.That(dbContext.Quests.FirstOrDefault(q => q.Id == 1), Is.Null);
            Assert.That(dbContext.QuestInterests.Where(qi => qi.QuestId == 1).ToList(), Is.Empty);
            Assert.That(dbContext.QuestJoiners.Where(qj => qj.QuestId == 1).ToList(), Is.Empty);
        }

        [Test]
        public void ConfirmQuestToDeleteAsync_WhenQuestNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange 

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => questService.ConfirmQuestToDeleteAsync(initiatorId, 999));
        }

        [Test]
        public void ConfirmQuestToDeleteAsync_WhenUserNotInitiator_ThrowsUnauthorizedException()
        {
            // Arrange
            SeedQuest(1, title: "A Quest");

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedException>(
                () => questService.ConfirmQuestToDeleteAsync(otherUserId, 1));
        }

        [Test]
        public void ConfirmQuestToDeleteAsync_WhenQuestCompleted_ThrowsInvalidOperationException()
        {
            // Arrange
            SeedQuest(1, title: "A Quest", status: QuestStatus.Completed);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(
                () => questService.ConfirmQuestToDeleteAsync(initiatorId, 1));

            Assert.That(ex!.Message, Is.EqualTo(GCommon.OutputMessages.CompletedQuestFailedMessage));
        }

        [Test]
        public async Task GetQuestDetailsWithJoinersViewModelAsync_WhenQuestExists_ReturnsDetails()
        {
            // Arrange
            SeedQuest(1, title: "A Quest");
            SeedJoiners((1, initiatorId));

            // Act
            var result = await questService.GetQuestDetailsWithJoinersViewModelAsync(initiatorId, 1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(1));
            Assert.That(result.Title, Is.EqualTo("A Quest"));
            Assert.That(result.Description, Is.EqualTo("Description"));
        }

        [Test]
        public void GetQuestDetailsWithJoinersViewModelAsync_WhenQuestNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => questService.GetQuestDetailsWithJoinersViewModelAsync(initiatorId, 999));
        }

        [Test]
        public async Task GetJoinedQuestsCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            SeedQuests(2);
            SeedJoiners((1, initiatorId), (2, initiatorId));

            // Act
            var count = await questService.GetJoinedQuestsCountAsync(initiatorId);

            // Assert
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetJoinedQuestsCountAsync_WhenUserHasNotJoinedAny_ReturnsZero()
        {
            // Arrange

            // Act
            var count = await questService.GetJoinedQuestsCountAsync(initiatorId);

            // Assert
            Assert.That(count, Is.EqualTo(0));
        }

        [TestCase(1, 4, 4)]
        [TestCase(2, 4, 2)]
        public async Task GetAllJoinedQuestsByProfileIdAsync_ReturnsPaginatedResults(
            int page, int pageSize, int expectedCount)
        {
            // Arrange
            SeedQuests(6);
            SeedJoiners(
                (1, initiatorId), (2, initiatorId), (3, initiatorId),
                (4, initiatorId), (5, initiatorId), (6, initiatorId));

            // Act
            var result = (await questService
                .GetAllJoinedQuestsByProfileIdAsync(initiatorId, page, pageSize))
                .ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(expectedCount));
            Assert.That(result, Has.All.Matches<QuestViewModel>(q => q.Title.StartsWith("Quest")));
        }

        [Test]
        public async Task GetAllJoinedQuestsByProfileIdAsync_WhenNoJoinedQuests_ReturnsEmpty()
        {
            // Arrange

            // Act
            var result = (await questService
                .GetAllJoinedQuestsByProfileIdAsync(initiatorId, 1, 10))
                .ToList();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetCreatedQuestsCountAsync_ReturnsOnlyCountForGivenUser()
        {
            // Arrange
            SeedQuest(1, title: "A Quest");
            SeedQuest(2, title: "B Quest");
            SeedQuest(3, title: "Other User Quest", initiator: otherUserId);

            // Act
            var count = await questService.GetCreatedQuestsCountAsync(initiatorId);

            // Assert
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetCreatedQuestsCountAsync_WhenUserHasNoQuests_ReturnsZero()
        {
            // Arrange

            // Act
            var count = await questService.GetCreatedQuestsCountAsync(unknownUserId);

            // Assert
            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public async Task IsJoinedAsync_WhenUserIsJoined_ReturnsTrue()
        {
            // Arrange
            SeedQuest(1, title: "A Quest");
            SeedJoiners((1, initiatorId));

            // Act
            var result = await questService.IsJoinedAsync(initiatorId, 1);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsJoinedAsync_WhenUserIsNotJoined_ReturnsFalse()
        {
            // Arrange
            SeedQuest(1, title: "A Quest");

            // Act
            var result = await questService.IsJoinedAsync(otherUserId, 1);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task IsJoinedAsync_WhenQuestDoesNotExist_ReturnsFalse()
        {
            // Arrange

            // Act
            var result = await questService.IsJoinedAsync(initiatorId, 999);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task MarkQuestCompletedAsync_SetsStatusToCompleted()
        {
            // Arrange
            SeedQuest(1, title: "A Quest");
            SeedJoiners((1, initiatorId));

            // Act
            await questService.MarkQuestCompletedAsync(initiatorId, 1);

            // Assert
            var quest = dbContext.Quests.First(q => q.Id == 1);
            Assert.That(quest.Status, Is.EqualTo(QuestStatus.Completed));
        }

        [Test]
        public void MarkQuestCompletedAsync_WhenQuestNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => questService.MarkQuestCompletedAsync(initiatorId, 999));
        }

        [Test]
        public void MarkQuestCompletedAsync_WhenUserNotInitiator_ThrowsUnauthorizedException()
        {
            // Arrange
            SeedQuest(1, title: "A Quest");

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedException>(
                () => questService.MarkQuestCompletedAsync(otherUserId, 1));
        }

        [Test]
        public async Task GetAllInterestsAsync_ReturnsAllInterests()
        {
            // Arrange
            SeedInterests((1, "Chess"), (2, "Coding"));

            // Act
            var result = await questService.GetAllInterestsAsync();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.Select(i => i.Name), Is.EquivalentTo(new[] { "Chess", "Coding" }));
        }

        [Test]
        public async Task GetAllInterestsAsync_WhenNoInterests_ReturnsEmpty()
        {
            // Arrange

            // Act
            var result = await questService.GetAllInterestsAsync();

            // Assert
            Assert.That(result, Is.Empty);
        }
    }
}