using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class HoldemPokerReportResultViewModel
    {
        public HoldemPokerReportResultViewModel(bool convertToUSD, string betHistoryUrl, IEnumerable<HoldemPokerRecordViewModel> records)
        {
            ConvertToUSD = convertToUSD;
            BetHistoryUrl = betHistoryUrl;
            Records = records;
        }

        public bool ConvertToUSD { get; }

        public string BetHistoryUrl { get; }

        public IEnumerable<HoldemPokerRecordViewModel> Records { get; }
    }
}
