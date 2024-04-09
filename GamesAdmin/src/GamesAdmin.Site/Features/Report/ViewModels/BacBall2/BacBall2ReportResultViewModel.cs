using GamesAdmin.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BacBall2ReportResultViewModel : ReportResultViewModel
    {
        public long DrawId { get; set; }
        public BacBall2Round RoundResult { get; set; }

        public IEnumerable<BacBall2RecordViewModel> Records { get; set; }

        public List<BacBall2RoundResult> Result { get; set; }

        public List<BacBall2SettlementResult> SettlementResult { get; set; }

        public List<int> GreenBalls = new List<int> { 3, 4, 7, 8, 11, 12, 15, 16, 19, 20, 23, 24, 27, 28, 31, 32, 35, 36, 39, 40, 43, 44, 47, 48, 51, 52, 55, 56, 59, 60, 63, 64, 67, 68, 71, 72, 75 };

        public string GetBallColor(int ball)
        {
            return GreenBalls.Contains(ball) ? "green-ball" : "red-ball";            
        }

        public BacBall2RoundResult GetResult()
        {
            return RoundResult.Result.FirstOrDefault(r => r.DrawId == DrawId);
        }

        public BacBall2SettlementResult GetSettlementResult()
        {
            return RoundResult.SettlementResult.FirstOrDefault(r => r.DrawId == DrawId);
        }

        public string GetWinningOption(BacBall2SettlementResult settlementResult)
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

    public class BacBall2RoundResultViewModel : BaseRoundResultViewModel
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

    public class BacBall2RecordViewModel : BetRecordViewModel
    {
        public decimal TotalAmount { get; set; }

        public long DrawId { get; set; }
    }
}