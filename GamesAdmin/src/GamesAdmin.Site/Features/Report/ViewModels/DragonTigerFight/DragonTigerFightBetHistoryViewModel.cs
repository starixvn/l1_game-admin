using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class DragonTigerFightBetHistoryViewModel : BetHistoryViewModel
    {
        public DragonTigerFightRoundResultViewModel RoundResult { get; set; }
        public IEnumerable<DragonTigerFightBetHistoryRecordViewModel> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }
    }

    public class DragonTigerFightBetHistoryRecordViewModel : BetHistoryRecordViewModel
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