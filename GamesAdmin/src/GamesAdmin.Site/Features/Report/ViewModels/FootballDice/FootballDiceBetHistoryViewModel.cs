using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class FootballDiceBetHistoryViewModel : BetHistoryViewModel
    {
        public FootballDiceRoundResultViewModel RoundResult { get; set; }
        public IEnumerable<FootballDiceBetHistoryRecordViewModel> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }

        public decimal GetOddsToShow(string choice)
        {
            return Odds[choice];
        }

        public decimal GetWinningOdds(string choice)
        {
            if(choice != "draw" && RoundResult.SettlementResult.Draw)
            {
                return 1;
            }

            var isWin = false;

            switch (choice)
            {
                case "home":
                    isWin = RoundResult.SettlementResult.Home;
                    break;

                case "away":
                    isWin = RoundResult.SettlementResult.Away;
                    break;

                case "draw":
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