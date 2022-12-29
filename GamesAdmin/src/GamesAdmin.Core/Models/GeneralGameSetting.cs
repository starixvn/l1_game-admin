namespace GamesAdmin.Core.Models
{
    public class GeneralGameSetting
    {
        public GeneralGameSetting(int gameId, string name, string displayName)
        {
            GameId = gameId;
            Name = name;
            DisplayName = displayName;
        }

        public int GameId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }
    }
}