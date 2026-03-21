using Microsoft.AspNetCore.Identity;
using Nexus.Data.Seeding.Contracts;
using static Nexus.GCommon.Exceptions.ExceptionMessages;

namespace Nexus.Data.Seeding
{
    public class IdentitySeeder : IIdentitySeeder
    {
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly string[] roles = new[]
        {
          "Admin",
          "User"
        };

        public IdentitySeeder(RoleManager<IdentityRole<Guid>> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task SeedRolesAsync()
        {
           foreach (string role in roles)
           {
                bool roleExist = await roleManager.RoleExistsAsync(role);

                if (!roleExist)
                {
                  IdentityRole<Guid> newRole = new(role);
                  IdentityResult result = await roleManager
                        .CreateAsync(newRole);

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(string
                            .Format(RoleSeedingException, role));
                    }

                }
           }
        }
    }
}
