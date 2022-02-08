using GamesAdmin.Core.Enumeration;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class HoldemPokerReportResultViewModel
    {
        public HoldemPokerReportResultViewModel(bool convertToUSD, GameId gameId, IEnumerable<HoldemPokerRecordViewModel> records)
        {
            GameId = gameId;
            ConvertToUSD = convertToUSD;
            Records = records;
        }

        public GameId GameId { get; }

        public bool ConvertToUSD { get; }

        public IEnumerable<HoldemPokerRecordViewModel> Records { get; }
    }
}
