using Nexus.ViewModels.Profile;

namespace Nexus.Data.Services.Core.Interfaces
{
    public interface IProfileService
    {
        // All Profiles
       Task<IEnumerable<ProfileViewModel>> GetAllProfilesByNameThenByAgeThenByCityAscAsync();

        // Edit Profiles
       Task<ProfileEditViewModel> GetEditProfileViewModelWithAllInterestsAsync(string userId);

       Task<List<AvailableInterestViewModel>> GetAllInterestsAsync();

       Task EditProfileAsync(string userId, ProfileEditViewModel profileViewModel);

        // My Profile

        Task<ProfileViewModel> GetCurrentUserProfile(string userId);
    }
}
