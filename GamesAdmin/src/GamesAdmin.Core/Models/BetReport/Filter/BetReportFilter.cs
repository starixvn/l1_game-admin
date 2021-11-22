using System;

namespace GamesAdmin.Core.Models.BetReport
{
    public class BetReportFilter
    {
        public long RoundId { get; set; }
        public string Nickname { get; set; }
        public bool ExcludeUUS { get; set; }
        public bool ExcludeBot { get; set; }
        public string PartnerId { get; set; }
        public string UserName { get; set; }
    }
}