using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class BigSmallReport
    {
        public RoundResult RoundResult {get;set;}
        public IEnumerable<BigSmallRecord> Records { get; set; }
    }

    public class RoundResult
    {
        public long Number { get; set; }

        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }
    }


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