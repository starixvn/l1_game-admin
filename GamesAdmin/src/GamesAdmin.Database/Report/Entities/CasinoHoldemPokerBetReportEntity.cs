namespace GamesAdmin.Database.Report.Entities
{
    using GamesAdmin.Core.Models;
    using GamesAdmin.Core.Models.BetReport;
    using GamesAdmin.Database.Entities;
    using System;
    using System.Collections.Generic;

    public class CasinoHoldemPokerBetReportEntity
    {
        public CasinoHoldemPokerBetReportEntity(
            DateTime time,
            string partnerId,
            string username,
            string nickname,
            string currency,
            CasinoHoldemPokerBetWithUnWindAccountsEntity bet,
            CasinoHoldemPokerRoundEntity round)
        {
            Time = time;
            PartnerId = partnerId;
            Username = username;
            Nickname = nickname;
            Currency = currency;

            Ended = round.Ended;
            RoundId = bet.Round;

            Player = new CasinoHoldemPokerHandModel { Cards = bet.Cards, CombinationCards = bet.Result.Cards, Id = bet.Result.HandRankId };

            Dealer = new CasinoHoldemPokerHandModel { Cards = round.Result.Dealer, CombinationCards = round.Ended && round.SettlementResult != null ? round.SettlementResult.Cards : new List<PokerCard>(), Id = round.Ended && round.SettlementResult != null ? round.SettlementResult.HandRankId : 0 };

            Stake = bet.Ante + bet.Aa + bet.Call;
            TotalWin = (Ended || bet.IsFold) && bet.SettlementResult != null ? bet.SettlementResult.AA + bet.SettlementResult.Ante + bet.SettlementResult.Call : 0;
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

        public CasinoHoldemPokerHandModel Player { get; }

        public CasinoHoldemPokerHandModel Dealer { get; }

        public bool Ended { get; }

        public decimal Stake { get; }

        public decimal TotalWin { get; }

        public decimal BetWin { get; }

        public bool IsFold { get; }

        public decimal Rate { get; }

        public bool IsQualified { get; }
    }

    public class CasinoHoldemPokerBetHistoryModel
    {
        private const string emptyOdds = "-";

        public CasinoHoldemPokerBetHistoryModel(
            CasinoHoldemPokerBetEntity bet,
            CasinoHoldemPokerRoundEntity round,
            CasinoHoldemPokerBetHistoryFilter filter)
        {
            Ended = round.Ended;
            IsFold = bet.IsFold;
            RoundId = bet.Round;
            IsQualified = round.SettlementResult?.IsQualified ?? false;

            Player = new CasinoHoldemPokerHandModel { Cards = bet.Cards, CombinationCards = bet.Result.Cards, Id = bet.Result.HandRankId };

            Dealer = new CasinoHoldemPokerHandModel { Cards = round.Result.Dealer, CombinationCards = round.Ended && round.SettlementResult != null ? round.SettlementResult.Cards : new List<PokerCard>(), Id = round.Ended && round.SettlementResult != null ? round.SettlementResult.HandRankId : 0 };

            CommunityCards = round.Result.CommunityCards;
            SettlementResult = bet.SettlementResult;

            AA = bet.Aa;
            AaOdds = Ended && (bet.SettlementResult?.AA > bet.Aa) ? bet.AaOdds.ToString() : emptyOdds;

            Ante = bet.Ante;
            AnteOdds = Ended && (bet.SettlementResult?.Ante > bet.Ante) ? bet.AnteOdds.ToString() : emptyOdds;

            Call = bet.Call;
            CallOdds = Ended && (bet.SettlementResult?.Call > bet.Call) ? bet.CallOdds.ToString() : emptyOdds;

            Stake = bet.Ante + bet.Aa + bet.Call;
            TotalWin = (Ended || bet.IsFold) && bet.SettlementResult != null ? bet.SettlementResult.AA + bet.SettlementResult.Ante + bet.SettlementResult.Call : 0;
            BetWin = TotalWin - Stake;

            Time = TimeZoneInfo.ConvertTimeFromUtc(bet.Time, filter.TimeZoneInfo);
            CallTime = TimeZoneInfo.ConvertTimeFromUtc(bet.CallTime, filter.TimeZoneInfo);

            Currency = bet.Currency;
            Rate = bet.Rate;
            ConvertToUSD = filter.ConvertToUSD;
        }

        public decimal AA { get; }

        public decimal Ante { get; }

        public decimal Call { get; }

        public string AaOdds { get; }

        public string AnteOdds { get; }

        public string CallOdds { get; }

        public long RoundId { get; }

        public CasinoHoldemPokerHandModel Player { get; }

        public CasinoHoldemPokerHandModel Dealer { get; }

        public IEnumerable<PokerCard> CommunityCards { get; }

        public CasinoHoldemPokerBetSettlementResult SettlementResult { get; }

        public bool Ended { get; }

        public bool IsFold { get; }

        public bool IsQualified { get; }

        public decimal Stake { get; }

        public decimal TotalWin { get; }

        public decimal BetWin { get; }

        public DateTime Time { get; }

        public DateTime CallTime { get; }

        public string Currency { get; }

        public decimal Rate { get; }

        public bool ConvertToUSD { get; }
    }

    public class CasinoHoldemPokerBetModel
    {
        public decimal Aa { get; set; }
        public decimal Ante { get; set; }
        public decimal Call { get; set; }
        public IEnumerable<PokerCard> PlayerCards { get; set; }
        public IEnumerable<PokerCard> CombinationCards { get; set; }
        public byte HandRankId { get; set; }
        public CasinoHoldemPokerDetailBetModel SettlementResult { get; set; }
        public decimal BetWin { get; set; }
    }

    public class CasinoHoldemPokerDetailBetModel
    {
        public decimal Aa { get; set; }
        public decimal Ante { get; set; }
        public decimal Call { get; set; }
    }
}
