using Nexus.ViewModels.Quest;

namespace Nexus.Data.Services.Core.Interfaces
{
    public interface IQuestService
    {
        // All Quests
        Task<IEnumerable<QuestViewModel>> GetAllQuestsOrderByTitleAsync();

        // Add Quests
        Task<QuestAddViewModel> GetEmptyQuestAddModelAsync();

        Task AddQuestsAndJoinInitiatorAsync(string userId, QuestAddViewModel questModel);

        // Edit Quests
        Task<QuestAddViewModel> GetQuestToEditViewModelAsync(string userId, int questId);

        Task EditQuestAsync(string userId, int questId, QuestAddViewModel questViewModel);

        // Delete Quests

        Task<QuestViewModel> GetQuestToDeleteAsync(string userId, int questId);
        Task ConfirmQuestToDeleteAsync(string userId, int questId);


        // Details for Quests

        Task<QuestDetailsViewModel?> GetQuestDetailsWithJoinersViewModelAsync(string userId, int questId);

        // Joined Quests

        Task<IEnumerable<QuestViewModel>> GetAllJoinedQuestsByProfileIdAsync(string userId);

        // Join Quests
        Task<bool> IsJoinedAsync(string userId, int questId);

        // Mark completed quests and give XP to joined users
        Task MarkQuestCompletedAsync(string userId, int questId);
    }
}
