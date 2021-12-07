using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Models.BetReport.Filter
{
    public class BetHistoryFilter
    {
        public GameId Game { get; set; }
        public long RoundId { get; set; }
        public string UserId { get; set; }
    }
}