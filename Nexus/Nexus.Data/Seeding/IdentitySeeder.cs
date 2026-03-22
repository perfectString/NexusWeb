using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Nexus.Data.Models;
using Nexus.Data.Seeding.Contracts;
using static Nexus.GCommon.Exceptions.ExceptionMessages;

namespace Nexus.Data.Seeding
{
    public class IdentitySeeder : IIdentitySeeder
    {
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly UserManager<Profile> userManager;
        private readonly IConfiguration configuration;
        private readonly string[] roles = new[]
        {
          "Admin",
          "User"
        };

        public IdentitySeeder(RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<Profile> userManager, IConfiguration configuration)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.configuration = configuration;
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
        public async Task SeedAdminAsync()
        {
            string adminEmail = configuration["Admin:Email"] 
                ?? throw new InvalidOperationException(AdminEmailException);
            string adminPass = configuration["Admin:Password"] 
                ?? throw new InvalidOperationException(AdminPassException);

           Profile? admin = await userManager.FindByEmailAsync(adminEmail);

           if (admin == null)
            {
                admin = new Profile()
                {
                    DisplayName = "Admin",
                    UserName = adminEmail,
                    City = "None",
                    Age = 99,
                    Bio = "Moderator of the website. Reach out if you have any problems",
                    Email = adminEmail,
                };
               IdentityResult result = await userManager
                    .CreateAsync(admin, adminPass);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(AdminCreationFailedException);
                }
            }

            bool isAdminInRole = await userManager.IsInRoleAsync(admin, roles[0]);

            if (!isAdminInRole)
            {
               IdentityResult result = await userManager.AddToRoleAsync(admin, roles[0]);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(AdminCreationFailedException);
                }
            }
        }
    }
}
