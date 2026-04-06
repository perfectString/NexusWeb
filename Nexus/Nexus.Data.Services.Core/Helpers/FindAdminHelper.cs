using Microsoft.EntityFrameworkCore;

namespace Nexus.Data.Services.Core.Helpers
{
    public static class FindAdminHelper
    {
        public static async Task<List<Guid>> GetAdminUserIdsAsync(NexusDbContext dbContext)
        {
            Guid adminRoleId = await dbContext
                .Roles
                .Where(r => r.Name == "Admin")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            return await dbContext
                .UserRoles
                .Where(ur => ur.RoleId == adminRoleId)
                .Select(ur => ur.UserId)
                .ToListAsync();
        }
    }
}
