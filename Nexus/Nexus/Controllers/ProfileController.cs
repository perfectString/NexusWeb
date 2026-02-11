using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Models;
using Nexus.ViewModels.Profile;

namespace Nexus.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        /*  every time someone access this the app will call a ctor to access the dbcontext 
         and save it in the readonly
         ctor injection == makes the code more readable without rewriting the same code i might 
        put this in different base controller class a bit later but for now im gonna use it for fundamental logic*/

        private readonly NexusDbContext dbContext;
        private readonly UserManager<Profile> currentUser;
        public ProfileController(NexusDbContext dbContext, UserManager<Profile> currentUser)
        {
            this.dbContext = dbContext;
            this.currentUser = currentUser;
        }

        [HttpGet]
        public IActionResult All()
        {
            IEnumerable<ProfileAllViewModel> allProfiles = dbContext
                .Users
                .Include(u => u.ProfileInterest)
                .ThenInclude(u=> u.Interest)
                .AsNoTracking()
                .Select(u => new ProfileAllViewModel 
                {
                   Id = u.Id,
                   DisplayName = u.DisplayName,
                    Age = u.Age,
                    City = u.City,
                    Bio = u.Bio,
                   DesiredConnection = u.DesiredConnection,
                    Interests = u.ProfileInterest
                    .Select(i => i.Interest.Name)
                    .ToList()
                } )
                .OrderBy(u => u.DisplayName)
                .ThenBy(u => u.Age)
                .ThenBy(u => u.City)
                .ToList();

            return View(allProfiles);
        }

        [HttpGet]

        public async Task<IActionResult> Edit()
        {
            var user = await currentUser.GetUserAsync(User);

            var interestId = dbContext
                .ProfileInterests
                .Where(pf => pf.ProfileId == user.Id)
                .Select(i => i.InterestId)
                .ToList();

            ProfileEditViewModel myProfile = new ProfileEditViewModel
            {
                DisplayName = user.DisplayName,
                Age = user.Age,
                City = user.City,
                Bio = user.Bio,
                DesiredConnection = user.DesiredConnection,
                AvailableInterests = GetAvailableInterests(),
                InterestId = interestId


            };
            return View(myProfile);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileEditViewModel myProfile)
        {
            if (myProfile.InterestId.Count > 3)
            {
                ModelState.AddModelError(
                    nameof(myProfile.InterestId),
                    "You can select up to 3 interests."
                );
            }
            if (!ModelState.IsValid)
            {
                myProfile.AvailableInterests = GetAvailableInterests();
                return View(myProfile);
            }
            // i could make a validation for checking if the user exists but only registered users can access the profile
            // will test it out if needed im gonna add one
            // i would liketo make all my methods and commands await and async for better work flow
            // but i would need more time to do so, in the rush of making this project i would be adding them
            // only where its really needed
            // for instance im adding it here so i can access the current users

            var user = await currentUser.GetUserAsync(User);

            try
            {
                user.DisplayName = myProfile.DisplayName;
                user.Age = myProfile.Age;
                user.City = myProfile.City;
                user.Bio = myProfile.Bio;
                user.DesiredConnection = myProfile.DesiredConnection;

                var oldInterests = dbContext
                    .ProfileInterests
                    .Where(p => p.ProfileId == user.Id)
                    .ToList();

                dbContext
                    .ProfileInterests
                    .RemoveRange(oldInterests);

                var newInterests = myProfile
                    .InterestId
                    .Select(i => new ProfileInterests
                    {
                        ProfileId = user.Id,
                        InterestId = i
                    });

                dbContext
                    .ProfileInterests
                    .AddRange(newInterests);

                await currentUser.UpdateAsync(user);
                await dbContext.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

                return RedirectToAction(nameof(ProfileController.All));
        }

        private List<AvailableInterestViewModel> GetAvailableInterests()
        {
            return dbContext
                .Interests
                .AsNoTracking()
                .Select(i => new AvailableInterestViewModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                })
                .OrderBy(i => i.Name)
                .ToList();
        }
    }
}
