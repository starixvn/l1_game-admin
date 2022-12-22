using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class WinThreeCardsBetHistoryViewModel : BetHistoryViewModel
    {
        public WinThreeCardsRoundResultViewModel RoundResult { get; set; }

        public WinThreeCardsBetHistoryRecordViewModel Bet { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }

        public decimal GetBetWin(string choice, decimal amount)
        {
            return string.IsNullOrEmpty(choice) ? 0 : Odds[choice] * amount;
        }
    }

    public class WinThreeCardsBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public decimal BetWin { get; set; }
        public decimal Dragon { get; set; }
        public decimal Phoenix { get; set; }
        public decimal Pair8Plus { get; set; }
        public decimal Straight { get; set; }
        public decimal Flush { get; set; }
        public decimal StraightFlush { get; set; }
        public decimal ThreeOfAKind { get; set; }

        public decimal GetValueBet(string choice)
        {
            decimal.TryParse(this.GetType().GetProperty(choice).GetValue(this, null).ToString(), out decimal amount);
            return amount;
        }
    }
}