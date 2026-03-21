using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Data.Seeding.Contracts;

namespace Nexus.Web.Infrastructure
{
    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder UseRolesSeeder(this IApplicationBuilder app)
        {
            IIdentitySeeder identitySeeder = app
                  .ApplicationServices
                  .GetRequiredService<IIdentitySeeder>();

            identitySeeder
                .SeedRolesAsync()
                .GetAwaiter()
                .GetResult();

            return app;
        }
    }
}
