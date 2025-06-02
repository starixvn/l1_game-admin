using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class JackpotReportViewModel
    {
        public int GameId { get; set; }

        public DateTime FromDate { get; set; } = DateTime.Now;

        public DateTime ToDate { get; set; } = DateTime.Now;

        public bool ExcludeBot { get; set; } = true;

        public bool ExcludeUUS { get; set; } = true;

        public bool ConvertToUSD { get; set; } = true;

        public string BaseCurrency { get; set; }

        public bool EnableConvertBase { get; set; }

        public string OperatorId { get; set; }

        public string SiteNames { get; set; }

        public IEnumerable<SelectListItem> Operators { get; set; }
    }
}
