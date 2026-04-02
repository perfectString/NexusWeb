using Nexus.ViewModels.Profile;

namespace Nexus.Data.Services.Core.Interfaces
{
    public interface IProfileService
    {
        // All Profile Count
        Task<int> GetAllProfilesCountAsync();

        // All Profiles 
        Task<IEnumerable<ProfileViewModel>> GetAllProfilesByNameThenByAgeThenByCityAscAsync(int page, int pageSize);

        // Get Interests
        Task<List<AvailableInterestViewModel>> GetAllInterestsAsync();

        // Edit Profiles
        Task<ProfileEditViewModel> GetEditProfileViewModelWithAllInterestsAsync(Guid userId);

        Task EditProfileAsync(Guid userId, ProfileEditViewModel profileViewModel);

        // My Profile
        Task<ProfileViewModel> GetCurrentUserProfile(Guid userId);
    }
}
