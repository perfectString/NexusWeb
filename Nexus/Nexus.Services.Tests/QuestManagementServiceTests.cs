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

        private static readonly Guid defaultInitiatorId = Guid.Parse("68320c63-0ac9-40fb-ba2d-b69d63e43edd");

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

        private Profile CreateProfile(
            Guid? id = null,
            string displayName = "User",
            string city = "None",
            int xp = 0)
        {
            return new Profile
            {
                Id = id ?? Guid.NewGuid(),
                DisplayName = displayName,
                City = city,
                ExperiencePoints = xp
            };
        }

        private void SeedQuest(
            int id,
            string title = "Quest",
            string description = "Desc",
            Guid? initiatorId = null,
            Profile? initiator = null,
            QuestStatus status = QuestStatus.Active,
            QuestDifficulty difficulty = QuestDifficulty.Easy,
            List<QuestJoiner>? joiners = null)
        {
            this.dbContext.Quests.Add(new Quest
            {
                Id = id,
                Title = title,
                Description = description,
                QuestInitiatorId = initiatorId ?? defaultInitiatorId,
                QuestInitiator = initiator!,
                Status = status,
                Difficulty = difficulty,
                QuestInterest = new List<QuestInterest>(),
                QuestJoiners = joiners ?? new List<QuestJoiner>()
            });
            this.dbContext.SaveChanges();
        }

        private void SeedQuests(int count, Guid initiatorId, Profile initiator)
        {
            for (int i = 1; i <= count; i++)
            {
                this.dbContext.Quests.Add(new Quest
                {
                    Id = i,
                    Title = $"Quest {i}",
                    Description = $"Desc {i}",
                    QuestInitiatorId = initiatorId,
                    QuestInitiator = initiator,
                    QuestInterest = new List<QuestInterest>(),
                    QuestJoiners = new List<QuestJoiner>()
                });
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

        private void SeedQuestJoiners(params (int QuestId, Guid ProfileId)[] joiners)
        {
            foreach (var (questId, profileId) in joiners)
            {
                this.dbContext.QuestJoiners.Add(new QuestJoiner { QuestId = questId, ProfileId = profileId });
            }
            this.dbContext.SaveChanges();
        }

        private static QuestManagementViewModel BuildEditModel(
            int id,
            string title = "Updated Quest",
            string description = "Updated Desc",
            QuestDifficulty difficulty = QuestDifficulty.Easy,
            QuestStatus status = QuestStatus.Active,
            List<int>? interestIds = null)
        {
            return new QuestManagementViewModel
            {
                Id = id,
                Title = title,
                Description = description,
                Difficulty = difficulty,
                Status = status,
                InterestIds = interestIds ?? new List<int>()
            };
        }

        [Test]
        public async Task GetAllQuestsCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            SeedQuest(1, title: "Quest1", description: "Desc1");
            SeedQuest(2, title: "Quest2", description: "Desc2");

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

        [TestCase(1, 5, 5)]
        [TestCase(2, 5, 2)]
        public async Task GetAllQuestsAsAdminAsync_ReturnsPaginatedResults(
            int page, int pageSize, int expectedCount)
        {
            // Arrange
            var initiator = CreateProfile(displayName: "Admin");
            this.dbContext.Users.Add(initiator);
            this.dbContext.SaveChanges();
            SeedQuests(7, initiator.Id, initiator);

            // Act
            var result = (await questManagementService
                .GetAllQuestsAsAdminAsync(page, pageSize))
                .ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(expectedCount));
            Assert.That(result, Has.All.Matches<QuestManagementViewModel>(q => q.Title.StartsWith("Quest")));
        }

        [Test]
        public async Task GetAllQuestsAsAdminAsync_WhenNoQuests_ReturnsEmpty()
        {
            // Act
            var result = (await questManagementService
                .GetAllQuestsAsAdminAsync(1, 10))
                .ToList();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAllInterestsAsync_ReturnsAllInterests()
        {
            // Arrange
            SeedInterests((1, "Chess"), (2, "Coding"));

            // Act
            var result = await questManagementService.GetAllInterestsAsync();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(
                result.Select(i => i.Name),
                Is.EquivalentTo(new[] { "Chess", "Coding" }));
        }

        [Test]
        public async Task GetAllInterestsAsync_WhenNoInterests_ReturnsEmpty()
        {

            // Act
            var result = await questManagementService.GetAllInterestsAsync();

            // Assert
            Assert.That(result, Is.Empty);
        }


        [Test]
        public async Task GetQuestToEditAsAdminAsync_WhenQuestExists_ReturnsQuest()
        {
            // Arrange
            var initiator = CreateProfile(displayName: "User");
            this.dbContext.Users.Add(initiator);
            this.dbContext.SaveChanges();
            SeedQuest(1, title: "Quest1", description: "Desc1", initiatorId: initiator.Id, initiator: initiator);

            // Act
            var result = await questManagementService.GetQuestToEditAsAdminAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Title, Is.EqualTo("Quest1"));
            Assert.That(result.Description, Is.EqualTo("Desc1"));
            Assert.That(result.QuestInitiator, Is.EqualTo("User"));
        }

        [Test]
        public void GetQuestToEditAsAdminAsync_WhenQuestNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => questManagementService.GetQuestToEditAsAdminAsync(999));
        }


        [Test]
        public async Task EditQuestAsAdminAsync_UpdatesTitleAndDescription()
        {
            // Arrange
            SeedQuest(1, title: "Quest1", description: "Desc1");
            var editModel = BuildEditModel(1, title: "Updated Quest", description: "Updated Desc");

            // Act
            await questManagementService.EditQuestAsAdminAsync(1, editModel);

            // Assert
            var updated = dbContext.Quests.First(q => q.Id == 1);
            Assert.That(updated.Title, Is.EqualTo("Updated Quest"));
            Assert.That(updated.Description, Is.EqualTo("Updated Desc"));
        }

        [Test]
        public void EditQuestAsAdminAsync_WhenQuestNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var editModel = BuildEditModel(999);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => questManagementService.EditQuestAsAdminAsync(999, editModel));
        }

        [Test]
        public async Task EditQuestAsAdminAsync_WithNoInterests_DoesNotAddQuestInterests()
        {
            // Arrange
            SeedQuest(1);
            var model = BuildEditModel(1, interestIds: new List<int>());

            // Act
            await questManagementService.EditQuestAsAdminAsync(1, model);

            // Assert
            Assert.That(
                dbContext.QuestInterests.Where(qi => qi.QuestId == 1).ToList(),
                Is.Empty);
        }

        [Test]
        public async Task EditQuestAsAdminAsync_WithDuplicateInterests_AddsDistinctOnly()
        {
            // Arrange
            SeedQuest(2);
            var model = BuildEditModel(2, interestIds: new List<int> { 1, 1, 2 });

            // Act
            await questManagementService.EditQuestAsAdminAsync(2, model);

            // Assert
            var interests = dbContext.QuestInterests
                .Where(qi => qi.QuestId == 2)
                .Select(qi => qi.InterestId)
                .ToList();
            Assert.That(interests, Has.Count.EqualTo(2));
            Assert.That(interests, Is.EquivalentTo(new[] { 1, 2 }));
        }

        [Test]
        public void EditQuestAsAdminAsync_WithMoreThanThreeInterests_ThrowsArgumentException()
        {
            // Arrange
            SeedQuest(11);
            var model = BuildEditModel(11, interestIds: new List<int> { 1, 2, 3, 4 });

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(
                () => questManagementService.EditQuestAsAdminAsync(11, model));
        }

        [Test]
        public async Task EditQuestAsAdminAsync_WhenAlreadyCompleted_DoesNotUpdateJoinerXp()
        {
            // Arrange
            var profile = CreateProfile(xp: 0);
            this.dbContext.Users.Add(profile);
            this.dbContext.SaveChanges();

            SeedQuest(3, status: QuestStatus.Completed,
                joiners: new List<QuestJoiner> { new QuestJoiner { Profile = profile } });

            var model = BuildEditModel(3, status: QuestStatus.Completed);

            // Act
            await questManagementService.EditQuestAsAdminAsync(3, model);

            // Assert
            Assert.That(profile.ExperiencePoints, Is.EqualTo(0));
        }

        [TestCase(QuestDifficulty.Easy)]
        [TestCase(QuestDifficulty.Medium)]
        [TestCase(QuestDifficulty.Hard)]
        public async Task EditQuestAsAdminAsync_WhenTransitioningToCompleted_AwardsCorrectXpToJoiners(
            QuestDifficulty difficulty)
        {
            // Arrange
            var profile = CreateProfile(xp: 0);
            this.dbContext.Users.Add(profile);
            this.dbContext.SaveChanges();

            SeedQuest(4, status: QuestStatus.Active,
                joiners: new List<QuestJoiner> { new QuestJoiner { Profile = profile } });

            var model = BuildEditModel(4, status: QuestStatus.Completed, difficulty: difficulty);

            // Act
            await questManagementService.EditQuestAsAdminAsync(4, model);

            // Assert
            var expectedXp = QuestRewardHelper.GetRewardXp(difficulty);
            Assert.That(profile.ExperiencePoints, Is.EqualTo(expectedXp));
        }


        [Test]
        public async Task DeleteQuestAsAdminAsync_DeletesQuestAndRelatedData()
        {
            // Arrange
            var joinerId = Guid.NewGuid();
            SeedQuest(1);
            SeedQuestInterests((1, 1));
            SeedQuestJoiners((1, joinerId));

            // Act
            await questManagementService.DeleteQuestAsAdminAsync(1);

            // Assert
            Assert.That(dbContext.Quests.FirstOrDefault(q => q.Id == 1), Is.Null);
            Assert.That(dbContext.QuestInterests.Where(qi => qi.QuestId == 1).ToList(), Is.Empty);
            Assert.That(dbContext.QuestJoiners.Where(qj => qj.QuestId == 1).ToList(), Is.Empty);
        }

        [Test]
        public void DeleteQuestAsAdminAsync_WhenQuestNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange 

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => questManagementService.DeleteQuestAsAdminAsync(999));
        }
    }
}