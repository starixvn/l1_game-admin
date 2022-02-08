namespace GamesAdmin.Database.Report.Entities
{
    using GamesAdmin.Database.Entities;
    using System;

    public class TexasHoldemPokerBetReportEntity : HoldemPokerBetReportEntity
    {
        public TexasHoldemPokerBetReportEntity(
            DateTime time,
            string partnerId,
            string username,
            string nickname,
            string currency,
            TexasHoldemPokerBetWithUnWindAccountsEntity bet,
            TexasHoldemPokerRoundEntity round) : base(time, partnerId, username, nickname, currency, bet, round)
        {
            Stake = bet.Ante + bet.Call + bet.Turn + bet.River;
            TotalWin = (Ended || bet.IsFold) && bet.SettlementResult != null ? bet.SettlementResult.Turn + bet.SettlementResult.River + bet.SettlementResult.Ante + bet.SettlementResult.Call : 0;
        }

        public override decimal Stake { get; }

        public override decimal TotalWin { get; }
    }

    public class TexasHoldemPokerBetHistoryModel : HoldemPokerBetHistoryModel
    {
        public TexasHoldemPokerBetHistoryModel(
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

        public override string AnteOdds { get; }

        public override string CallOdds { get; }

        public override decimal Stake { get; }

        public override decimal TotalWin { get; }

        private string FormatOdds(decimal odds) => odds.Equals(0) ? emptyOdds : odds.ToString();
    }
}
