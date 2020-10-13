using System;
using System.Collections.Generic;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Database.Report.Entities
{
    public class BlackjackBetReportEntity
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public long HandId { get; set; }

        public decimal BetWin { get; set; }

        public decimal Bet { get; set; }

        public byte Point { get; set; }

        public long? SplitForm { get; set; }

        public long Round { get; set; }

        public IEnumerable<BlackjackCardEntity> Cards { get; set; }

        public IEnumerable<BlackjackCardEntity> DealerCards { get; set; }

        public bool End { get; set; }

        public byte DealerPoint { get; set; }
    }
}