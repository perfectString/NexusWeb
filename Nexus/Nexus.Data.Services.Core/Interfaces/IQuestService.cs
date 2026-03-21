using Nexus.ViewModels.Quest;

namespace Nexus.Data.Services.Core.Interfaces
{
    public interface IQuestService
    {
        // All Quests
        Task<IEnumerable<QuestViewModel>> GetAllQuestsOrderByTitleAsync();

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

        Task<IEnumerable<QuestViewModel>> GetAllJoinedQuestsByProfileIdAsync(Guid userId);

        // Join Quests
        Task<bool> IsJoinedAsync(Guid userId, int questId);

        // Mark completed quests and give XP to joined users
        Task MarkQuestCompletedAsync(Guid userId, int questId);
    }
}
