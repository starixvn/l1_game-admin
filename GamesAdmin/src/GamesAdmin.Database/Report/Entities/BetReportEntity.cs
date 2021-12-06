using System;

namespace GamesAdmin.Database.Report.Entities
{
    public class BetReportEntity
    {
        public DateTime Time { get; set; }
        public string UserId { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public string Currency { get; set; }

        public string PartnerId { get; set; }

        public decimal Rate { get; set; }
    }
}