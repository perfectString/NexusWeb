namespace Nexus.ViewModels.Leaderboard
{
    public class LeaderboardViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public int Level { get; set; }
        public int ExperiencePoints { get; set; }
        public int CompletedQuests { get; set; }
    }
}
