using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Data.Seeding.Contracts;

namespace Nexus.Web.Infrastructure
{
    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder UseRolesSeeder(this IApplicationBuilder app)
        {

            using IServiceScope scope = app
                 .ApplicationServices
                 .CreateScope();


            IIdentitySeeder identitySeeder = scope
                .ServiceProvider
                  .GetRequiredService<IIdentitySeeder>();

            identitySeeder
                .SeedRolesAsync()
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder UseAdminSeeder(this IApplicationBuilder app)
        {
            using IServiceScope scope = app
             .ApplicationServices
              .CreateScope();


            IIdentitySeeder identitySeeder = scope
                .ServiceProvider
                  .GetRequiredService<IIdentitySeeder>();


            identitySeeder
                .SeedAdminAsync()
                .GetAwaiter()
                .GetResult();

            return app;
        }
    }
}
