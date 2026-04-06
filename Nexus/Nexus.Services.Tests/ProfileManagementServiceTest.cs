using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Data.Models;
using Nexus.Data.Services.Core;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.GCommon.Exceptions;
using Nexus.ViewModels.Admin.Profile;

namespace Nexus.Services.Tests
{
    [TestFixture]
    public class ProfileManagementServiceTest
    {
        private NexusDbContext dbContext;
        private IProfileManagementService profileManagementService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NexusDbContext>()
                .UseInMemoryDatabase("DummyNexusDb" + Guid.NewGuid())
                .Options;

            this.dbContext = new NexusDbContext(options);
            this.profileManagementService = new ProfileManagementService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        [Test]
        public async Task GetAllProfilesCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            this.dbContext.Users.Add(new Profile { Id = Guid.NewGuid(), DisplayName = "VeeDummy", City = "NightCity" });
            this.dbContext.Users.Add(new Profile { Id = Guid.NewGuid(), DisplayName = "Geralt", City = "Novigrad" });
            this.dbContext.Users.Add(new Profile { Id = Guid.NewGuid(), DisplayName = "Admin", City = "AdminCity" });
            this.dbContext.SaveChanges();

            // Act
            var count = await profileManagementService.GetAllProfilesCountAsync();

            // Assert
            Assert.AreEqual(3, count);
        }

        [Test]
        public async Task GetAllProfilesCountAsync_WhenNoProfiles_ReturnsZero()
        {

            // Act
            var count = await profileManagementService
                .GetAllProfilesCountAsync();

            // Assert
            Assert.AreEqual(0, count);
        }

        [Test]
        public async Task GetAllProfilesAsAdminAsync_ReturnsPagedProfiles()
        {
            // Arrange
            for (int i = 1; i <= 7; i++)
            {
                this.dbContext.Users.Add(new Profile { Id = Guid.NewGuid(), DisplayName = $"User{i}", City = $"City{i}" });
            }
            this.dbContext.SaveChanges();

            // Act
            var page1 = (await profileManagementService
                .GetAllProfilesAsAdminAsync(1, 5))
                .ToList();

            var page2 = (await profileManagementService
                .GetAllProfilesAsAdminAsync(2, 5))
                .ToList();

            // Assert
            Assert.AreEqual(5, page1.Count);
            Assert.AreEqual(2, page2.Count);
            Assert.IsTrue(page1.All(p => p.DisplayName
            .StartsWith("User")));
        }

        [Test]
        public async Task GetAllProfilesAsAdminAsync_WhenNoProfiles_ReturnsEmpty()
        {

            // Act
            var result = (await profileManagementService
                .GetAllProfilesAsAdminAsync(1, 10)).ToList();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetProfileToEditAsAdminAsync_WhenUserExists_ReturnsProfile()
        {
            // Arrange
            var userId = Guid.NewGuid();
            this.dbContext
                .Users
                .Add(new Profile { Id = userId, DisplayName = "Vee", City = "NightCity" });
            this.dbContext.SaveChanges();

            // Act
            var result = await profileManagementService
                .GetProfileToEditAsAdminAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual("Vee", result.DisplayName);
        }

        [Test]
        public void GetProfileToEditAsAdminAsync_WhenProfileNotFound_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await profileManagementService.GetProfileToEditAsAdminAsync(userId);
            });
        }

        [Test]
        public async Task EditProfileAsAdminAsync_UpdatesProfile()
        {
            // Arrange
            var userId = Guid.NewGuid();
            dbContext
                .Users
                .Add(new Profile { Id = userId, DisplayName = "Ezio", City = "Florence" });
            dbContext.SaveChanges();

            var editModel = new ProfileManagementViewModel
            {
                Id = userId,
                DisplayName = "Ezio Auditore",
                City = "Venice"

            };

            // Act
            await profileManagementService.EditProfileAsAdminAsync(userId, editModel);

            // Assert
            var updated = dbContext.Users.First(u => u.Id == userId);
            Assert.AreEqual("Ezio Auditore", updated.DisplayName);
            Assert.AreEqual("Venice", updated.City);
        }

        [Test]
        public void EditProfileAsAdminAsync_WhenProfileNotFound_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var editModel = new ProfileManagementViewModel
            {
                Id = userId,
                DisplayName = "ShouldNotExist",
                City = "Nowhere"
            };

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await profileManagementService
                .EditProfileAsAdminAsync(userId, editModel);
            });
        }

        [Test]
        public async Task DeleteProfileAsAdminAsync_DeletesProfile()
        {
            // Arrange
            var userId = Guid.NewGuid();
            this.dbContext
                .Users
                .Add(new Profile { Id = userId, DisplayName = "Peter Parker", City = "NY" });
            this.dbContext.SaveChanges();

            // Act
            await profileManagementService.DeleteProfileAsAdminAsync(userId);

            // Assert
            Assert.IsNull(dbContext.Users.FirstOrDefault(u => u.Id == userId));
        }

        [Test]
        public void DeleteProfileAsAdminAsync_WhenProfileNotFound_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await profileManagementService.DeleteProfileAsAdminAsync(userId);
            });
        }

        [Test]
        public async Task DeleteProfileAsAdminAsync_DeletesProfileAndRelatedQuestData()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var quest = new Quest
            {
                Id = 1,
                Title = "Dance Battle",
                Description = "Learn dance moves",
                QuestInitiatorId = userId,
                QuestInterest = new List<QuestInterest>(),
                QuestJoiners = new List<QuestJoiner>()
            };
            Interest interest = new() { Id = 1, Name = "Mahjong" };
            this.dbContext
                .Users
                .Add(new Profile { Id = userId, DisplayName = "Kiryu", City = "Japan" });

            this.dbContext.Interests.Add(interest);
            this.dbContext.Quests.Add(quest);
            this.dbContext
                .QuestInterests
                .Add(new QuestInterest { QuestId = quest.Id, InterestId = interest.Id });

            this.dbContext
                .QuestJoiners
                .Add(new QuestJoiner { QuestId = quest.Id, ProfileId = userId });
            this.dbContext.SaveChanges();

            // Act
            await profileManagementService.DeleteProfileAsAdminAsync(userId);

            // Assert
            Assert.IsNull(dbContext.Users.FirstOrDefault(u => u.Id == userId));
            Assert.IsEmpty(dbContext.QuestInterests.Where(qi => qi.QuestId == quest.Id));
            Assert.IsEmpty(dbContext.QuestJoiners.Where(qj => qj.QuestId == quest.Id));
            Assert.IsNull(dbContext.Quests.FirstOrDefault(q => q.Id == quest.Id));
        }
        [Test]
        public void DeleteProfileAsAdminAsync_WhenProfileIsAdmin_ThrowsException()
        {
            // Arrange
            var adminId = Guid.NewGuid();
            IdentityRole<Guid> adminRole = new() { Id = Guid.NewGuid(), Name = "Admin" };
            this.dbContext.Roles.Add(adminRole);

            this.dbContext
                .Users
                .Add(new Profile { Id = adminId, DisplayName = "Admin", City = "AdminCity" });
            
            this.dbContext
                .UserRoles
                .Add(new IdentityUserRole<Guid> { UserId = adminId, RoleId = adminRole.Id });

            this.dbContext.SaveChanges();

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedException>(async () =>
            {
                await profileManagementService.DeleteProfileAsAdminAsync(adminId);
            });
        }
    }
}
