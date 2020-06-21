using System.Collections.Generic;
using GamesAdmin.Core.Models.BetReport;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoProVietReportViewModel
    {
        public KenoResultViewModel RoundResult { get; set; }

        public IEnumerable<KenoBetRecord> BetTickets { get; set; }
    }
}
