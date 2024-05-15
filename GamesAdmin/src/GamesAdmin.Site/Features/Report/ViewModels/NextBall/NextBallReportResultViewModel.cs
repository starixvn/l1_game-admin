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

        public string GetBallColor(int ball)
        {
            return ball % 2 == 0 ? "red-ball" : "blue-ball";
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