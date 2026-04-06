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
    public class ProfileManagementServiceTests
    {
        private NexusDbContext dbContext;
        private IProfileManagementService profileManagementService;

        private static readonly Guid unknownUserId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

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

        private Profile CreateProfile(
            Guid? id = null,
            string displayName = "Dummy",
            string city = "DummyCity",
            string? bio = null,
            int xp = 0)
        {
            return new Profile
            {
                Id = id ?? Guid.NewGuid(),
                DisplayName = displayName,
                City = city,
                Bio = bio,
                ExperiencePoints = xp
            };
        }

        private Guid SeedProfile(
            string displayName = "Dummy",
            string city = "DummyCity",
            string? bio = null,
            int xp = 0)
        {
            var profile = CreateProfile(displayName: displayName, city: city, bio: bio, xp: xp);
            this.dbContext.Users.Add(profile);
            this.dbContext.SaveChanges();
            return profile.Id;
        }

        private void SeedProfiles(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                this.dbContext.Users.Add(CreateProfile(displayName: $"User{i}", city: $"City{i}"));
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

        private void SeedQuestWithRelations(Guid initiatorId, int questId, int interestId)
        {
            this.dbContext.Quests.Add(new Quest
            {
                Id = questId,
                Title = "Test Quest",
                Description = "Test Description",
                QuestInitiatorId = initiatorId,
                QuestInterest = new List<QuestInterest>(),
                QuestJoiners = new List<QuestJoiner>()
            });
            this.dbContext.QuestInterests.Add(new QuestInterest { QuestId = questId, InterestId = interestId });
            this.dbContext.QuestJoiners.Add(new QuestJoiner { QuestId = questId, ProfileId = initiatorId });
            this.dbContext.SaveChanges();
        }

        private Guid SeedAdminProfile(string displayName = "Admin", string city = "AdminCity")
        {
            var adminRoleId = Guid.NewGuid();
            var adminId = Guid.NewGuid();

            this.dbContext.Roles.Add(new IdentityRole<Guid> { Id = adminRoleId, Name = "Admin" });
            this.dbContext.Users.Add(CreateProfile(id: adminId, displayName: displayName, city: city));
            this.dbContext.UserRoles.Add(new IdentityUserRole<Guid> { UserId = adminId, RoleId = adminRoleId });
            this.dbContext.SaveChanges();

            return adminId;
        }

        private static ProfileManagementViewModel BuildEditModel(
            Guid id,
            string displayName = "EditedName",
            string city = "EditedCity",
            string? bio = null,
            int xp = 0)
        {
            return new ProfileManagementViewModel
            {
                Id = id,
                DisplayName = displayName,
                City = city,
                Bio = bio,
                ExperiencePoints = xp
            };
        }

        [Test]
        public async Task GetAllProfilesCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            SeedProfiles(3);

            // Act
            var count = await profileManagementService.GetAllProfilesCountAsync();

            // Assert
            Assert.That(count, Is.EqualTo(3));
        }

        [Test]
        public async Task GetAllProfilesCountAsync_WhenNoProfiles_ReturnsZero()
        {
            // Arrange

            // Act
            var count = await profileManagementService.GetAllProfilesCountAsync();

            // Assert
            Assert.That(count, Is.EqualTo(0));
        }

        [TestCase(1, 5, 5)]
        [TestCase(2, 5, 2)]
        public async Task GetAllProfilesAsAdminAsync_ReturnsPaginatedResults(
            int page, int pageSize, int expectedCount)
        {
            // Arrange
            SeedProfiles(7);

            // Act
            var result = (await profileManagementService
                .GetAllProfilesAsAdminAsync(page, pageSize))
                .ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(expectedCount));
            Assert.That(result, Has.All.Matches<ProfileManagementViewModel>(p => p.DisplayName.StartsWith("User")));
        }

        [Test]
        public async Task GetAllProfilesAsAdminAsync_WhenNoProfiles_ReturnsEmpty()
        {
            // Arrange 

            // Act
            var result = (await profileManagementService
                .GetAllProfilesAsAdminAsync(1, 10))
                .ToList();

            // Assert
            Assert.That(result, Is.Empty);
        }


        [Test]
        public async Task GetProfileToEditAsAdminAsync_WhenUserExists_ReturnsProfile()
        {
            // Arrange
            var userId = SeedProfile(displayName: "Vee", city: "NightCity");

            // Act
            var result = await profileManagementService.GetProfileToEditAsAdminAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(userId));
            Assert.That(result.DisplayName, Is.EqualTo("Vee"));
            Assert.That(result.City, Is.EqualTo("NightCity"));
        }

        [Test]
        public void GetProfileToEditAsAdminAsync_WhenProfileNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange 

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => profileManagementService.GetProfileToEditAsAdminAsync(unknownUserId));
        }

        [Test]
        public async Task EditProfileAsAdminAsync_UpdatesProfile()
        {
            // Arrange
            var userId = SeedProfile(displayName: "Ezio", city: "Florence");
            var editModel = BuildEditModel(userId, displayName: "Ezio Auditore", city: "Venice");

            // Act
            await profileManagementService.EditProfileAsAdminAsync(userId, editModel);

            // Assert
            var updated = dbContext.Users.First(u => u.Id == userId);
            Assert.That(updated.DisplayName, Is.EqualTo("Ezio Auditore"));
            Assert.That(updated.City, Is.EqualTo("Venice"));
        }

        [Test]
        public void EditProfileAsAdminAsync_WhenProfileNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var editModel = BuildEditModel(unknownUserId);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => profileManagementService.EditProfileAsAdminAsync(unknownUserId, editModel));
        }

        [Test]
        public async Task DeleteProfileAsAdminAsync_DeletesProfile()
        {
            // Arrange
            var userId = SeedProfile(displayName: "Peter Parker", city: "NY");

            // Act
            await profileManagementService.DeleteProfileAsAdminAsync(userId);

            // Assert
            Assert.That(dbContext.Users.FirstOrDefault(u => u.Id == userId), Is.Null);
        }

        [Test]
        public void DeleteProfileAsAdminAsync_WhenProfileNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => profileManagementService.DeleteProfileAsAdminAsync(unknownUserId));
        }

        [Test]
        public async Task DeleteProfileAsAdminAsync_DeletesProfileAndRelatedQuestData()
        {
            // Arrange
            var userId = SeedProfile(displayName: "Kiryu", city: "Japan");
            SeedInterests((1, "Mahjong"));
            SeedQuestWithRelations(userId, questId: 1, interestId: 1);

            // Act
            await profileManagementService.DeleteProfileAsAdminAsync(userId);

            // Assert
            Assert.That(dbContext.Users.FirstOrDefault(u => u.Id == userId), Is.Null);
            Assert.That(dbContext.Quests.FirstOrDefault(q => q.Id == 1), Is.Null);
            Assert.That(dbContext.QuestInterests.Where(qi => qi.QuestId == 1).ToList(), Is.Empty);
            Assert.That(dbContext.QuestJoiners.Where(qj => qj.QuestId == 1).ToList(), Is.Empty);
        }

        [Test]
        public void DeleteProfileAsAdminAsync_WhenProfileIsAdmin_ThrowsUnauthorizedException()
        {
            // Arrange
            var adminId = SeedAdminProfile();

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedException>(
                () => profileManagementService.DeleteProfileAsAdminAsync(adminId));
        }
    }
}