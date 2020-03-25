namespace GamesAdmin.Core.Models
{
    public class BetInfo
    {
        public BetInfo(decimal bet, int round, bool select, string uid, bool sicbo)
        {
            Bet = bet;
            Round = round;
            Select = select;
            UID = uid;
            Sicbo = sicbo;
        }

        public decimal Bet { get; set; }

        public int Round { get; set; }

        public bool Select { get; set; }

        public string UID { get; set; }

        public bool Sicbo { get; set; }
    }
}
