using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CasinoHoldemPokerReportResultViewModel
    {
        public CasinoHoldemPokerReportResultViewModel(bool convertToUSD, IEnumerable<CasinoHoldemPokerRecordViewModel> records)
        {
            ConvertToUSD = convertToUSD;
            Records = records;
        }

        public bool ConvertToUSD { get; }
        public IEnumerable<CasinoHoldemPokerRecordViewModel> Records { get; }
    }
}
