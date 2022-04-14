using GamesAdmin.Core.Models.BetReport;
using System;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class HoldemPokerBetHistoryViewModel
    {
        public HoldemPokerBetHistoryViewModel(TimeZoneInfo timeZone, HoldemPokerBetHistory record)
        {
            TimeZone = timeZone;
            Record = record;
        }
        public TimeZoneInfo TimeZone { get; }
        public HoldemPokerBetHistory Record { get; }
    }
}
