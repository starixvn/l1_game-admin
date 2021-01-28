namespace GamesAdmin.Api.VoidGame.Models
{
    public class GameTicketCollection
    {
        public GameTicketCollection(string round, string one, string bet)
        {
            Round = round;
            One = one;
            Bet = bet;
        }

        public string Round { get; }

        public string One { get; }

        public string Bet { get; }
    }
}