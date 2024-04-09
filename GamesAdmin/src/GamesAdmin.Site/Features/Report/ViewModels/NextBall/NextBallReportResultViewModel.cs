using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class NextBallReportResultViewModel : ReportResultViewModel
    {
        public GameId Game { get; set; }
        public long DrawId { get; set; }
        public NextBallRound RoundResult { get; set; }

        public IEnumerable<NextBallRecordViewModel> Records { get; set; }

        public List<NextBallRoundResult> Result { get; set; }

        public List<NextBallSettlementResult> SettlementResult { get; set; }

        public List<int> GreenBalls = new List<int> { 3, 4, 7, 8, 11, 12, 15, 16, 19, 20, 23, 24, 27, 28, 31, 32, 35, 36, 39, 40, 43, 44, 47, 48, 51, 52, 55, 56, 59, 60, 63, 64, 67, 68, 71, 72, 75 };

        public string GetBallColor(int ball)
        {
            return GreenBalls.Contains(ball) ? "green-ball" : "red-ball";
        }

        public NextBallRoundResult GetResult()
        {
            return RoundResult.Result.FirstOrDefault(r => r.DrawId == DrawId);
        }

        public NextBallSettlementResult GetSettlementResult()
        {
            return RoundResult.SettlementResult.FirstOrDefault(r => r.DrawId == DrawId);
        }

        public string GetWinningOptions()
        {
            var settlementResult = GetSettlementResult();
            var winningOptions = new List<string>
            {
                settlementResult.Over ? "Over" :string.Empty,
                settlementResult.Under ? "Under" :string.Empty,
                settlementResult.Odd ? "Odd" :string.Empty,
                settlementResult.Even ? "Even" :string.Empty,
            };

            return string.Join(", ", winningOptions.Where(s => !string.IsNullOrWhiteSpace(s)));
        }
    }

    public class NextBallRecordViewModel : BetRecordViewModel
    {
        public decimal TotalAmount { get; set; }

        public long DrawId { get; set; }
    }
}