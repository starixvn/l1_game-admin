namespace GamesAdmin.Database.Report.Entities
{
    using GamesAdmin.Core.Models;
    using GamesAdmin.Core.Models.BetReport;
    using GamesAdmin.Database.Entities;
    using System;
    using System.Collections.Generic;

    public class TexasHoldemPokerBetReportEntity
    {
        public TexasHoldemPokerBetReportEntity(
            DateTime time,
            string partnerId,
            string username,
            string nickname,
            string currency,
            TexasHoldemPokerBetWithUnWindAccountsEntity bet,
            TexasHoldemPokerRoundEntity round)
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

            Stake = bet.Ante + bet.Call + bet.Turn + bet.River;
            TotalWin = (Ended || bet.IsFold) && bet.SettlementResult != null ? bet.SettlementResult.Turn + bet.SettlementResult.River + bet.SettlementResult.Ante + bet.SettlementResult.Call : 0;
            BetWin = TotalWin - Stake;
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
    }

    public class TexasHoldemPokerBetHistoryModel
    {
        private const string emptyOdds = "-";

        public TexasHoldemPokerBetHistoryModel(
            TexasHoldemPokerBetEntity bet,
            TexasHoldemPokerRoundEntity round)
        {
            Ended = round.Ended;
            IsFold = bet.IsFold;
            RoundId = bet.Round;
            IsQualified = round.SettlementResult?.IsQualified ?? false;


            Player = new CasinoHoldemPokerHandModel { Cards = bet.Cards, CombinationCards = bet.Result.Cards, Id = bet.Result.HandRankId };

            Dealer = new CasinoHoldemPokerHandModel { Cards = round.Result.Dealer, CombinationCards = round.Ended && round.SettlementResult != null ? round.SettlementResult.Cards : new List<PokerCard>(), Id = round.Ended && round.SettlementResult != null ? round.SettlementResult.HandRankId : 0 };

            CommunityCards = round.Result.CommunityCards;
            SettlementResult = bet.SettlementResult;


            Ante = bet.Ante;
            AnteOdds = FormatOdds(bet.AnteOdds);

            Call = bet.Call;
            CallOdds = FormatOdds(bet.CallOdds);

            Turn = bet.Turn;
            TurnOdds = FormatOdds(bet.TurnOdds);

            River = bet.River;
            RiverOdds = FormatOdds(bet.RiverOdds);

            Stake = bet.Ante + bet.Turn + bet.Call + bet.River;
            TotalWin = (Ended || bet.IsFold) && bet.SettlementResult != null ? bet.SettlementResult.Turn + bet.SettlementResult.River + bet.SettlementResult.Ante + bet.SettlementResult.Call : 0;
            BetWin = TotalWin - Stake;

            Time = bet.Time;
            CallTime = bet.CallTime;
            TurnTime = bet.TurnTime;
            RiverTime = bet.RiverTime;
        }

        public decimal Ante { get; }

        public decimal Call { get; }

        public decimal Turn { get; }

        public decimal River { get; }

        public string AnteOdds { get; }

        public string CallOdds { get; }

        public string TurnOdds { get; }

        public string RiverOdds { get; }

        public long RoundId { get; }

        public CasinoHoldemPokerHandModel Player { get; }

        public CasinoHoldemPokerHandModel Dealer { get; }

        public IEnumerable<PokerCard> CommunityCards { get; }

        public TexasHoldemPokerBetSettlementResult SettlementResult { get; }

        public bool Ended { get; }

        public bool IsFold { get; }

        public bool IsQualified { get; }

        public decimal Stake { get; }

        public decimal TotalWin { get; }

        public decimal BetWin { get; }

        public DateTime Time { get; }

        public DateTime CallTime { get; }

        public DateTime TurnTime { get; }

        public DateTime RiverTime { get; }

        private string FormatOdds(decimal odds) => odds.Equals(0) ? emptyOdds : odds.ToString();
    }


    public class TexasHoldemPokerBetModel
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

    public class TexasHoldemPokerDetailBetModel
    {
        public decimal Aa { get; set; }
        public decimal Ante { get; set; }
        public decimal Call { get; set; }
    }
}
