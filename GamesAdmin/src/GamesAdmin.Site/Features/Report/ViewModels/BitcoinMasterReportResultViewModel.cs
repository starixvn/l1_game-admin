using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration.BetChoices;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BitcoinMasterReportResultViewModel
    {
        public BitcoinMasterRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<BitcoinMasterRecordViewModel> Records { get; set; }

        public string WinningOptions => GetWinningOptions();

        public BitcoinMasterRecordViewModel Total => GetTotalRow();

        private BitcoinMasterRecordViewModel GetTotalRow()
        {
            var total = new BitcoinMasterRecordViewModel();
            foreach (var r in Records)
            {
                total.Bet += r.Bet;
                total.BetWin += r.BetWin;
                total.Back += r.Back;
            }

            return total;
        }

        private string GetWinningOptions()
        {
            var winningOptions = new List<string>
            {
                RoundResult.SettlementResult[BitcoinMasterBetChoice.UpValue] ? "Up" :string.Empty,
                RoundResult.SettlementResult[BitcoinMasterBetChoice.DownValue] ? "Down" :string.Empty,
                RoundResult.SettlementResult[BitcoinMasterBetChoice.OddValue] ? "Odd" :string.Empty,
                RoundResult.SettlementResult[BitcoinMasterBetChoice.EvenValue] ? "Even" :string.Empty,
            };

            return string.Join(", ", winningOptions.Where(s => !string.IsNullOrWhiteSpace(s)));
        }
    }
}