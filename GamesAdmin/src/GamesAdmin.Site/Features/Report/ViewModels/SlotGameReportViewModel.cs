using System;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Report.Validators;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class SlotGameReportViewModel : ReportViewModel
    {
        public DateTime FromDate { get; set; } = DateTime.Today;

        public DateTime ToDate { get; set; } = DateTime.Today;

        [SlotGameReportValidation]
        public new long? RoundId { get; set; }

        [SlotGameReportValidation]
        public new string Nickname { get; set; }

        [SlotGameReportValidation]
        public new string Username { get; set; }

        public SlotGame GameInfo { get; set; }
    }
}