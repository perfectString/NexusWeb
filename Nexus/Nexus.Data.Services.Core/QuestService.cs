using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nexus.Data.Models;
using Nexus.Data.Services.Core.Interfaces;
using Nexus.ViewModels.Profile;
using Nexus.ViewModels.Quest;

namespace Nexus.Data.Services.Core
{
    public class QuestService : IQuestService
    {

        private readonly NexusDbContext dbContext;
        public QuestService(NexusDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<QuestViewModel>> GetQuestsOrderByTitleAsync()
        {
            IQueryable<Quest> fetchQuestsQuery = dbContext
              .Quests
              .AsNoTracking();

            IEnumerable<QuestViewModel> allQuestsVm = await fetchQuestsQuery
             .OrderBy(q=> q.Title)
             .Select(q => new QuestViewModel()
             {
                 Id = q.Id,
                 Title = q.Title,
                 Description = q.Description,
                 QuestInitiator = q.QuestInitiator.DisplayName,
                 InitiatorId = q.QuestInitiatorId
             })
                .ToArrayAsync();

            return allQuestsVm;
        }

        public async Task<QuestAddViewModel> GetEmptyQuestAddModelAsync()
        {
            QuestAddViewModel emptyFormModel = new QuestAddViewModel();

            //to do
            return emptyFormModel;
        }
    }
}
