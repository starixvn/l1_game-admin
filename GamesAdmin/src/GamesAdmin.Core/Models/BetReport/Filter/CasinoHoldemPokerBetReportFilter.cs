using System;
using TimeZoneConverter;

namespace GamesAdmin.Core.Models.BetReport
{
    public class SEAsiaStandardTimeFilter
    {
        public TimeZoneInfo TimeZoneInfo => TZConvert.GetTimeZoneInfo("SE Asia Standard Time");
    }

    public class CasinoHoldemPokerBetReportFilter : SEAsiaStandardTimeFilter
    {
        public string PartnerId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int RoundId { get; set; }
        public string Nickname { get; set; }
        public string Username { get; set; }
        public bool ExcludeUUS { get; set; }
        public bool ConvertToUSD { get; set; }

    }

    public class CasinoHoldemPokerBetHistoryFilter : SEAsiaStandardTimeFilter
    {
        public int RoundId { get; set; }
        public string Nickname { get; set; }
        public bool ConvertToUSD { get; set; }
    }
}
