using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class SlotRNGBetHistoryViewModel : BetHistoryViewModel
    {
        public SlotRNGRoundResultViewModel RoundResult { get; set; }
        public IEnumerable<SlotRNGBetHistoryRecordViewModel> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }
        public string GameName { get; set; }

        public decimal GetOdds(IEnumerable<string> subChoices, string mainChoice)
        {
            var choice = GetOddsSubChoice(subChoices);
            return string.IsNullOrEmpty(choice) ? Odds[mainChoice] : Odds[choice];
        }

        public decimal GetBetWin(IEnumerable<string> subChoices, string mainChoice, decimal amount)
        {
            var choice = GetOddsSubChoice(subChoices);
            if (RoundResult.SettlementResult[mainChoice])
            {
                choice = mainChoice;
            }
            return string.IsNullOrEmpty(choice) ? 0 : Odds[choice] * amount;
        }

        private string GetOddsSubChoice(IEnumerable<string> subChoices)
        {
            var choice = string.Empty;
            foreach (var c in subChoices)
            {
                if (RoundResult.SettlementResult[c])
                {
                    choice = c;
                }
            }
            return choice;
        }
    }

    public class SlotRNGBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public string Choice { get; set; }
        public IEnumerable<string> SubChoices { get; set; }
        public decimal Amount { get; set; }
    }
}