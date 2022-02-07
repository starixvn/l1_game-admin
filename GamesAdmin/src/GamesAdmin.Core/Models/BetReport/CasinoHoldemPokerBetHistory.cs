namespace GamesAdmin.Core.Models.BetReport
{
    public class CasinoHoldemPokerBetHistory : HoldemPokerBetHistory
    {
        public CasinoHoldemPokerBetSettlementResult SettlementResult { get; set; }

        public string AaOdds { get; set; }

        public decimal AA { get; set; }
    }

    public class CasinoHoldemPokerBetSettlementResult
    {
        public decimal Aa { get; set; }

        public decimal Ante { get; set; }

        public decimal Call { get; set; }
    }
}
