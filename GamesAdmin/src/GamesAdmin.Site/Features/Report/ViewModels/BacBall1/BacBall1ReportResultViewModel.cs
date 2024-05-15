using GamesAdmin.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BacBall1ReportResultViewModel : ReportResultViewModel
    {
        public long DrawId { get; set; }
        public BacBall1Round RoundResult { get; set; }

        public IEnumerable<BacBall1RecordViewModel> Records { get; set; }

        public List<BacBall1RoundResult> Result { get; set; }

        public List<BacBall1SettlementResult> SettlementResult { get; set; }

        public string GetBallColor(int ball)
        {
            return ball % 2 == 0 ? "red-ball" : "blue-ball";
        }

        public BacBall1RoundResult GetResult()
        {
            return RoundResult.Result.FirstOrDefault(r => r.DrawId == DrawId);
        }

        public BacBall1SettlementResult GetSettlementResult()
        {
            return RoundResult.SettlementResult.FirstOrDefault(r => r.DrawId == DrawId);
        }

        public string GetWinningOption(BacBall1SettlementResult settlementResult)
        {
            var option = "";
            if (settlementResult.Player)
            {
                option = "Player";
            }
            else if (settlementResult.Banker)
            {
                option = "Banker";
            }
            else
            {
                option = "Tie";
            }

            return option;
        }
    }

    public class BacBall1RoundResultViewModel : BaseRoundResultViewModel
    {
        public Dictionary<string, int> SettlementResult { get; set; }

        public Dictionary<int, string> ResultMapper = new Dictionary<int, string>() { { 0, "Ordinary Card" }, { 1, "Qiu Qiu" }, { 2, "Big" }, { 3, "Small" }, { 4, "Twin" }, { 5, "Six Devil" } };

        public string GetWinningOptions()
        {
            var result = SettlementResult.Where(s => s.Value > 0)
                .Select(s => FormatString(s.Key));

            return string.Join(", ", result);
        }

        public static string FormatString(string input)
        {
            var result = string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? $" {x}" : x.ToString()));
            return char.ToUpper(result[0]) + result.Substring(1).Trim();
        }
    }

    public class BacBall1RecordViewModel : BetRecordViewModel
    {
        public decimal TotalAmount { get; set; }

        public long DrawId { get; set; }
    }
}