namespace GamesAdmin.Core.Models.Lobby
{
    public class DirectLobbyModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string[] CurrencyGroupIds { get; set; }
        public int[] GameIds { get; set; }
    }
}
