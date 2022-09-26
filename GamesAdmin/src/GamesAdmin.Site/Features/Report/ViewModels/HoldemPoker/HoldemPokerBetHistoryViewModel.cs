using GamesAdmin.Core.Models.BetReport;
using System;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class HoldemPokerBetHistoryViewModel
    {
        public HoldemPokerBetHistoryViewModel(TimeZoneInfo timeZone, HoldemPokerBetHistory record, int baseCurrencyMultipler)
        {
            TimeZone = timeZone;
            Record = record;
            BaseCurrencyMultiplier = baseCurrencyMultipler;
        }
        public TimeZoneInfo TimeZone { get; }
        public HoldemPokerBetHistory Record { get; }

        public int BaseCurrencyMultiplier {get;}
    }
}
