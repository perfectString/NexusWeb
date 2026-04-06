using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Data.Models;
using Nexus.Data.Services.Core;
using Nexus.GCommon.Enums;
using Nexus.GCommon.Exceptions;
using Nexus.ViewModels.Profile;

namespace Nexus.Services.Tests
{
    [TestFixture]
    public class ProfileServiceTests
    {
        private NexusDbContext dbContext;
        private ProfileService profileService;

        private static readonly Guid adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private static readonly Guid userRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        private static readonly Guid adminId = Guid.Parse("8d7b0283-56a0-4c40-85d3-d1be04437f3e");
        private static readonly Guid userOneId = Guid.Parse("dd172857-29d5-4d62-9850-5221384ed08f");
        private static readonly Guid userTwoId = Guid.Parse("d57e9e71-cc0f-4ddf-ae83-52dc4dfc2be7");
        private static readonly Guid unknownUserId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NexusDbContext>()
                .UseInMemoryDatabase("NexusDummyDb" + Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new NexusDbContext(options);

            dbContext.Roles.AddRange(
                new IdentityRole<Guid> { Id = adminRoleId, Name = "Admin" },
                new IdentityRole<Guid> { Id = userRoleId, Name = "User" });

            dbContext.Users.AddRange(
                CreateProfile(adminId, "AdminDummy", "NightCity", xp: 8000),
                CreateProfile(userOneId, "AlexDummy", "NightCity", xp: 4000),
                CreateProfile(userTwoId, "YoanaDummy", "NightCity", xp: 2000));

            dbContext.UserRoles.AddRange(
                new IdentityUserRole<Guid> { UserId = adminId, RoleId = adminRoleId },
                new IdentityUserRole<Guid> { UserId = userOneId, RoleId = userRoleId },
                new IdentityUserRole<Guid> { UserId = userTwoId, RoleId = userRoleId });

            dbContext.SaveChanges();

            profileService = new ProfileService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private static Profile CreateProfile(
            Guid id,
            string displayName,
            string city,
            int age = 0,
            int xp = 0,
            string? bio = null,
            ConnectionType connection = ConnectionType.Friends)
        {
            return new Profile
            {
                Id = id,
                DisplayName = displayName,
                City = city,
                Age = age,
                Bio = bio,
                ExperiencePoints = xp,
                DesiredConnection = connection,
                JoinedQuests = new List<QuestJoiner>()
            };
        }

        private void SeedUserWithRole(
            Guid id,
            string displayName,
            string city,
            int age = 0,
            Guid? roleId = null)
        {
            this.dbContext.Users.Add(CreateProfile(id, displayName, city, age));
            this.dbContext.UserRoles.Add(new IdentityUserRole<Guid>
            {
                UserId = id,
                RoleId = roleId ?? userRoleId
            });
        }

        private void SeedInterests(params (int Id, string Name)[] interests)
        {
            foreach (var (id, name) in interests)
            {
                this.dbContext.Interests.Add(new Interest { Id = id, Name = name });
            }
            this.dbContext.SaveChanges();
        }

        private void SeedProfileInterests(params (Guid ProfileId, int InterestId)[] links)
        {
            foreach (var (profileId, interestId) in links)
            {
                this.dbContext.ProfileInterests.Add(new ProfileInterest
                {
                    ProfileId = profileId,
                    InterestId = interestId
                });
            }
            this.dbContext.SaveChanges();
        }

        private static ProfileEditViewModel BuildEditModel(
            string displayName = "UpdatedName",
            int age = 25,
            string city = "UpdatedCity",
            string? bio = null,
            ConnectionType connection = ConnectionType.Friends,
            List<int>? interestIds = null)
        {
            return new ProfileEditViewModel
            {
                DisplayName = displayName,
                Age = age,
                City = city,
                Bio = bio,
                DesiredConnection = connection,
                InterestId = interestIds ?? new List<int>()
            };
        }

        [Test]
        public async Task GetAllProfilesCountAsync_ExcludesAdmins()
        {
            // Arrange

            // Act
            var count = await profileService.GetAllProfilesCountAsync();

            // Assert
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllProfilesByNameThenByAgeThenByCityAscAsync_ExcludesAdmins()
        {
            // Arrange

            // Act
            var result = (await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync(1, 10))
                .ToList();

            // Assert
            Assert.That(result.Any(p => p.DisplayName == "AdminDummy"), Is.False);
        }

        [Test]
        public async Task GetAllProfilesByNameThenByAgeThenByCityAscAsync_OrderedByNameThenAgeThenCity()
        {
            // Arrange
            var idA1 = Guid.NewGuid();
            var idA2 = Guid.NewGuid();
            var idB = Guid.NewGuid();
            SeedUserWithRole(idA1, "SameName", "A", age: 20);
            SeedUserWithRole(idA2, "SameName", "B", age: 20);
            SeedUserWithRole(idB, "AnotherName", "C", age: 30);
            this.dbContext.SaveChanges();

            // Act
            var result = (await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync(1, 10))
                .ToList();

            // Assert
            var displayNames = result.Select(p => p.DisplayName).ToList();
            var ordered = result
                .OrderBy(p => p.DisplayName)
                .ThenBy(p => p.Age)
                .ThenBy(p => p.City)
                .Select(p => p.DisplayName)
                .ToList();
            Assert.That(displayNames, Is.EqualTo(ordered));
        }

        [TestCase(1, 5, 5)]
        [TestCase(2, 5, 1)]
        public async Task GetAllProfilesByNameThenByAgeThenByCityAscAsync_Pagination_ReturnsExpectedPageSize(
            int page, int pageSize, int expectedCount)
        {
            // Arrange
            for (int i = 0; i < 4; i++)
            {
                SeedUserWithRole(Guid.NewGuid(), $"Dummy{i}", "City", age: 20);
            }
            this.dbContext.SaveChanges();

            // Act
            var result = (await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync(page, pageSize))
                .ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(expectedCount));
        }

        [Test]
        public async Task GetAllProfilesByNameThenByAgeThenByCityAscAsync_WhenOnlyAdmins_ReturnsEmpty()
        {
            // Arrange
            this.dbContext.Users.RemoveRange(
                dbContext.Users.Where(u => u.DisplayName != "AdminDummy"));
            this.dbContext.SaveChanges();

            // Act
            var result = (await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync(1, 10))
                .ToList();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAllProfilesByNameThenByAgeThenByCityAscAsync_UsersWithNoInterests_ReturnsEmptyInterestsList()
        {
            // Arrange 

            // Act
            var result = (await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync(1, 10))
                .ToList();

            // Assert
            Assert.That(result, Has.All.Matches<ProfileViewModel>(p => p.Interests != null));
            Assert.That(result, Has.All.Matches<ProfileViewModel>(p => p.Interests.Count == 0));
        }

        [Test]
        public async Task GetAllProfilesByNameThenByAgeThenByCityAscAsync_UsersWithInterests_ReturnsInterestNames()
        {
            // Arrange
            SeedInterests((105, "Chess"));
            SeedProfileInterests((userOneId, 105));

            // Act
            var result = (await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync(1, 10))
                .ToList();
            var userWithInterest = result.FirstOrDefault(p => p.DisplayName == "AlexDummy");

            // Assert
            Assert.That(userWithInterest, Is.Not.Null);
            Assert.That(userWithInterest!.Interests, Does.Contain("Chess"));
        }

        [Test]
        public async Task GetAllInterestsAsync_ReturnsAllInterests_OrderedByName()
        {
            // Arrange
            SeedInterests((1, "Cars"), (2, "Running"), (3, "Basketball"));

            // Act
            var result = await profileService.GetAllInterestsAsync();

            // Assert
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(
                result.Select(i => i.Name).ToList(),
                Is.EqualTo(new[] { "Basketball", "Cars", "Running" }));
        }

        [Test]
        public async Task GetAllInterestsAsync_WhenNoInterests_ReturnsEmpty()
        {
            // Arrange 

            // Act
            var result = await profileService.GetAllInterestsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }


        [Test]
        public async Task GetEditProfileViewModelWithAllInterestsAsync_ReturnsCorrectViewModel()
        {
            // Arrange
            SeedInterests((1, "Chess"), (2, "Coding"));
            SeedProfileInterests((userOneId, 1));

            // Act
            var result = await profileService.GetEditProfileViewModelWithAllInterestsAsync(userOneId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.DisplayName, Is.EqualTo("AlexDummy"));
            Assert.That(result.City, Is.EqualTo("NightCity"));
            Assert.That(result.AvailableInterests, Has.Count.EqualTo(2));
            Assert.That(result.InterestId, Has.Count.EqualTo(1));
            Assert.That(result.InterestId[0], Is.EqualTo(1));
        }

        [Test]
        public void GetEditProfileViewModelWithAllInterestsAsync_WhenUserNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => profileService.GetEditProfileViewModelWithAllInterestsAsync(unknownUserId));
        }

        // EditProfileAsync

        [Test]
        public async Task EditProfileAsync_UpdatesProfileAndInterests()
        {
            // Arrange
            SeedInterests((1, "Chess"), (2, "Coding"));
            SeedProfileInterests((userOneId, 1));
            var editModel = BuildEditModel(
                displayName: "UpdatedAlexDummy",
                age: 30,
                city: "NewCity",
                bio: "UpdatedBio",
                connection: ConnectionType.Groups,
                interestIds: new List<int> { 2 });

            // Act
            await profileService.EditProfileAsync(userOneId, editModel);

            // Assert
            var updatedUser = dbContext.Users.First(u => u.Id == userOneId);
            Assert.That(updatedUser.DisplayName, Is.EqualTo("UpdatedAlexDummy"));
            Assert.That(updatedUser.Age, Is.EqualTo(30));
            Assert.That(updatedUser.City, Is.EqualTo("NewCity"));
            Assert.That(updatedUser.Bio, Is.EqualTo("UpdatedBio"));
            Assert.That(updatedUser.DesiredConnection, Is.EqualTo(ConnectionType.Groups));

            var profileInterests = dbContext.ProfileInterests
                .Where(pi => pi.ProfileId == userOneId)
                .ToList();
            Assert.That(profileInterests, Has.Count.EqualTo(1));
            Assert.That(profileInterests[0].InterestId, Is.EqualTo(2));
        }

        [Test]
        public void EditProfileAsync_WhenUserNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var editModel = BuildEditModel();

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => profileService.EditProfileAsync(unknownUserId, editModel));
        }

        [Test]
        public async Task GetCurrentUserProfile_WithInterests_ReturnsProfileWithInterestNames()
        {
            // Arrange
            SeedInterests((101, "Basketball"));
            SeedProfileInterests((userOneId, 101));

            // Act
            var result = await profileService.GetCurrentUserProfile(userOneId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.DisplayName, Is.EqualTo("AlexDummy"));
            Assert.That(result.City, Is.EqualTo("NightCity"));
            Assert.That(result.Interests, Does.Contain("Basketball"));
        }

        [Test]
        public async Task GetCurrentUserProfile_WithNoInterests_ReturnsEmptyInterestsList()
        {
            // Arrange 

            // Act
            var result = await profileService.GetCurrentUserProfile(userTwoId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.DisplayName, Is.EqualTo("YoanaDummy"));
            Assert.That(result.Interests, Is.Not.Null);
            Assert.That(result.Interests, Is.Empty);
        }

        [Test]
        public void GetCurrentUserProfile_WhenUserNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange 

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(
                () => profileService.GetCurrentUserProfile(unknownUserId));
        }
    }
}