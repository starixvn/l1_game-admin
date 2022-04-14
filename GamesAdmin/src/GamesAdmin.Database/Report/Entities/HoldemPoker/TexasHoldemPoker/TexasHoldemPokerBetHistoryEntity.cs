namespace GamesAdmin.Database.Report.Entities
{
    using System;
    using GamesAdmin.Database.Entities;

    public class TexasHoldemPokerBetHistoryEntity : HoldemPokerBetHistoryEntity
    {
        public TexasHoldemPokerBetHistoryEntity(
            TexasHoldemPokerBetEntity bet,
            TexasHoldemPokerRoundEntity round,
            bool convertToUSD) : base(bet, round, convertToUSD)
        {
            SettlementResult = bet.SettlementResult;

            AnteOdds = FormatOdds(bet.AnteOdds);
            CallOdds = FormatOdds(bet.CallOdds);

            Turn = bet.Turn;
            TurnOdds = FormatOdds(bet.TurnOdds);

            River = bet.River;
            RiverOdds = FormatOdds(bet.RiverOdds);

            Stake = bet.Ante + bet.Turn + bet.Call + bet.River;
            TotalWin = (Ended || bet.IsFold) && bet.SettlementResult != null ? bet.SettlementResult.Turn + bet.SettlementResult.River + bet.SettlementResult.Ante + bet.SettlementResult.Call : 0;

            TurnTime = bet.TurnTime;
            RiverTime = bet.RiverTime;
        }

        public decimal Turn { get; }

        public decimal River { get; }

        public string TurnOdds { get; }

        public string RiverOdds { get; }

        public TexasHoldemPokerBetSettlementResult SettlementResult { get; }

        public DateTime TurnTime { get; }

        public DateTime RiverTime { get; }

        public new string AnteOdds { get; }

        public new string CallOdds { get; }

        public new decimal Stake { get; }

        public new decimal TotalWin { get; }

        private string FormatOdds(decimal odds) => odds.Equals(0) ? emptyOdds : odds.ToString();
    }
}
