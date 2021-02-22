using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class GameInfo
    {
        public GameInfo(
            string name,
            bool enableFreeBet,
            GameType type = GameType.AutoRNG)
        {
            Name = name;
            EnableFreeBet = enableFreeBet;
            Type = type;            
        }

        public GameType Type { get; }

        public string Name { get; }

        public bool EnableFreeBet { get; }
    }
}