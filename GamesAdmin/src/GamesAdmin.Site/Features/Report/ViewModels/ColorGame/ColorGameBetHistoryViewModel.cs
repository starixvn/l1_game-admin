using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class ColorGameBetHistoryViewModel : BetHistoryViewModel
    {
        public ColorGameRoundResultViewModel RoundResult { get; set; }
        public IEnumerable<ColorGameBetHistoryRecordViewModel> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }

        public string GetOddName(string choice)
        {
            var isWin = this.RoundResult.SettlementResult[choice.ToLower()];
            var resultMapper = new Dictionary<string, int>() {{ "yellow", 0}, { "red", 1}, { "pink", 2}, { "white", 3}, { "blue", 4}, { "green", 5}};
            var oddMapper = new Dictionary<int, string>(){ { 1, "single"}, { 2, "double"}, { 3, "triple"}};

            if(this.RoundResult.InferableChoices.Contains(choice))
            {
                return choice;
            }
            else if (isWin)
            {
                var winCount = 0;
                if(this.RoundResult.Dice1 == resultMapper[choice])
                {
                    winCount += 1;
                }
                if (this.RoundResult.Dice2 == resultMapper[choice])
                {
                    winCount += 1;
                }
                if (this.RoundResult.Dice3 == resultMapper[choice])
                {
                    winCount += 1;
                }
                return oddMapper[winCount];
            }
            else
            {
                return string.Empty;
            }
        }

        public decimal GetOddsToShow(string choice)
        {
            var winningOddsName = GetOddName(choice);
            if (string.IsNullOrEmpty(winningOddsName))
            {
                return 0;
            }
            else
            {
                return Odds[winningOddsName];
            }
        }

        public decimal GetWinningOdds(string choice)
        {
            var isWin = this.RoundResult.SettlementResult[choice.ToLower()];
            return isWin ? GetOddsToShow(choice) : 0;
        }
    }

    public class ColorGameBetHistoryRecordViewModel : BetHistoryRecordViewModel
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