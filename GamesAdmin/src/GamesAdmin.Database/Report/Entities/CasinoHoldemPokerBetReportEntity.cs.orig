namespace GamesAdmin.Database.Report.Entities
{
    using GamesAdmin.Core.Models.BetReport;
    using GamesAdmin.Database.Entities;
    using System;

    public class CasinoHoldemPokerBetReportEntity : HoldemPokerBetReportEntity
    {
        public CasinoHoldemPokerBetReportEntity(
            DateTime time,
            string partnerId,
            string username,
            string nickname,
            string currency,
            CasinoHoldemPokerBetWithUnWindAccountsEntity bet,
            CasinoHoldemPokerRoundEntity round) : base(time, partnerId, username, nickname, currency, bet, round)
        {
            Stake = bet.Ante + bet.Aa + bet.Call;
            TotalWin = (Ended || bet.IsFold) && bet.SettlementResult != null ? bet.SettlementResult.Aa + bet.SettlementResult.Ante + bet.SettlementResult.Call : 0;
            ExchangeRateId = round.ExchangeRateId;
        }

        public override decimal Stake { get; }

        public override decimal TotalWin { get; }

        public int ExchangeRateId { get; }
    }

    public class CasinoHoldemPokerBetHistoryModel : HoldemPokerBetHistoryModel
    {
        public CasinoHoldemPokerBetHistoryModel(
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

        public override string AnteOdds { get; }

        public override string CallOdds { get; }

        public override decimal Stake { get; }

        public override decimal TotalWin { get; }

        public CasinoHoldemPokerBetSettlementResult SettlementResult { get; }
    }
}
