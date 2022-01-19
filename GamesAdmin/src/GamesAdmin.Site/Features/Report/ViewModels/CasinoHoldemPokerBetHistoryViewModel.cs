using GamesAdmin.Core.Models.BetReport;
using System;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CasinoHoldemPokerBetHistoryViewModel
    {
        public CasinoHoldemPokerBetHistoryViewModel(TimeZoneInfo timeZone, CasinoHoldemPokerBetHistory record)
        {
            TimeZone = timeZone;
            Record = record;
        }
        public TimeZoneInfo TimeZone { get; }
        public CasinoHoldemPokerBetHistory Record { get; }
    }
}
