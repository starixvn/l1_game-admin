using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class ChoiceHistory
    {
        public ChoiceHistory(string choice, string odd, DateTime time, decimal stake, decimal win)
        {
            Choice = choice;
            Odd = odd;
            Time = time;
            Stake = stake;
            Win = win;
        }

        public string Choice { get; }

        public string Odd { get; }

        public DateTime Time { get; }

        public decimal Stake { get; }

        public decimal Win { get; }
    }

    public class HoldemPokerBetHistory
    {
        public IEnumerable<ChoiceHistory> ChoiceHistories { get; set; }

        public bool ConvertToUSD { get; set; }

        public long RoundId { get; set; }

        public HoldemPokerHandModel Player { get; set; }

        public HoldemPokerHandModel Dealer { get; set; }

        public IEnumerable<PokerCard> CommunityCards { get; set; }

        public bool Ended { get; set; }

        public bool IsFold { get; set; }

        public bool? IsQualified { get; set; }

        public bool? IsSwappedOrHold { get; set; }

        public decimal Stake { get; set; }

        public decimal TotalWin { get; set; }

        public decimal BetWin { get; set; }

        public string Currency { get; set; }

        public decimal Rate { get; set; }

        public decimal ConvertToBase(decimal value) => value * Rate;
    }

    public class HoldemPokerHandModel
    {
        private readonly string[] RankHandName = {
                        "Does Not Qualify",
                        "High Card",
                        "Pair",
                        "Two Pair",
                        "Three Of A Kind",
                        "Straight",
                        "Flush",
                        "Full House",
                        "Four Of A Kind",
                        "Straight Flush",
                        "Royal Flush",
                    };

        public int Id { get; set; }

        public IEnumerable<PokerCard> Cards { get; set; }

        public IEnumerable<PokerCard> CombinationCards { get; set; }

        public string Rank => RankHandName[Id];
    }

    public class PokerCard
    {
        public byte rank { get; set; }

        public byte suit { get; set; }
    }
}
