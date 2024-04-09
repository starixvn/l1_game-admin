namespace GamesAdmin.Core.Models.BetReport
{
    public class FootballDiceRoundResult : RoundResult
    {
        public FootballDiceResult Result { get; set; }

        public FootballDiceSettlementResult SettlementResult { get; set; }
    }

    public class FootballDiceResult
    {
        public decimal[] Home { get; set; }
        public decimal[] Away { get; set; }
    }

    public class FootballDiceSettlementResult
    {
        public bool Home { get; set; }
        public bool Away { get; set; }
        public bool Draw { get; set; }
        public bool Draw12 { get; set; }
    }
}