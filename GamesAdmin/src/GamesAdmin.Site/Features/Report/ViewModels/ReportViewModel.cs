using System;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class ReportViewModel
    {
        public long? RoundId { get; set; }

        public string Nickname { get; set; }

        public bool ExcludeBot { get; set; }

        public bool ExcludeUUS { get; set; }

        public string Game { get; set; }
    }

    public class BlackjackReportViewModel
    {
        public long? HandId { get; set; }

        public string Nickname { get; set; }

        public bool ExcludeUUS { get; set; }
    }

    public class JackpotReportViewModel
    {
        public byte GameId { get; set; }

        public DateTime FromDate { get; set; } = DateTime.Now;

        public DateTime ToDate { get; set; } = DateTime.Now;

        public bool ExcludeBot { get; set; } = true;

        public bool ExcludeUUS { get; set; } = true;
    }

    public class KenoMatchReportViewModel :ReportViewModel
    {
        public GameId Game { get; set; }
    }
}