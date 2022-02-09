namespace GamesAdmin.Database.Report.Entities
{
    using GamesAdmin.Core.Models.BetReport;
    using GamesAdmin.Database.Entities;
    using System;
    using System.Collections.Generic;

    public class HoldemPokerBetReportEntity
    {
        public HoldemPokerBetReportEntity(
            DateTime time,
            string partnerId,
            string username,
            string nickname,
            string currency,
            HoldemPokerBetEntity bet,
            HoldemPokerRoundEntity round)
        {
            Time = time;
            PartnerId = partnerId;
            Username = username;
            Nickname = nickname;
            Currency = currency;

            Ended = round.Ended;
            RoundId = bet.Round;

            Player = new HoldemPokerHandModel { Cards = bet.Cards, CombinationCards = bet.Result.Cards, Id = bet.Result.HandRankId };
            Dealer = new HoldemPokerHandModel { Cards = round.Result.Dealer, CombinationCards = round.Ended && round.SettlementResult != null ? round.SettlementResult.Cards : new List<PokerCard>(), Id = round.Ended && round.SettlementResult != null ? round.SettlementResult.HandRankId : 0 };
            BetWin = TotalWin - Stake;

            IsFold = bet.IsFold;
            Rate = bet.Rate;
            IsQualified = round.SettlementResult?.IsQualified ?? false;
        }

        public DateTime Time { get; }

        public string PartnerId { get; }

        public string Username { get; }

        public string Nickname { get; }

        public string Currency { get; set; }

        public long RoundId { get; }

        public HoldemPokerHandModel Player { get; }

        public HoldemPokerHandModel Dealer { get; }

        public bool Ended { get; }

        public decimal Stake { get; }

        public decimal TotalWin { get; }

        public decimal BetWin { get; }

        public bool IsFold { get; }

        public decimal Rate { get; }

        public bool IsQualified { get; }
    }
}
