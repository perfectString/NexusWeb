using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.ViewModels.Profile;
using Nexus.ViewModels.Quest;

namespace Nexus.Data.Services.Core.Interfaces
{
    public interface IQuestService
    {
        // All Quests
        Task<IEnumerable<QuestViewModel>> GetQuestsOrderByTitleAsync();

        // Add Quests
        Task<QuestAddViewModel> GetEmptyQuestAddModelAsync();
    }
}
