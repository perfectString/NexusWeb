using Nexus.ViewModels.Profile;
using Nexus.ViewModels.Quest;

namespace Nexus.Data.Services.Core.Interfaces
{
    public interface IQuestService
    {
        // All Quests 
        Task<int> GetAllQuestsCountAsync();

        Task<IEnumerable<QuestViewModel>> GetAllQuestsOrderByTitleAsync(int page, int pageSize);

        // Add Quests
        Task<QuestAddViewModel> GetEmptyQuestAddModelAsync();

        Task AddQuestsAndJoinInitiatorAsync(Guid userId, QuestAddViewModel questModel);

        // Edit Quests
        Task<QuestAddViewModel> GetQuestToEditViewModelAsync(Guid userId, int questId);

        Task EditQuestAsync(Guid userId, int questId, QuestAddViewModel questViewModel);

        // Delete Quests
        Task<QuestViewModel> GetQuestToDeleteAsync(Guid userId, int questId);
        Task ConfirmQuestToDeleteAsync(Guid userId, int questId);

        // Details for Quests
        Task<QuestDetailsViewModel?> GetQuestDetailsWithJoinersViewModelAsync(Guid userId, int questId);

        // Joined Quests 
        Task<int> GetJoinedQuestsCountAsync(Guid userId);

        Task<IEnumerable<QuestViewModel>> GetAllJoinedQuestsByProfileIdAsync(Guid userId, int page, int pageSize);

        // Created Quests count
        Task<int> GetCreatedQuestsCountAsync(Guid userId);

        // Join and Check Quests
        Task<bool> IsJoinedAsync(Guid userId, int questId);
        Task JoinQuestAsync(Guid userId, int questId);

        // Mark completed quests and give XP to joined users
        Task MarkQuestCompletedAsync(Guid userId, int questId);

        // Get Interests
        Task<List<AvailableInterestViewModel>> GetAllInterestsAsync();
    }
}
