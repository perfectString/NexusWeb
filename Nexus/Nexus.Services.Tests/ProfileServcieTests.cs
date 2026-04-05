
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

        private static readonly Guid adminRoleId = Guid.NewGuid();
        private static readonly Guid userRoleId = Guid.NewGuid();

        private static readonly Guid AdminId = Guid.Parse("8d7b0283-56a0-4c40-85d3-d1be04437f3e");
        private static readonly Guid userOneId = Guid.Parse("dd172857-29d5-4d62-9850-5221384ed08f");
        private static readonly Guid userTwoId = Guid.Parse("d57e9e71-cc0f-4ddf-ae83-52dc4dfc2be7");


        [SetUp]
        public void Setup()
        {
            //Arrange for most cases
            var options = new DbContextOptionsBuilder<NexusDbContext>()
                .UseInMemoryDatabase("NexusDummyDb" + Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new NexusDbContext(options);


            IdentityRole<Guid> adminRole = new() { Id = adminRoleId, Name = "Admin" };
            IdentityRole<Guid> userRole = new() { Id = userRoleId, Name = "User" };
            dbContext.Roles.AddRange(adminRole, userRole);

            Profile admin = new()
            {
                Id = AdminId,
                DisplayName = "AdminDummy",
                City = "NightCity",
                ExperiencePoints = 8000,
                JoinedQuests = new List<QuestJoiner>()
            };
            Profile userOne = new()
            {
                Id = userOneId,
                DisplayName = "AlexDummy",
                City = "NightCity",
                ExperiencePoints = 4000,
                JoinedQuests = new List<QuestJoiner>()
            };
            Profile userTwo = new()
            {
                Id = userTwoId,
                DisplayName = "YoanaDummy",
                City = "NightCity",
                ExperiencePoints = 2000,
                JoinedQuests = new List<QuestJoiner>()
            };
            this.dbContext.Users.AddRange(admin, userTwo, userOne);

            this.dbContext.UserRoles.Add(new IdentityUserRole<Guid> { UserId = admin.Id, RoleId = adminRoleId });
            this.dbContext.UserRoles.Add(new IdentityUserRole<Guid> { UserId = userOne.Id, RoleId = userRoleId });
            this.dbContext.UserRoles.Add(new IdentityUserRole<Guid> { UserId = userTwo.Id, RoleId = userRoleId });

            this.dbContext.SaveChanges();

            profileService = new ProfileService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task GetAllProfilesCountAsync_ExcludesAdmins()
        {
            //Arrange in setup

            // Act
            var count = await profileService.GetAllProfilesCountAsync();

            // Assert
            Assert.AreEqual(2, count);
        }

        [Test]
        public async Task GetAllProfilesByNameThenByAgeThenByCityAscAsync_ExcludesAdmins()
        {
            //Arrange in setup

            // Act
            var result = (await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync(1, 10))
                .ToList();

            // Assert
            Assert.IsFalse(result.Any(p => p.DisplayName == "AdminDummy"));
        }

        [Test]
        public async Task GetAllProfilesByNameThenByAgeThenByCityAscAsync_OrderedCorrectly()
        {
            // Arrange
            Profile userA1 = new() { Id = Guid.NewGuid(), DisplayName = "SameName", Age = 20, City = "A" };
            Profile userA2 = new() { Id = Guid.NewGuid(), DisplayName = "SameName", Age = 20, City = "B" };
            Profile userB = new() { Id = Guid.NewGuid(), DisplayName = "AnotherName", Age = 30, City = "C" };
            this.dbContext.Users.AddRange(userA1, userA2, userB);
            this.dbContext.UserRoles.AddRange(
                new IdentityUserRole<Guid> { UserId = userA1.Id, RoleId = userRoleId },
                new IdentityUserRole<Guid> { UserId = userA2.Id, RoleId = userRoleId },
                new IdentityUserRole<Guid> { UserId = userB.Id, RoleId = userRoleId }
            );
            this.dbContext.SaveChanges();

            // Act
            var result = (await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync(1, 10))
                .ToList();

            // Assert
            var ordered = result
                .OrderBy(p => p.DisplayName)
                .ThenBy(p => p.Age)
                .ThenBy(p => p.City)
                .ToList();

            CollectionAssert.AreEqual(ordered, result);
        }

        [Test]
        public async Task GetAllProfilesByNameThenByAgeThenByCityAscAsync_PaginationShouldWork()
        {
            // Arrange

            //I already have two users in the DB because of the setup
            for (int i = 0; i < 4; i++)
            {
                Profile user = new() 
                { 
                    Id = Guid.NewGuid(),
                    DisplayName = $"Dummy{i}",
                    Age = 20, 
                    City = "City" 
                };
                this.dbContext.Users.Add(user);
                this.dbContext.UserRoles.Add(new IdentityUserRole<Guid> { UserId = user.Id, RoleId = userRoleId });
            }
            this.dbContext.SaveChanges();

            // Act
            var page1 = (await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync(1, 5))
                .ToList();

            var page2 = (await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync(2, 5))
                .ToList();

            // Assert
            Assert.AreEqual(5, page1.Count);
            Assert.AreEqual(1, page2.Count);
        }

        [Test]
        public async Task GetAllProfilesByNameThenByAgeThenByCityAscAsync_WithOnlyAdminAndNoUsers_ReturnsEmpty()
        {
            // Arrange
            this.dbContext
                .Users
                .RemoveRange(dbContext.Users
                .Where(u => u.DisplayName != "AdminDummy"));

            this.dbContext.SaveChanges();

            // Act
            var result = (await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync(1, 10))
                .ToList();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllProfilesByNameThenByAgeThenByCityAscAsync_HandlesUsersWithNoInterests()
        {
            // Arrange in setup

            // Act
            var result = (await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync(1, 10))
                .ToList();

            // Assert
            Assert.IsTrue(result.All(p => p.Interests != null));
        }

        [Test]
        public async Task GetAllProfilesByNameThenByAgeThenByCityAscAsync_HandlesUsersWithInterests()
        {
            // Arrange
            Interest interest = new() { Id = 105, Name = "Chess" };
            this.dbContext.Interests.Add(interest);
            this.dbContext.ProfileInterests.Add(new ProfileInterest
            {
                ProfileId = dbContext
                .Users
                .First(u => u.DisplayName == "AlexDummy").Id,
                InterestId = interest.Id,
                Interest = interest
            });
            this.dbContext.SaveChanges();

            // Act
            var result = (await profileService
                .GetAllProfilesByNameThenByAgeThenByCityAscAsync(1, 10))
                .ToList();

            // Assert
            var userWithInterest = result
                .FirstOrDefault(p => p.DisplayName == "AlexDummy");
            Assert.IsNotNull(userWithInterest);
            Assert.Contains("Chess", userWithInterest.Interests);
        }

        [Test]
        public async Task GetAllInterestsAsync_ReturnsAllInterests_OrderedByName()
        {
            // Arrange
            List<Interest> interests = new()
            {
                new Interest { Id = 1, Name = "Cars" },
                new Interest { Id = 2, Name = "Running" },
                new Interest { Id = 3, Name = "Basketball" }
            };
            this.dbContext.Interests.AddRange(interests);
            this.dbContext.SaveChanges();

            // Act
            var result = await profileService.GetAllInterestsAsync();

            // Assert
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(
                new[] { "Basketball", "Cars", "Running" },
                result.Select(i => i.Name).ToArray()
            );
        }

        [Test]
        public async Task GetAllInterestsAsync_ReturnsEmpty_WhenNoInterests()
        {
            // Arrange

            // Act
            var result = await profileService.GetAllInterestsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetEditProfileViewModelWithAllInterestsAsync_ReturnsCorrectViewModel()
        {
            // Arrange
            Interest interestOne = new() { Id = 1, Name = "Chess" };
            Interest interestTwo = new() { Id = 2, Name = "Coding" };
            this.dbContext.Interests.AddRange(interestOne, interestTwo);
            this.dbContext.ProfileInterests.Add(new ProfileInterest
            {
                ProfileId = userOneId,
                InterestId = 1,
                Interest = interestOne
            });
            this.dbContext.SaveChanges();

            // Act
            var result = await profileService
                .GetEditProfileViewModelWithAllInterestsAsync
                (userOneId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("AlexDummy", result.DisplayName);
            Assert.AreEqual("NightCity", result.City);
            Assert.AreEqual(2, result.AvailableInterests.Count); 
            Assert.AreEqual(1, result.InterestId.Count); 
            Assert.AreEqual(1, result.InterestId[0]);
        }

        [Test]
        public void GetEditProfileViewModelWithAllInterestsAsync_WhenUserNotFound_ThrowsException()
        {
            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await profileService
                .GetEditProfileViewModelWithAllInterestsAsync(Guid.NewGuid());
            });
        }

        [Test]
        public async Task EditProfileAsync_UpdatesProfileAndInterests()
        {
            // Arrange
            Interest interestOne = new () { Id = 1, Name = "Chess" };
            Interest interestTwo = new() { Id = 2, Name = "Coding" };
            dbContext.Interests.AddRange(interestOne, interestTwo);
            dbContext.ProfileInterests.Add(new ProfileInterest
            {
                ProfileId = userOneId,
                InterestId = 1,
                Interest = interestOne
            });
            dbContext.SaveChanges();

            ProfileEditViewModel editModel = new()
            {
                DisplayName = "UpdatedAlexDummy",
                Age = 30,
                City = "NewCity",
                Bio = "UpdatedBio",
                DesiredConnection = ConnectionType.Groups,
                InterestId = new List<int>{2} 
            };

            // Act
            await profileService
                .EditProfileAsync(userOneId,
                editModel);

            // Assert
            var updatedUser = dbContext
                .Users
                .First(u => u.Id == userOneId);

            Assert.AreEqual("UpdatedAlexDummy", updatedUser.DisplayName);
            Assert.AreEqual(30, updatedUser.Age);
            Assert.AreEqual("NewCity", updatedUser.City);
            Assert.AreEqual("UpdatedBio", updatedUser.Bio);
            Assert.AreEqual(ConnectionType.Groups, updatedUser.DesiredConnection);

            var profileInterests = dbContext
                .ProfileInterests
                .Where(pi => pi.ProfileId == updatedUser.Id)
                .ToList();

            Assert.AreEqual(1, profileInterests.Count);
            Assert.AreEqual(2, profileInterests[0].InterestId);
        }

        [Test]
        public void EditProfileAsync_WhenUserNotFound_ThrowsException()
        {
            // Arrange
            var editModel = new ProfileEditViewModel
            {
                DisplayName = "NoUser",
                Age = 20,
                City = "Nowhere",
                Bio = "None",
                DesiredConnection = 0,
                InterestId = new List<int>()
            };

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await profileService
                .EditProfileAsync(Guid.NewGuid(), editModel);
            });
        }

        [Test]
        public async Task GetCurrentUserProfile_ProfileWithInterests_ShouldBeReturnedCorrectly()
        {
            // Arrange
            Interest interest = new() { Id = 101, Name = "Basketball" };
            dbContext.Interests.Add(interest);
            dbContext.ProfileInterests.Add(new ProfileInterest
            {
                ProfileId = userOneId,
                InterestId = interest.Id,
                Interest = interest
            });
            this.dbContext.SaveChanges();

            // Act
            var result = await profileService
                .GetCurrentUserProfile(userOneId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("AlexDummy", result.DisplayName);
            Assert.AreEqual("NightCity", result.City);
            Assert.Contains("Basketball", result.Interests);
        }

        [Test]
        public async Task GetCurrentUserProfile_ProfileWithNoInterests_ShouldBeReturnedCorrectly()
        {
            // Arrange - userTwo has no interests in setup

            // Act
            var result = await profileService
                .GetCurrentUserProfile(userTwoId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("YoanaDummy", result.DisplayName);
            Assert.IsNotNull(result.Interests);
            Assert.IsEmpty(result.Interests);
        }

        [Test]
        public void GetCurrentUserProfile_WhenUserNotFound_ThrowsException()
        {
            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await profileService.GetCurrentUserProfile(Guid.NewGuid());
            });
        }
    }
}




