using System;
using System.Collections.Generic;
using GamesAdmin.Core.Models.BetReport;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoBetHistoryViewModel
    {
        public KenoBetHistoryViewModel(IEnumerable<KenoBetHistory> betHistories, TimeZoneInfo timeZoneInfo)
        {
            BetHistories = betHistories;
            TimeZoneInfo = timeZoneInfo;
        }

        public IEnumerable<KenoBetHistory> BetHistories { get; }

        public TimeZoneInfo TimeZoneInfo { get; }
    }
}
