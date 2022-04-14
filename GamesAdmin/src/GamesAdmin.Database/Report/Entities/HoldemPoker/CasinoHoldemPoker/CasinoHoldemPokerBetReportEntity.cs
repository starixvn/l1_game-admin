namespace GamesAdmin.Database.Report.Entities
{
    using System;
    using GamesAdmin.Database.Entities;

    public class CasinoHoldemPokerBetReportEntity : HoldemPokerBetReportEntity
    {
        public CasinoHoldemPokerBetReportEntity(
            DateTime time,
            string partnerId,
            string username,
            string nickname,
            string currency,
            CasinoHoldemPokerBetEntity bet,
            CasinoHoldemPokerRoundEntity round) : base(time, partnerId, username, nickname, currency, bet, round)
        {
            Stake = bet.Ante + bet.Aa + bet.Call;
            TotalWin = (Ended || bet.IsFold) && bet.SettlementResult != null ? bet.SettlementResult.Aa + bet.SettlementResult.Ante + bet.SettlementResult.Call : 0;
        }

        public new decimal Stake { get; }

        public new decimal TotalWin { get; }
    }
}
