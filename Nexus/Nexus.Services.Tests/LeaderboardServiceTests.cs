using System.Data;
using System.Runtime.Intrinsics.X86;
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
        private static readonly Guid adminRoleId = Guid.NewGuid();
        private static readonly Guid userRoleId = Guid.NewGuid();


        [SetUp]
        public void Setup()
        {
            // Arange for most tests
            var options = new DbContextOptionsBuilder<NexusDbContext>()
                .UseInMemoryDatabase("DummyDatabaseNexus" + Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new NexusDbContext(options);

            IdentityRole<Guid> adminRole = new() { Id = adminRoleId, Name = "Admin"};
            IdentityRole<Guid> userRole = new() { Id = userRoleId, Name = "User"};
            this.dbContext.Roles.AddRange(adminRole, userRole);

           
           Profile admin = new ()
           { 
               Id = Guid.Parse("8d7b0283-56a0-4c40-85d3-d1be04437f3e"),
               DisplayName = "AdminDummy", 
               City = "NightCity",
               ExperiencePoints = 8000, 
               JoinedQuests = new List<QuestJoiner>() 
           };
            Profile userOne = new()
            {
                Id = Guid.Parse("dd172857-29d5-4d62-9850-5221384ed08f"),
                DisplayName = "AlexDummy",
                City = "NightCity",
                ExperiencePoints = 4000,
                JoinedQuests = new List<QuestJoiner>()
            };
            Profile userTwo = new()
            {
                Id = Guid.Parse("d57e9e71-cc0f-4ddf-ae83-52dc4dfc2be7"),
                DisplayName = "YoanaDummy",
                City = "NightCity",
                ExperiencePoints = 2000,
                JoinedQuests = new List<QuestJoiner>()
            };
            this.dbContext.Users.AddRange(admin, userOne, userTwo);

            List<IdentityUserRole<Guid>> userRoles = new()
            {
                new IdentityUserRole<Guid> { UserId = admin.Id, RoleId = adminRoleId },
                new IdentityUserRole<Guid> { UserId = userOne.Id, RoleId = userRoleId },
                new IdentityUserRole<Guid> { UserId = userTwo.Id, RoleId = userRoleId }
            };
            this.dbContext.UserRoles.AddRange(userRoles);

            Quest questOne = new() { Id = 100, Status = QuestStatus.Completed, Title = "CompletedQuestOne", Description= "Testing" };
            Quest questTwo = new() { Id = 200, Status = QuestStatus.Completed,  Title = "CompletedQuestTwo", Description = "Testing" };
            Quest questThree = new() { Id = 300, Status = QuestStatus.Active,  Title = "ActiveQuestOne", Description = "Testing" };
            this.dbContext.Quests.AddRange(questOne, questTwo, questThree);

            List<QuestJoiner> questJoiners = new()
            {
                new QuestJoiner { ProfileId = userOne.Id, QuestId = questOne.Id, Quest = questOne },
                new QuestJoiner { ProfileId = userOne.Id, QuestId = questTwo.Id, Quest = questTwo },
                new QuestJoiner { ProfileId = userTwo.Id, QuestId = questOne.Id, Quest = questOne },
                new QuestJoiner { ProfileId = userTwo.Id, QuestId = questThree.Id, Quest = questThree },
                new QuestJoiner { ProfileId = admin.Id, QuestId = questOne.Id, Quest = questOne }
            };
            this.dbContext.QuestJoiners.AddRange(questJoiners);
            this.dbContext.SaveChanges();

            leaderboardService = new LeaderboardService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        [Test]
        public async Task TopFiveUsersByLevelAsync_ExcludeAdminsAndOrdersByXP()
        {
            //Arrange is in setup
            //Act
            var result = (await leaderboardService.TopFiveUsersByLevelAsync()).ToList();

            //Assert
            Assert.AreEqual(2, result.Count); 
            Assert.AreEqual("AlexDummy", result[0].DisplayName); 
            Assert.AreEqual("YoanaDummy", result[1].DisplayName);
            Assert.IsFalse(result.Any(u => u.DisplayName == "AdminDummy")); 
        }

        [Test]
        public async Task TopFiveUsersByCompletedQuestsAsync_OrderedByCompletedQuestsAndExludesAdmin()
        {
            //Arrange is in setup
            //Act
            var result = (await leaderboardService.TopFiveUsersByCompletedQuestsAsync()).ToList();

            //Assert
            Assert.AreEqual(2, result.Count); 
            Assert.AreEqual("AlexDummy", result[0].DisplayName); 
            Assert.AreEqual(2, result[0].CompletedQuests);
            Assert.AreEqual("YoanaDummy", result[1].DisplayName); 
            Assert.AreEqual(1, result[1].CompletedQuests);
            Assert.IsFalse(result.Any(u => u.DisplayName == "AdminDummy"));
        }

        [Test]
        public async Task TopFiveUsersByLevelAsync_ReturnsEmpty_WhenNoUsers()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<NexusDbContext>()
                .UseInMemoryDatabase("DummyNexusDb" + Guid.NewGuid().ToString())
                .Options;
            NexusDbContext emptyContext = new(options);
            LeaderboardService service = new(emptyContext);

            //Act
            var result = (await service.TopFiveUsersByLevelAsync()).ToList();
            
            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task TopFiveUsersByCompletedQuestsAsync_ReturnsEmpty_WhenNoUsers()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<NexusDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            NexusDbContext emptyContext = new(options);
            LeaderboardService service = new(emptyContext);
            //Act
            var result = (await service.TopFiveUsersByCompletedQuestsAsync()).ToList();

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task TopFiveUsersByLevelAsync_ReturnsAtMostFiveUsers()
        {
           
            var options = new DbContextOptionsBuilder<NexusDbContext>()
                .UseInMemoryDatabase("DummyNexusDb" + Guid.NewGuid().ToString())
                .Options;
            NexusDbContext context = new(options);


            IdentityRole<Guid> adminRole = new () { Id = adminRoleId, Name = "Admin" };
            IdentityRole<Guid> userRole = new() { Id = userRoleId, Name = "User" };
            context.Roles.AddRange(adminRole, userRole);

            List<Profile> users = new();
            List<IdentityUserRole<Guid>> userRoles = new();
            for (int i = 0; i < 7; i++)
            {
                Profile user = new()
                {
                    Id = Guid.NewGuid(),
                    DisplayName = $"Dummy{i}",
                    City = "NightCity",
                    ExperiencePoints = 1000 + i * 2,
                    JoinedQuests = new List<QuestJoiner>()
                };
                users.Add(user);
                userRoles.Add(new IdentityUserRole<Guid>
                {
                    UserId = user.Id,
                    RoleId = (i == 0) ? adminRoleId : userRoleId 
                });
            }
            context.Users.AddRange(users);
            context.UserRoles.AddRange(userRoles);
            context.SaveChanges();

            LeaderboardService service = new(context);

            // Act
            var result = (await service.TopFiveUsersByLevelAsync()).ToList();

            // Assert
            Assert.LessOrEqual(result.Count, 5);
            Assert.IsFalse(result.Any(u => u.DisplayName == "Dummy0")); 
        }

        [Test]
        public async Task TopFiveUsersByCompletedQuestsAsync_ReturnsAtMostFiveUsers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NexusDbContext>()
                .UseInMemoryDatabase("DummyNexusDb" + Guid.NewGuid().ToString())
                .Options;
            var context = new NexusDbContext(options);

            IdentityRole<Guid> adminRole = new() { Id = adminRoleId, Name = "Admin" };
            IdentityRole<Guid> userRole = new() { Id = userRoleId, Name = "User" };
            context.Roles.AddRange(adminRole, userRole);

            List<Profile> users = new();
            List<IdentityUserRole<Guid>> userRoles = new();
            List<Quest> quests = new();
            List<QuestJoiner> questJoiners = new();
            for (int i = 0; i < 7; i++)
            {
                var user = new Profile
                {
                    Id = Guid.NewGuid(),
                    DisplayName = $"Dummy{i}",
                    City = "NightCity",
                    ExperiencePoints = 1000 + i * 2,
                    JoinedQuests = new List<QuestJoiner>()
                };
                users.Add(user);
                userRoles.Add(new IdentityUserRole<Guid>
                {
                    UserId = user.Id,
                    RoleId = (i == 0) ? adminRoleId : userRoleId 
                });

                for (int j = 0; j < i; j++)
                {
                    Quest quest = new() { Id = 1000 + i * 10 + j, Status = QuestStatus.Completed, Title = $"CompletedQuest{j}", Description = "Testing" };
                    quests.Add(quest);
                    questJoiners.Add(new QuestJoiner { ProfileId = user.Id, QuestId = quest.Id, Quest = quest });
                }
            }
            context.Users.AddRange(users);
            context.UserRoles.AddRange(userRoles);
            context.Quests.AddRange(quests);
            context.QuestJoiners.AddRange(questJoiners);
            context.SaveChanges();

            LeaderboardService service = new(context);

            // Act
            var result = (await service.TopFiveUsersByCompletedQuestsAsync()).ToList();

            // Assert
            Assert.AreEqual(5, result.Count);
            Assert.IsFalse(result.Any(u => u.DisplayName == "Dummy0")); 
                                                                     
            Assert.AreEqual("Dummy6", result[0].DisplayName);
        }
    }
}
