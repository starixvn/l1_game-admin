using GamesAdmin.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BacBall1BetHistoryViewModel : BetHistoryViewModel
    {
        public BacBall1Round RoundResult { get; set; }
        public IEnumerable<BacBall1BetHistoryRecordViewModel> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }

        public bool IsWinning(long drawId,  string displayChoice)
        {
            var settlementResult = RoundResult.SettlementResult.First(s => s.DrawId == drawId);
            return bool.Parse(settlementResult.GetType().GetProperty(displayChoice).GetValue(settlementResult, null).ToString());
        }

        public decimal CalculateWinAmount(BacBall1BetHistoryRecordViewModel bet)
        {
            decimal amount = 0;
            if (IsWinning(bet.DrawId, "Tie") && (bet.GetDisplayChoice() == "Player" || bet.GetDisplayChoice() == "Banker")) {
                amount = bet.Amount;
            }
            else
            {
                amount = IsWinning(bet.DrawId, bet.GetDisplayChoice()) ? Odds[bet.Choice] * bet.Amount : 0;
            }

            return ConvertToUSD ? bet.ConvertToBase(amount) / BaseCurrencyMultiplier : amount;
        }

        public string CalculateDrawIDOrder(BacBall1BetHistoryRecordViewModel bet)
        {
            var order = 0;
            foreach(var result in RoundResult.Result.Select((value, i) => new { i, value })) 
            {
                if(result.value.DrawId == bet.DrawId)
                {
                    order = result.i;
                }
                            
            }

            var orderText = order switch
            {
                0 => "1st",
                1 => "2nd",
                2 => "3rd",
                3 => "4th",
                _ => ""
            };

            return orderText;
        }
    }

    public class BacBall1BetHistoryRecordViewModel : BetHistoryRecordViewModel
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