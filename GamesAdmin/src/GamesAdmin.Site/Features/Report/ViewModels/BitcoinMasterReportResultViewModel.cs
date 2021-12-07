using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration.BetChoices;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BitcoinMasterReportResultViewModel
    {
        public string Currency { get; set; }

        public BitcoinMasterRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<BitcoinMasterRecordViewModel> Records { get; set; }

        public List<BitcoinMasterRecordViewModel> UpRecords
            => Records.Where(x => x.Choice == BitcoinMasterBetChoice.UpValue).ToList();

        public List<BitcoinMasterRecordViewModel> DownRecords
            => Records.Where(x => x.Choice == BitcoinMasterBetChoice.DownValue).ToList();

        public List<BitcoinMasterRecordViewModel> OddRecords
            => Records.Where(x => x.Choice == BitcoinMasterBetChoice.OddValue).ToList();

        public List<BitcoinMasterRecordViewModel> EvenRecords
            => Records.Where(x => x.Choice == BitcoinMasterBetChoice.EvenValue).ToList();

        public string WinningOptions => GetWinningOptions();

        public BitcoinMasterRecordViewModel UpTotal => GetTotalRow(UpRecords);
        public BitcoinMasterRecordViewModel DownTotal => GetTotalRow(DownRecords);
        public BitcoinMasterRecordViewModel OddTotal => GetTotalRow(OddRecords);
        public BitcoinMasterRecordViewModel EvenTotal => GetTotalRow(EvenRecords);

        private BitcoinMasterRecordViewModel GetTotalRow(IEnumerable<BitcoinMasterRecordViewModel> records)
        {
            var total = new BitcoinMasterRecordViewModel();
            foreach (var r in records)
            {
                total.Bet += r.Bet;
                total.Back += r.Back;
                total.BetWin += r.GrossWin;
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