using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus.ViewModels.Quest
{
    public class QuestViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string QuestInitiator { get; set; } = null!;

        
        public string InitiatorId { get; set; } = null!;
    }
}
