using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Data.Models;
using Nexus.Data.Services.Core.Helpers;

namespace Nexus.Services.Tests
{
    [TestFixture]
    public class FindAdminHelperTests
    {
        private NexusDbContext dbContext;

        private static readonly Guid adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private static readonly Guid userRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        private static readonly Guid adminOneId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        private static readonly Guid adminTwoId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        private static readonly Guid userOneId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        private static readonly Guid userTwoId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NexusDbContext>()
                .UseInMemoryDatabase("NexusDummyDb" + Guid.NewGuid())
                .Options;

            this.dbContext = new NexusDbContext(options);

            dbContext.Roles.AddRange(
                new IdentityRole<Guid> { Id = adminRoleId, Name = "Admin" },
                new IdentityRole<Guid> { Id = userRoleId, Name = "User" });

            dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        private void SeedProfileWithRole(Guid userId, Guid roleId, string displayName = "Dummy")
        {
            dbContext.Users.Add(new Profile
            {
                Id = userId,
                DisplayName = displayName,
                City = "TestCity"
            });
            dbContext.UserRoles.Add(new IdentityUserRole<Guid>
            {
                UserId = userId,
                RoleId = roleId
            });
            dbContext.SaveChanges();
        }

        [Test]
        public async Task GetAdminUserIdsAsync_WhenAdminsExist_ReturnsOnlyAdminIds()
        {
            // Arrange
            SeedProfileWithRole(adminOneId, adminRoleId, "Admin");
            SeedProfileWithRole(userOneId, userRoleId, "User");

            // Act
            var result = await FindAdminHelper.GetAdminUserIdsAsync(dbContext);

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result, Does.Contain(adminOneId));
        }

        [Test]
        public async Task GetAdminUserIdsAsync_WithMultipleAdmins_ReturnsAllAdminIds()
        {
            // Arrange
            SeedProfileWithRole(adminOneId, adminRoleId, "AdminOne");
            SeedProfileWithRole(adminTwoId, adminRoleId, "AdminTwo");
            SeedProfileWithRole(userOneId, userRoleId, "User");

            // Act
            var result = await FindAdminHelper.GetAdminUserIdsAsync(dbContext);

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Is.EquivalentTo(new[] { adminOneId, adminTwoId }));
        }

        [Test]
        public async Task GetAdminUserIdsAsync_WhenNoAdminRoleAssigned_ReturnsEmptyList()
        {
            // Arrange
            SeedProfileWithRole(userOneId, userRoleId, "UserOne");
            SeedProfileWithRole(userTwoId, userRoleId, "UserTwo");

            // Act
            var result = await FindAdminHelper.GetAdminUserIdsAsync(dbContext);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAdminUserIdsAsync_ReturnsList_NotNull()
        {
            // Arrange 

            // Act
            var result = await FindAdminHelper.GetAdminUserIdsAsync(dbContext);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<List<Guid>>());
        }
    }
}