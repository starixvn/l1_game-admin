using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class GameInfo
    {
        public GameInfo(
            string name,
            bool enableFreeBet,
            GameType type = GameType.AutoRNG,
            byte randomMode = 1,
            string displayName = "",
            int gameId = 0,
            string[] siteExclusions = null,
            int maxWinPerRound = 0,
            bool applyMultiplier = false)
        {
            Name = name;
            EnableFreeBet = enableFreeBet;
            Type = type;
            SiteExclusions = siteExclusions;
            RandomMode = randomMode;
            GameId = gameId;
            DisplayName = displayName;
            MaxWinPerRound = maxWinPerRound;
            ApplyMultiplier = applyMultiplier;
        }

        public int GameId { get; }

        public string DisplayName { get; }

        public GameType Type { get; }

        public string Name { get; }

        public bool EnableFreeBet { get; }

        public string[] SiteExclusions { get; }

        public byte RandomMode { get; }

        public int MaxWinPerRound { get; set; }

        public bool ApplyMultiplier { get; set; }
    }
}