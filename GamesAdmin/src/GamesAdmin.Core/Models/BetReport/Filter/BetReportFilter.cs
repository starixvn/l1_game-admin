using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.ApiLogs;

namespace GamesAdmin.Core.Models.BetReport
{
    public class BetReportFilter
    {
        public Auth Auth { get; set; }
        public int Game { get; set; }
        public long RoundId { get; set; }
        public string Nickname { get; set; }
        public bool ExcludeUUS { get; set; }
        public bool ExcludeBot { get; set; }
        public string PartnerId { get; set; }
        public string Username { get; set; }
        public bool ConvertToUSD { get; set; }
    }
}