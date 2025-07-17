namespace GamesAdmin.Core.Models
{
    public class CurrencyGroupGameConfig
    {
        public CurrencyGroupGameConfig(string groupId, int gameId, string type, object value)
        {
            GroupId = groupId;
            GameId = gameId;
            Type = type;
            Value = value;
        }

        public string GroupId { get; }

        public int GameId { get; set; }

        public string Type { get; set; }

        public object Value { get; set; }
    }
}