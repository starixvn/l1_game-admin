namespace GamesAdmin.Site.Features.GameRoundResult.Models
{
    public class GameRoundResultAccessUrlRequest
    {
        public Auth Auth { get; set; }

        public string Language { get; set; }
    }

    public class Auth
    {
        public string ClientId { get; set; }

        public string ClientKey { get; set; }
    }
}