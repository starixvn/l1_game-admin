using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BigSmallReportViewModel
    {
        public long RoundId { get; set; }

        public string Nickname { get; set; }

        public bool ExcludeBot { get; set; }

        public IEnumerable<SelectListItem> Rounds { get; set; }
    }
}