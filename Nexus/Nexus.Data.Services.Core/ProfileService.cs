using Microsoft.EntityFrameworkCore;
using Nexus.Data.Models;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.ViewModels.Profile;

namespace Nexus.Data.Services.Core
{
    public class ProfileService : IProfileService
    {

        //FIX display of level of the users 

        private readonly NexusDbContext dbContext;
        public ProfileService(NexusDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<ProfileViewModel>> GetAllProfilesByNameThenByAgeThenByCityAscAsync()
        {
            IQueryable<Profile> fetchProfilesQuery = dbContext
                .Users
                .Include(p => p.ProfileInterest)
                    .ThenInclude(i => i.Interest)
                .AsNoTracking();

            IEnumerable<ProfileViewModel> allProfilesVm = await fetchProfilesQuery
                .OrderBy(p => p.DisplayName)
                .ThenBy(p => p.Age)
                .ThenBy(p => p.City)
                .Select(u => new ProfileViewModel
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
                })
                .ToArrayAsync();

            return allProfilesVm;
        }
        public async Task<ProfileEditViewModel> GetEditProfileViewModelWithAllInterestsAsync(string userId)
        {
            Profile? userFetch = await dbContext
                .Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u=> u.Id == userId);

            //Even if there is always going to be user to be found in the Db.
            if (userFetch == null)
            {
                throw new ArgumentException("This profile was not found!");
            }
            
            List<int>interestId = await dbContext
                .ProfileInterests
                .Where(pf => pf.ProfileId == userId)
                .Select(i => i.InterestId)
                .ToListAsync();

            ProfileEditViewModel myProfileViewModel = new ProfileEditViewModel
            {
                DisplayName = userFetch.DisplayName,
                Age = userFetch.Age,
                City = userFetch.City,
                Bio = userFetch.Bio,
                DesiredConnection = userFetch.DesiredConnection,
                AvailableInterests = await GetAllInterestsAsync(),
                InterestId = interestId,
               


            };
            return myProfileViewModel;
        }

        public async Task<List<AvailableInterestViewModel>> GetAllInterestsAsync()
        {
            return await dbContext
               .Interests
               .AsNoTracking()
               .Select(i => new AvailableInterestViewModel()
               {
                   Id = i.Id,
                   Name = i.Name,
               })
               .OrderBy(i => i.Name)
               .ToListAsync();
        }

        public async Task EditProfileAsync(string userId, ProfileEditViewModel profileViewModel)
        {
            Profile? userFetch = await dbContext
                .Users
                .SingleOrDefaultAsync(u => u.Id == userId);

            //Even if there is always going to be user to be found in the Db.
            if (userFetch == null)
            {
                throw new ArgumentException("This profile was not found!");
            }

            userFetch.DisplayName = profileViewModel.DisplayName;
            userFetch.Age = profileViewModel.Age;
            userFetch.City = profileViewModel.City;
            userFetch.Bio = profileViewModel.Bio;
            userFetch.DesiredConnection = profileViewModel.DesiredConnection;

            List<ProfileInterests> oldInterests = dbContext
                .ProfileInterests
                .Where(p => p.ProfileId == userFetch.Id)
                .ToList();

            dbContext
                .ProfileInterests
                .RemoveRange(oldInterests);

            IEnumerable<ProfileInterests> newInterests = profileViewModel
                .InterestId
                .Select(i => new ProfileInterests
                {
                    ProfileId = userFetch.Id,
                    InterestId = i
                });

            dbContext
                .ProfileInterests
                .AddRange(newInterests);

            await dbContext.SaveChangesAsync();
        }

        public async Task<ProfileViewModel> GetCurrentUserProfile(string userId)
        {
            Profile? userFetch = await dbContext
             .Users
                .Include(p => p.ProfileInterest)
                    .ThenInclude(i => i.Interest)
             .AsNoTracking()
             .SingleOrDefaultAsync(u => u.Id == userId);

            if (userFetch == null)
            {
                throw new ArgumentException("Not found.");
            }

            ProfileViewModel profileViewModel = new ProfileViewModel()
            {
                Id = userFetch.Id,
                DisplayName = userFetch.DisplayName, 
                Age = userFetch.Age,
                City = userFetch.City,
                Bio = userFetch.Bio,
                DesiredConnection = userFetch.DesiredConnection,
                ExperiencePoints = userFetch.ExperiencePoints,
                Level = userFetch.Level,
                Interests = userFetch.ProfileInterest
                    .Select(i => i.Interest.Name)
                    .ToList()
            };

            return profileViewModel;
        }
    }
}
