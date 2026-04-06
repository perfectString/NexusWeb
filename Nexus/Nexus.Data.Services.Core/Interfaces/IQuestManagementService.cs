using Nexus.ViewModels.Admin.Quest;
using Nexus.ViewModels.Profile;

namespace Nexus.Data.Services.Core.Interfaces
{
    public interface IQuestManagementService
    {
        /* Admin options */

        // All Quests Count
        Task<int> GetAllQuestsCountAsync();

        // All Quests 
        Task<IEnumerable<QuestManagementViewModel>> GetAllQuestsAsAdminAsync(int page, int pageSize);

        // Get Interests
        Task<List<AvailableInterestViewModel>> GetAllInterestsAsync();

        //Edit Quest 
        Task<QuestManagementViewModel> GetQuestToEditAsAdminAsync(int questId);

        Task EditQuestAsAdminAsync(int questId, QuestManagementViewModel model);

        // Delete Quest 
        Task DeleteQuestAsAdminAsync(int questId);
    }
}
