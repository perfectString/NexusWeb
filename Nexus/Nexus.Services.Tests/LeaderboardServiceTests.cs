using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Data.Models;
using Nexus.Data.Services.Core;
using Nexus.GCommon.Enums;

namespace Nexus.Services.Tests
{
    [TestFixture]
    public class LeaderboardServiceTests
    {
        private NexusDbContext dbContext;
        private LeaderboardService leaderboardService;

        private static readonly Guid adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private static readonly Guid userRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        private static readonly Guid adminId = Guid.Parse("8d7b0283-56a0-4c40-85d3-d1be04437f3e");
        private static readonly Guid userOneId = Guid.Parse("dd172857-29d5-4d62-9850-5221384ed08f");
        private static readonly Guid userTwoId = Guid.Parse("d57e9e71-cc0f-4ddf-ae83-52dc4dfc2be7");

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NexusDbContext>()
                .UseInMemoryDatabase("DummyDatabaseNexus" + Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new NexusDbContext(options);

            SeedRoles(dbContext);

            SeedProfileWithRole(dbContext, adminId, "AdminDummy", "NightCity", 8000, adminRoleId);
            SeedProfileWithRole(dbContext, userOneId, "AlexDummy", "NightCity", 4000, userRoleId);
            SeedProfileWithRole(dbContext, userTwoId, "YoanaDummy", "NightCity", 2000, userRoleId);

            var questOne = SeedQuest(dbContext, 100, "CompletedQuestOne", QuestStatus.Completed);
            var questTwo = SeedQuest(dbContext, 200, "CompletedQuestTwo", QuestStatus.Completed);
            var questThree = SeedQuest(dbContext, 300, "ActiveQuestOne", QuestStatus.Active);

            SeedQuestJoiners(dbContext,
                (userOneId, questOne),
                (userOneId, questTwo),
                (userTwoId, questOne),
                (userTwoId, questThree),
                (adminId, questOne));

            dbContext.SaveChanges();
            leaderboardService = new LeaderboardService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        private static void SeedRoles(NexusDbContext context)
        {
            context.Roles.AddRange(
                new IdentityRole<Guid> { Id = adminRoleId, Name = "Admin" },
                new IdentityRole<Guid> { Id = userRoleId, Name = "User" });
            context.SaveChanges();
        }

        private static Profile CreateProfile(
            Guid id,
            string displayName,
            string city = "NightCity",
            int xp = 0)
        {
            return new Profile
            {
                Id = id,
                DisplayName = displayName,
                City = city,
                ExperiencePoints = xp,
                JoinedQuests = new List<QuestJoiner>()
            };
        }

        private static void SeedProfileWithRole(
            NexusDbContext context,
            Guid id,
            string displayName,
            string city,
            int xp,
            Guid roleId)
        {
            context.Users.Add(CreateProfile(id, displayName, city, xp));
            context.UserRoles.Add(new IdentityUserRole<Guid> { UserId = id, RoleId = roleId });
            context.SaveChanges();
        }

        private static Quest SeedQuest(
            NexusDbContext context,
            int id,
            string title,
            QuestStatus status)
        {
            var quest = new Quest
            {
                Id = id,
                Title = title,
                Description = "Testing",
                Status = status
            };
            context.Quests.Add(quest);
            context.SaveChanges();
            return quest;
        }

        private static void SeedQuestJoiners(
            NexusDbContext context,
            params (Guid ProfileId, Quest Quest)[] joiners)
        {
            foreach (var (profileId, quest) in joiners)
            {
                context.QuestJoiners.Add(new QuestJoiner
                {
                    ProfileId = profileId,
                    QuestId = quest.Id,
                    Quest = quest
                });
            }
            context.SaveChanges();
        }

        private static (NexusDbContext context, LeaderboardService service) CreateIsolatedContext()
        {
            var options = new DbContextOptionsBuilder<NexusDbContext>()
                .UseInMemoryDatabase("DummyNexusDb" + Guid.NewGuid().ToString())
                .Options;

            var context = new NexusDbContext(options);
            SeedRoles(context);
            var service = new LeaderboardService(context);
            return (context, service);
        }

        private static void SeedUsersWithCompletedQuests(
            NexusDbContext context,
            int userCount,
            int adminIndex = 0,
            bool assignCompletedQuests = false)
        {
            int questIdCounter = 1000;

            for (int i = 0; i < userCount; i++)
            {
                var userId = Guid.NewGuid();
                var roleId = (i == adminIndex) ? adminRoleId : userRoleId;

                SeedProfileWithRole(context, userId, $"Dummy{i}", "NightCity", 1000 + i * 2, roleId);

                if (assignCompletedQuests)
                {
                    for (int j = 0; j < i; j++)
                    {
                        var quest = SeedQuest(context, questIdCounter++, $"CompletedQuest{j}", QuestStatus.Completed);
                        SeedQuestJoiners(context, (userId, quest));
                    }
                }
            }
        }

        [Test]
        public async Task TopFiveUsersByLevelAsync_ExcludesAdmins()
        {
            // Arrange 

            // Act
            var result = (await leaderboardService.TopFiveUsersByLevelAsync()).ToList();

            // Assert
            Assert.That(result.Any(u => u.DisplayName == "AdminDummy"), Is.False);
        }

        [Test]
        public async Task TopFiveUsersByLevelAsync_OrdersByXpDescending()
        {
            // Arrange 

            // Act
            var result = (await leaderboardService.TopFiveUsersByLevelAsync()).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].DisplayName, Is.EqualTo("AlexDummy"));
            Assert.That(result[0].ExperiencePoints, Is.EqualTo(4000));
            Assert.That(result[1].DisplayName, Is.EqualTo("YoanaDummy"));
            Assert.That(result[1].ExperiencePoints, Is.EqualTo(2000));
        }

        [Test]
        public async Task TopFiveUsersByLevelAsync_WhenNoUsers_ReturnsEmpty()
        {
            // Arrange
            var (context, service) = CreateIsolatedContext();

            // Act
            var result = (await service.TopFiveUsersByLevelAsync()).ToList();

            // Assert
            Assert.That(result, Is.Empty);

            context.Dispose();
        }

        [Test]
        public async Task TopFiveUsersByLevelAsync_WithMoreThanFiveUsers_ReturnsAtMostFive()
        {
            // Arrange
            var (context, service) = CreateIsolatedContext();
            SeedUsersWithCompletedQuests(context, userCount: 7, adminIndex: 0);

            // Act
            var result = (await service.TopFiveUsersByLevelAsync()).ToList();

            // Assert
            Assert.That(result, Has.Count.LessThanOrEqualTo(5));
            Assert.That(result.Any(u => u.DisplayName == "Dummy0"), Is.False);

            context.Dispose();
        }

        [Test]
        public async Task TopFiveUsersByCompletedQuestsAsync_ExcludesAdmins()
        {
            // Arrange 

            // Act
            var result = (await leaderboardService.TopFiveUsersByCompletedQuestsAsync()).ToList();

            // Assert
            Assert.That(result.Any(u => u.DisplayName == "AdminDummy"), Is.False);
        }

        [Test]
        public async Task TopFiveUsersByCompletedQuestsAsync_OrdersByCompletedQuestsDescending()
        {
            // Arrange

            // Act
            var result = (await leaderboardService.TopFiveUsersByCompletedQuestsAsync()).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].DisplayName, Is.EqualTo("AlexDummy"));
            Assert.That(result[0].CompletedQuests, Is.EqualTo(2));
            Assert.That(result[1].DisplayName, Is.EqualTo("YoanaDummy"));
            Assert.That(result[1].CompletedQuests, Is.EqualTo(1));
        }

        [Test]
        public async Task TopFiveUsersByCompletedQuestsAsync_WhenNoUsers_ReturnsEmpty()
        {
            // Arrange
            var (context, service) = CreateIsolatedContext();

            // Act
            var result = (await service.TopFiveUsersByCompletedQuestsAsync()).ToList();

            // Assert
            Assert.That(result, Is.Empty);

            context.Dispose();
        }

        [Test]
        public async Task TopFiveUsersByCompletedQuestsAsync_WithMoreThanFiveUsers_ReturnsTopFiveOrdered()
        {
            // Arrange
            var (context, service) = CreateIsolatedContext();
            SeedUsersWithCompletedQuests(context, userCount: 7, adminIndex: 0, assignCompletedQuests: true);

            // Act
            var result = (await service.TopFiveUsersByCompletedQuestsAsync()).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(5));
            Assert.That(result.Any(u => u.DisplayName == "Dummy0"), Is.False);
            Assert.That(result[0].DisplayName, Is.EqualTo("Dummy6"));
            Assert.That(result[0].CompletedQuests, Is.GreaterThanOrEqualTo(result[1].CompletedQuests));
        }
    }
}