using GamesAdmin.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class NextBallBetHistoryViewModel : BetHistoryViewModel
    {
        public NextBallRound RoundResult { get; set; }
        public IEnumerable<NextBallBetHistoryRecordViewModel> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }

        public decimal GetWinAmount(NextBallBetHistoryRecordViewModel bet)
        {
            var settlementResult = RoundResult.SettlementResult.First(s => s.DrawId == bet.DrawId);
            var isWin = bool.Parse(settlementResult.GetType().GetProperty(bet.GetDisplayChoice()).GetValue(settlementResult, null).ToString());
            decimal amount = isWin ? Odds[bet.Choice] * bet.Amount : 0;
            return ConvertToUSD ? bet.ConvertToBase(amount) / BaseCurrencyMultiplier : amount;
        }
    }

    public class NextBallBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public long DrawId { get; set; }

        public string Choice { get; set; }
        public decimal Amount { get; set; }

        public string GetDisplayChoice()
        {
            var result = string.Concat(Choice.Select((x, i) => i > 0 && char.IsUpper(x) ? $" {x}" : x.ToString()));
            return char.ToUpper(result[0]) + result.Substring(1).Trim();
        }
    }
}