using GamesAdmin.Core.Enumeration;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class HoldemPokerReportResultViewModel
    {
        public HoldemPokerReportResultViewModel(bool convertToUSD, GameId gameId, IEnumerable<HoldemPokerRecordViewModel> records, int baseCurrencyMultiplier)
        {
            GameId = gameId;
            ConvertToUSD = convertToUSD;
            Records = records;
            BaseCurrencyMultiplier = baseCurrencyMultiplier;
        }

        public int BaseCurrencyMultiplier { get; }

        public GameId GameId { get; }

        public bool ConvertToUSD { get; }

        public IEnumerable<HoldemPokerRecordViewModel> Records { get; }
    }
}