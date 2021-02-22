using System;
using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.Spins;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class LuckyWheelHistoryViewModel
    {
        public LuckyWheelHistoryViewModel(IEnumerable<SpinResult> spinResults, TimeZoneInfo timeZoneInfo)
        {
            SpinResults = spinResults;
            TimeZoneInfo = timeZoneInfo;
        }

        public IEnumerable<SpinResult> SpinResults { get; }

        public TimeZoneInfo TimeZoneInfo { get; }

        public int Total => SpinResults.Count();

        public int TotalWinningScoins => SpinResults.Sum(spin => spin.Result);
    }
}
