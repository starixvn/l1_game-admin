using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BandarQiuQiuBetHistoryViewModel : BetHistoryViewModel
    {
        public BandarQiuQiuRoundResultViewModel RoundResult { get; set; }
        public IEnumerable<BandarQiuQiuBetHistoryRecordViewModel> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }

        public const string Tie = "tie";        
        public readonly Dictionary<int, string> HandRankingMapper = new Dictionary<int, string>() { { 0, "ordinaryCard" }, { 1, "quiQui" }, { 2, "big" }, { 3, "small" }, { 4, "twin" }, { 5, "sixDevil" } };

        public string GetOddName(string choice)
        {
            var isWin = this.RoundResult.SettlementResult[choice.ToLower()];            
            if (isWin == 0)
            {
                return string.Empty;
            }
            else if (isWin == 1) {
                return this.GetHandRanking(choice);
            }
            else
            {
                return Tie;
            }
        }

        private string GetHandRanking(string choice)
        {
            var displayChoice = char.ToUpper(choice[0]) + choice.Substring(1).Trim();
            var handRankingId = this.RoundResult.GetType().GetProperty(displayChoice).GetValue(this.RoundResult, null);
            return this.HandRankingMapper[(int)handRankingId];            
        }

        public decimal GetOddsToShow(string choice)
        {
            var winningOddsName = GetOddName(choice);
            if (string.IsNullOrEmpty(winningOddsName))
            {
                return 0;
            }
            else if(winningOddsName == Tie)
            {
                return 1;
            }
            else
            {
                return Odds[winningOddsName];
            }
        }

        public decimal GetWinningOdds(string choice)
        {
            var isWin = this.RoundResult.SettlementResult[choice.ToLower()];
            return isWin > 0 ? GetOddsToShow(choice) : 0;
        }
    }

    public class BandarQiuQiuBetHistoryRecordViewModel : BetHistoryRecordViewModel
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