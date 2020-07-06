using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoReportViewModel
    {
        public GameType Game { get; set; }

        public long RoundId { get; set; }

        public KenoResultViewModel RoundResult { get; set; }

        public IEnumerable<KenoBetRecord> BetTickets { get; set; }

        public int Total => BetTickets?.Count() == 0 ? 0 : BetTickets.Count();
    }
}
