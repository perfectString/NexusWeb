using Nexus.ViewModels.Profile;

namespace Nexus.Data.Services.Core.Interfaces
{
    public interface IProfileService
    {
        // All Profiles
       Task<IEnumerable<ProfileViewModel>> GetAllProfilesByNameThenByAgeThenByCityAscAsync();

        // Edit Profiles
       Task<ProfileEditViewModel> GetEditProfileViewModelWithAllInterestsAsync(Guid userId);

       Task<List<AvailableInterestViewModel>> GetAllInterestsAsync();

       Task EditProfileAsync(Guid userId, ProfileEditViewModel profileViewModel);

        // My Profile

        Task<ProfileViewModel> GetCurrentUserProfile(Guid userId);
    }
}
