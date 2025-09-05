using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Models.BetReport
{
    public class BetHistoryFilter
    {
        public int Game { get; set; }
        public long RoundId { get; set; }
        public string UserId { get; set; }
        public string PartnerId { get; set; }
    }
}