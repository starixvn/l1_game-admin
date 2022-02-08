using GamesAdmin.Core.Enumeration;
using System;

namespace GamesAdmin.Core.Models.BetReport
{
    public class HoldemPokerBetReportFilter
    {
        public GameId GameId { get; set; }

        public string PartnerId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int RoundId { get; set; }

        public string Nickname { get; set; }

        public string Username { get; set; }

        public bool ExcludeUUS { get; set; }

        public bool ConvertToUSD { get; set; }
    }

    public class HoldemPokerBetHistoryFilter
    {
        public GameId GameId { get; set; }

        public int RoundId { get; set; }

        public string Nickname { get; set; }

        public bool ConvertToUSD { get; set; }
    }
}
