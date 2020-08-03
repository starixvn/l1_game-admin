using System;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.DailySummary
{
    [BsonCollection("daily_summaries")]
    public class DailySummaryEntity : Document
    {
        public DateTime Time { get; set; }

        public int GameId { get; set; }

        public string Currency { get; set; }

        public decimal Stake { get; set; }

        public decimal Payout { get; set; }

        public int Tickets { get; set; }
    }
}