namespace GamesAdmin.Api.VoidGame.Models
{
    public class GameTicketCollection
    {
        public GameTicketCollection(string one, string bet)
        {
            One = one;
            Bet = bet;
        }

        public string One { get; }

        public string Bet { get; }
    }
}