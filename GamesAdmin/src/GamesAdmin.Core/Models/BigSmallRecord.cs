namespace GamesAdmin.Core.Models
{
    public class BigSmallRecord
    {
        public string Username { get; set; }

        public string Nickname { get; set; }

        public bool SelectBig { get; set; }

        public decimal Bet { get; set; }

        public decimal BetWin { get; set; }

        public decimal Back { get; set; }
    }
}