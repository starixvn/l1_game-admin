using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Enumeration.BetOdds;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class FootballDiceBetHistoryViewModel : BetHistoryViewModel
    {
        public FootballDiceRoundResultViewModel RoundResult { get; set; }
        public IEnumerable<FootballDiceBetHistoryRecordViewModel> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }

        public decimal GetOddsToShow(string choice)
        {
            if(choice == FootballDiceBetChoice.DrawValue && RoundResult.SettlementResult.Draw12)
            {
                return Odds[FootballDiceBetOdds.Draw12Value];
            }

            return Odds[choice];
        }

        public decimal GetWinningOdds(string choice)
        {
            if(choice != FootballDiceBetChoice.DrawValue && RoundResult.SettlementResult.Draw)
            {
                return 1;
            }

            var isWin = false;

            switch (choice)
            {
                case FootballDiceBetChoice.HomeValue:
                    isWin = RoundResult.SettlementResult.Home;
                    break;

                case FootballDiceBetChoice.AwayValue:
                    isWin = RoundResult.SettlementResult.Away;
                    break;

                case FootballDiceBetChoice.DrawValue:
                    isWin = RoundResult.SettlementResult.Draw;
                    break;
            }

            return isWin ? GetOddsToShow(choice) : 0;
        }
    }

    public class FootballDiceBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public string Choice { get; set; }
        public decimal Amount { get; set; }

        public string GetDisplayChoice()
        {
            var result = string.Concat(Choice.Select((x, i) => i > 0 && char.IsUpper(x) ? $" {x}" : x.ToString()));
            return char.ToUpper(result[0]) + result.Substring(1).Trim();
        }
    }
}