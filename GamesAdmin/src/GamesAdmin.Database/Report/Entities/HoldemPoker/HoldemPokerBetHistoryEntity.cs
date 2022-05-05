namespace GamesAdmin.Database.Report.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GamesAdmin.Core.Models.BetReport;
    using GamesAdmin.Database.Entities;

    public class HoldemPokerBetHistoryEntity
    {
        protected const string emptyOdds = "-";

        public HoldemPokerBetHistoryEntity(
            HoldemPokerBetEntity bet,
            HoldemPokerRoundEntity round,
            bool convertToUSD)
        {
            Ended = round.Ended;
            IsFold = bet.IsFold;
            RoundId = bet.Round;
            IsSwappedOrHold = round.IsSwappedOrHold;
            IsQualified = round.SettlementResult?.IsQualified;

            var defaultPokerCards = Enumerable.Empty<PokerCard>();

            Player = new HoldemPokerHandModel
            {
                Cards = bet?.Cards ?? defaultPokerCards,
                CombinationCards = bet?.Result?.Cards ?? defaultPokerCards,
                Id = bet?.Result?.HandRankId ?? 0
            };

            Dealer = new HoldemPokerHandModel
            {
                Cards = round?.Result?.Dealer ?? defaultPokerCards,
                CombinationCards = round.Ended && round.SettlementResult != null ? round.SettlementResult.Cards : defaultPokerCards,
                Id = round.Ended && round.SettlementResult != null ? round.SettlementResult.HandRankId : 0
            };

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

        public string AnteOdds { get; }

        public string CallOdds { get; }

        public long RoundId { get; }

        public HoldemPokerHandModel Player { get; }

        public HoldemPokerHandModel Dealer { get; }

        public IEnumerable<PokerCard> CommunityCards { get; }

        public bool Ended { get; }

        public bool IsFold { get; }

        public bool? IsQualified { get; }

        public bool? IsSwappedOrHold { get; set; }

        public decimal Stake { get; }

        public decimal TotalWin { get; }

        public decimal BetWin { get; }

        public DateTime Time { get; }

        public DateTime CallTime { get; }

        public string Currency { get; }

        public decimal Rate { get; }

        public bool ConvertToUSD { get; }
    }
}
