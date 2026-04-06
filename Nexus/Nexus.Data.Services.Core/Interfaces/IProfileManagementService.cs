using Nexus.ViewModels.Admin.Profile;

namespace Nexus.Data.Services.Core.Interfaces
{
    public interface IProfileManagementService
    {
        /* Admin options */

        // Profile Count
        Task<int> GetAllProfilesCountAsync();

        // All profiles (including admin)
        Task<IEnumerable<ProfileManagementViewModel>> GetAllProfilesAsAdminAsync(int page, int pageSize);

        //Edit profile 
        Task<ProfileManagementViewModel> GetProfileToEditAsAdminAsync(Guid userId);

        Task EditProfileAsAdminAsync(Guid userId, ProfileManagementViewModel model);

        // Delete Profile 
        Task DeleteProfileAsAdminAsync(Guid userId);
    }
}
