namespace GamesAdmin.Database.Report.Entities
{
    using GamesAdmin.Core.Models;
    using GamesAdmin.Core.Models.BetReport;
    using GamesAdmin.Database.Entities;
    using System;
    using System.Collections.Generic;

    public abstract class HoldemPokerBetReportEntity
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

        public abstract decimal Stake { get; }

        public abstract decimal TotalWin { get; }

        public decimal BetWin { get; }

        public bool IsFold { get; }

        public decimal Rate { get; }

        public bool IsQualified { get; }
    }

    public abstract class HoldemPokerBetHistoryModel
    {
        protected const string emptyOdds = "-";

        public HoldemPokerBetHistoryModel(
            HoldemPokerBetEntity bet,
            HoldemPokerRoundEntity round,
            bool convertToUSD)
        {
            Ended = round.Ended;
            IsFold = bet.IsFold;
            RoundId = bet.Round;
            IsQualified = round.SettlementResult?.IsQualified ?? false;

            Player = new HoldemPokerHandModel { Cards = bet.Cards, CombinationCards = bet.Result.Cards, Id = bet.Result.HandRankId };

            Dealer = new HoldemPokerHandModel { Cards = round.Result.Dealer, CombinationCards = round.Ended && round.SettlementResult != null ? round.SettlementResult.Cards : new List<PokerCard>(), Id = round.Ended && round.SettlementResult != null ? round.SettlementResult.HandRankId : 0 };

            CommunityCards = round.Result.CommunityCards;
            Ante = bet.Ante;

            Call = bet.Call;

            BetWin = TotalWin - Stake;

            Time = bet.Time;
            CallTime = bet.CallTime;

            Currency = bet.Currency;
            Rate = bet.Rate;
            ConvertToUSD = convertToUSD;
        }

        public decimal Ante { get; }

        public decimal Call { get; }

        public abstract string AnteOdds { get; }

        public abstract string CallOdds { get; }

        public long RoundId { get; }

        public HoldemPokerHandModel Player { get; }

        public HoldemPokerHandModel Dealer { get; }

        public IEnumerable<PokerCard> CommunityCards { get; }

        public bool Ended { get; }

        public bool IsFold { get; }

        public bool IsQualified { get; }

        public abstract decimal Stake { get; }

        public abstract decimal TotalWin { get; }

        public decimal BetWin { get; }

        public DateTime Time { get; }

        public DateTime CallTime { get; }

        public string Currency { get; }

        public decimal Rate { get; }

        public bool ConvertToUSD { get; }
    }
}
