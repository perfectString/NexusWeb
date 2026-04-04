using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Data.Models;
using Nexus.Data.Seeding;
using Nexus.Data.Seeding.Contracts;
using Nexus.Data.Services.Core;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.Web.Infrastructure;

namespace Nexus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<NexusDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Identity
            builder.Services
                .AddIdentity<Profile, IdentityRole<Guid>>(options =>
                {
                    ConfigureIdentity(options, builder.Configuration);
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<NexusDbContext>()
                .AddDefaultTokenProviders();

            // No-op email sender 
            builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();

            // Identity Seed
            builder.Services.AddTransient<IIdentitySeeder, IdentitySeeder>();

            // Profile Service
            builder.Services.AddScoped<IProfileService, ProfileService>();

            // Quest Service
            builder.Services.AddScoped<IQuestService, QuestService>();

            // ProfileManagement Service
            builder.Services.AddScoped<IProfileManagementService, ProfileManagementService>();
            
            // QuestManagement Service
            builder.Services.AddScoped<IQuestManagementService, QuestManagementService>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // Connection String
            string? developerConnectionString = builder.Configuration
                .GetConnectionString("DeveloperConnection");

            builder.Services.AddDbContext<NexusDbContext>(options =>
            {
                options.UseSqlServer(developerConnectionString);
            });
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();



            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            // Authentication & authorization 
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");


            //Seeding user & admins
            app.UseRolesSeeder();
            app.UseAdminSeeder();

            app.MapControllerRoute(
                name: "adminArea",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        // Identity Configuration
        // Dev Settings
        private static void ConfigureIdentity(IdentityOptions options, ConfigurationManager configuration)
        {

            // Sign in Settings
            options.SignIn.RequireConfirmedAccount = configuration
                .GetValue<bool>("IdentityOptions:SignIn:RequireConfirmedAccount");
            options.SignIn.RequireConfirmedPhoneNumber = configuration
                .GetValue<bool>("IdentityOptions:SignIn:RequireConfirmedPhoneNumber");
            options.SignIn.RequireConfirmedEmail = configuration
                .GetValue<bool>("IdentityOptions:SignIn:RequireConfirmedEmail");


            // User Settings
            options.User.RequireUniqueEmail = configuration
                .GetValue<bool>("IdentityOptions:User:RequireUniqueEmail");

            // Password Settings
            options.Password.RequireUppercase = configuration
                .GetValue<bool>("IdentityOptions:Password:RequireUppercase");
            options.Password.RequireLowercase = configuration
                .GetValue<bool>("IdentityOptions:Password:RequireLowercase");
            options.Password.RequireDigit = configuration
                .GetValue<bool>("IdentityOptions:Password:RequireDigit");
            options.Password.RequireNonAlphanumeric = configuration
                .GetValue<bool>("IdentityOptions:Password:RequireNonAlphanumeric");
            options.Password.RequiredLength = configuration
                .GetValue<int>("IdentityOptions:Password:RequiredLength");
        }
    }
}
