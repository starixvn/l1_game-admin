using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class GameInfo
    {
        public GameInfo(
            string name,
            string displayName,
            bool enableFreeBet,
            GameType type = GameType.AutoRNG)
        {
            Type = type;
            Name = name;
            DisplayName = displayName;
            EnableFreeBet = enableFreeBet;
        }

        public GameType Type { get; }

        public string Name { get; }

        public string DisplayName { get; }

        public bool EnableFreeBet { get; }
    }
}