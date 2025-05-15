using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.Lobby
{
    [BsonCollection("direct_lobbies")]
    public class DirectLobbyEntity : MiniGameBaseEntity
    {
        public string Name { get; set; }
        public string[] CurrencyGroupIds { get; set; }
        public int[] GameIds { get; set; }
        public int[] NewGames { get; set; }
    }
}
