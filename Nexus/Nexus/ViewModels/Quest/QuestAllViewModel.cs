using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexus.ViewModels.Quest
{
    public class QuestAllViewModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string QuestInitiator { get; set; } = null!;


    }
}
