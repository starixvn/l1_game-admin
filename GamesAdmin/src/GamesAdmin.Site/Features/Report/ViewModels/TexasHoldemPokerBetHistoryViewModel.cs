using GamesAdmin.Core.Models.BetReport;
using System;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class TexasHoldemPokerBetHistoryViewModel
    {
        public TexasHoldemPokerBetHistoryViewModel(TimeZoneInfo timeZone, TexasHoldemPokerBetHistory record)
        {
            TimeZone = timeZone;
            Record = record;
        }
        public TimeZoneInfo TimeZone { get; }
        public TexasHoldemPokerBetHistory Record { get; }
    }
}
