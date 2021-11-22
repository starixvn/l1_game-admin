using System;
using System.Collections.Generic;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Database.Report.Entities
{
    public class BlackjackBetReportEntity: BetReportEntity
    {
        public long HandId { get; set; }

        public int ExchangeRateId { get; set; }

        public decimal BetWin { get; set; }

        public decimal Bet { get; set; }

        public byte Point { get; set; }

        public long? SplitForm { get; set; }

        public long Round { get; set; }

        public IEnumerable<CardEntity> Cards { get; set; }

        public IEnumerable<CardEntity> DealerCards { get; set; }

        public bool Ended { get; set; }

        public byte DealerPoint { get; set; }

        public IEnumerable<string> Actions { get; set; }

        public IEnumerable<byte> SoftPoints { get; set; }

        public string Currency { get; set; }

        public string PartnerId { get; set; }

        public double Rate { get; set; }
    }
}