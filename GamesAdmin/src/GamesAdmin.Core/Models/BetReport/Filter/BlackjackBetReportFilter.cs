using System;

namespace GamesAdmin.Core.Models.BetReport
{
    public class BlackjackBetReportFilter 
    {
        public long HandId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Nickname { get; set; }
        public bool ExcludeUUS { get; set; }
        public string PartnerId { get; set; }
        public string Username { get; set; }
        public bool ConvertToUSD { get; set; }
        public bool ConvertToUSD { get; set; }
    }
}