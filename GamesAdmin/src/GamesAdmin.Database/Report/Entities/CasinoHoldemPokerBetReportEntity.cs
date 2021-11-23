namespace GamesAdmin.Database.Report.Entities
{
    using GamesAdmin.Database.Entities;
    using System;

    public class CasinoHoldemPokerBetReportEntity
    {
        private readonly string[] RankHandName = {
                        "DoesNotQualify",
                        "HighCard",
                        "Pair",
                        "TwoPairs",
                        "ThreeOfAKind",
                        "Straight",
                        "Flush",
                        "FullHouse",
                        "FourOfAKind",
                        "StraightFlush",
                        "RoyalFlush",
                    };

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

            PlayerResult = RankHandName[bet.Result.HandRankId];
            DealerResult = RankHandName[Ended && round.SettlementResult != null ? round.SettlementResult.HandRankId : 0];

            Stake = bet.Ante + bet.AA + bet.Call;
            TotalWin = (Ended || bet.IsFold) && bet.SettlementResult != null ? bet.SettlementResult.AA + bet.SettlementResult.Ante + bet.SettlementResult.Call : 0;
            BetWin = TotalWin - Stake;
        }

        public DateTime Time { get; }

        public string PartnerId { get; }

        public string Username { get; }

        public string Nickname { get; }

        public string Currency { get; set; }

        public long RoundId { get; }

        public string PlayerResult { get; }

        public string DealerResult { get; }

        public bool Ended { get; }

        public decimal Stake { get; }

        public decimal TotalWin { get; }

        public decimal BetWin { get; }
    }
}
