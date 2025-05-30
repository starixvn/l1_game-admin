using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class JackpotPoolsReportViewModel
    {
        public int GameId { get; set; }

        public string OperatorId { get; set; }

        public string SiteNames { get; set; }

        public IEnumerable<SelectListItem> Operators { get; set; }
    }
}
