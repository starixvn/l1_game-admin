using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class GameInfo
    {
        public GameInfo(
            string name,
            bool enableFreeBet,
            GameType type = GameType.AutoRNG,
            string[] siteExclusions = null)
        {
            Name = name;
            EnableFreeBet = enableFreeBet;
            Type = type;
            SiteExclusions = siteExclusions;
        }

        public GameType Type { get; }

        public string Name { get; }

        public bool EnableFreeBet { get; }

        public string[] SiteExclusions { get; }
    }
}