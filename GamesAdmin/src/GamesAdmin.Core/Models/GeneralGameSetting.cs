using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Models
{
    public class GeneralGameSetting
    {
        public GeneralGameSetting(int gameId, string name, string displayName, GameType type)
        {
            GameId = gameId;
            Name = name;
            DisplayName = displayName;
            Type = type;
        }

        public int GameId { get; }

        public string Name { get; }

        public string DisplayName { get; }

        public GameType Type { get; }
    }
}