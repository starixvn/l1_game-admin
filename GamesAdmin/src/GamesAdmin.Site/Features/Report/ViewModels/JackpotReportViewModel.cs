using System;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class JackpotReportViewModel
    {
        public int GameId { get; set; }

        public DateTime FromDate { get; set; } = DateTime.Now;

        public DateTime ToDate { get; set; } = DateTime.Now;

        public bool ExcludeBot { get; set; } = true;

        public bool ExcludeUUS { get; set; } = true;

        public bool ConvertToINH { get; set; } = true;

        public string BaseCurrency { get; set; }

        public bool EnableConvertBase { get; set; }
    }
}
