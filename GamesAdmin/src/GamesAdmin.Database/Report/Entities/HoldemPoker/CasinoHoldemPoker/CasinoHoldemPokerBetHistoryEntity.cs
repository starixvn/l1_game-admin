namespace GamesAdmin.Database.Report.Entities
{
    using GamesAdmin.Database.Entities;

    public class CasinoHoldemPokerBetHistoryEntity : HoldemPokerBetHistoryEntity
    {
        public CasinoHoldemPokerBetHistoryEntity(
            CasinoHoldemPokerBetEntity bet,
            CasinoHoldemPokerRoundEntity round,
            bool convertToUSD) : base(bet, round, convertToUSD)
        {
            SettlementResult = bet.SettlementResult;

            AA = bet.Aa;
            AaOdds = Ended && (bet.SettlementResult?.Aa > bet.Aa) ? bet.AaOdds.ToString() : emptyOdds;

            AnteOdds = Ended && (bet.SettlementResult?.Ante > bet.Ante) ? bet.AnteOdds.ToString() : emptyOdds;
            CallOdds = Ended && (bet.SettlementResult?.Call > bet.Call) ? bet.CallOdds.ToString() : emptyOdds;

            Stake = bet.Ante + bet.Aa + bet.Call;
            TotalWin = (Ended || bet.IsFold) && bet.SettlementResult != null ? bet.SettlementResult.Aa + bet.SettlementResult.Ante + bet.SettlementResult.Call : 0;
        }

        public decimal AA { get; }

        public string AaOdds { get; }

        public new string AnteOdds { get; }

        public new string CallOdds { get; }

        public new decimal Stake { get; }

        public new decimal TotalWin { get; }

        public CasinoHoldemPokerBetSettlementResult SettlementResult { get; }
    }
}
