
namespace Nexus.GCommon.Helpers
{
    public static class LevelHelper
    {
        private const int XpPerLevel = 100;

        public static int GetLevel(int totalXp)
        {
            return (totalXp / XpPerLevel) + 1;
        }

        public static int GetXpIntoCurrentLevel(int totalXp)
        {
            return totalXp % XpPerLevel;
        }

        public static int GetXpNeededPerLevel()
        {
            return XpPerLevel;
        }

        public static int GetXpNeededToNextLevel(int totalXp)
        {
            int xpIntoLevel = totalXp % XpPerLevel;
            return XpPerLevel - xpIntoLevel;
        }

        public static int GetProgressPercentage(int totalXp)
        {
            return totalXp % XpPerLevel;
        }
    }
}
