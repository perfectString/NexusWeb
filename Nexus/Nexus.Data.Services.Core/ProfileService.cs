using Microsoft.EntityFrameworkCore;
using Nexus.Data.Models;
using Nexus.Data.Services.Core.Helpers;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.GCommon.Exceptions;
using Nexus.ViewModels.Profile;

namespace Nexus.Data.Services.Core
{
    public class ProfileService : IProfileService
    {
        private readonly NexusDbContext dbContext;
        public ProfileService(NexusDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> GetAllProfilesCountAsync()
        {
            Guid adminRole = await dbContext
                .Roles
                .Where(r => r.Name == "Admin")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            List<Guid> adminIds = await dbContext
                .UserRoles
                .Where(ur => ur.RoleId == adminRole)
                .Select(ur => ur.UserId)
                .ToListAsync();

            return await dbContext
                .Users
                .Where(p => !adminIds.Contains(p.Id))
                .CountAsync();
        }

        public async Task<IEnumerable<ProfileViewModel>> GetAllProfilesByNameThenByAgeThenByCityAscAsync(int page, int pageSize)
        {
            //Excluding the admin from all profiles view
            Guid adminRole = await dbContext
                .Roles
                .Where(r => r.Name == "Admin")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            List<Guid> adminIds = await dbContext
                .UserRoles
                .Where(ur => ur.RoleId == adminRole)
                .Select(ur => ur.UserId)
                .ToListAsync();

            IEnumerable<ProfileViewModel> allProfilesVm = await dbContext
                .Users
                .Include(p => p.ProfileInterest)
                    .ThenInclude(i => i.Interest)
                .AsNoTracking()
                .Where(p => !adminIds.Contains(p.Id))
                .OrderBy(p => p.DisplayName)
                .ThenBy(p => p.Age)
                .ThenBy(p => p.City)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new ProfileViewModel
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    Age = u.Age,
                    City = u.City,
                    Bio = u.Bio,
                    DesiredConnection = u.DesiredConnection,
                    ExperiencePoints = u.ExperiencePoints,
                    Level = LevelHelper.GetLevel(u.ExperiencePoints),
                    XpIntoCurrentLevel = LevelHelper.GetXpIntoCurrentLevel(u.ExperiencePoints),
                    XpNeededPerLevel = LevelHelper.GetXpNeededPerLevel(),
                    XpNeededToNextLevel = LevelHelper.GetXpNeededToNextLevel(u.ExperiencePoints),
                    ProgressPercentage = LevelHelper.GetProgressPercentage(u.ExperiencePoints),
                    Interests = u.ProfileInterest
                    .Select(i => i.Interest.Name)
                    .ToList()
                })
                .ToArrayAsync();

            return allProfilesVm;
        }

        public async Task<ProfileEditViewModel> GetEditProfileViewModelWithAllInterestsAsync(Guid userId)
        {
            Profile? userFetch = await dbContext
                .Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == userId);

            //Even if there is always going to be user to be found in the Db.
            if (userFetch == null)
            {
                throw new EntityNotFoundException();
               
            }

            List<int> interestId = await dbContext
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

        public async Task EditProfileAsync(Guid userId, ProfileEditViewModel profileViewModel)
        {
            Profile? userFetch = await dbContext
                .Users
                .SingleOrDefaultAsync(u => u.Id == userId);

            //Even if there is always going to be user to be found in the Db.
            if (userFetch == null)
            {
                throw new EntityNotFoundException();
            }

            userFetch.DisplayName = profileViewModel.DisplayName;
            userFetch.Age = profileViewModel.Age;
            userFetch.City = profileViewModel.City;
            userFetch.Bio = profileViewModel.Bio;
            userFetch.DesiredConnection = profileViewModel.DesiredConnection;

            List<ProfileInterest> oldInterests = dbContext
                .ProfileInterests
                .Where(p => p.ProfileId == userFetch.Id)
                .ToList();

            dbContext
                .ProfileInterests
                .RemoveRange(oldInterests);

            IEnumerable<ProfileInterest> newInterests = profileViewModel
                .InterestId
                .Select(i => new ProfileInterest
                {
                    ProfileId = userFetch.Id,
                    InterestId = i
                });

            dbContext
                .ProfileInterests
                .AddRange(newInterests);

            await dbContext.SaveChangesAsync();
        }

        public async Task<ProfileViewModel> GetCurrentUserProfile(Guid userId)
        {
            Profile? userFetch = await dbContext
             .Users
                .Include(p => p.ProfileInterest)
                    .ThenInclude(i => i.Interest)
             .AsNoTracking()
             .SingleOrDefaultAsync(u => u.Id == userId);

            if (userFetch == null)
            {
                throw new EntityNotFoundException();
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
                Level = LevelHelper.GetLevel(userFetch.ExperiencePoints),
                XpIntoCurrentLevel = LevelHelper.GetXpIntoCurrentLevel(userFetch.ExperiencePoints),
                XpNeededPerLevel = LevelHelper.GetXpNeededPerLevel(),
                XpNeededToNextLevel = LevelHelper.GetXpNeededToNextLevel(userFetch.ExperiencePoints),
                ProgressPercentage = LevelHelper.GetProgressPercentage(userFetch.ExperiencePoints),
                Interests = userFetch.ProfileInterest
                    .Select(i => i.Interest.Name)
                    .ToList()
            };

            return profileViewModel;
        }
    }
}
