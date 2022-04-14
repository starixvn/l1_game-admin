namespace GamesAdmin.Database.Report.Entities
{
    using System;
    using GamesAdmin.Database.Entities;

    public class TexasHoldemPokerBetReportEntity : HoldemPokerBetReportEntity
    {
        public TexasHoldemPokerBetReportEntity(
            DateTime time,
            string partnerId,
            string username,
            string nickname,
            string currency,
            TexasHoldemPokerBetEntity bet,
            TexasHoldemPokerRoundEntity round) : base(time, partnerId, username, nickname, currency, bet, round)
        {
            Stake = bet.Ante + bet.Call + bet.Turn + bet.River;
            TotalWin = (Ended || bet.IsFold) && bet.SettlementResult != null ? bet.SettlementResult.Turn + bet.SettlementResult.River + bet.SettlementResult.Ante + bet.SettlementResult.Call : 0;
        }

        public new decimal Stake { get; }

        public new decimal TotalWin { get; }
    }
}
