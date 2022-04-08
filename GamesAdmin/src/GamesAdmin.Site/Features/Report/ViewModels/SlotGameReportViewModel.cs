using GamesAdmin.Site.Features.Report.Validators;
using System;
using System.ComponentModel;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class SlotGameReportViewModel : ReportViewModel
    {
        [DisplayName("Partner")]
        public string PartnerId { get; set; }

        public DateTime FromDate { get; set; } = DateTime.Today;
        public DateTime ToDate { get; set; } = DateTime.Today;

        [DisplayName("Convert to USD")]
        public bool ConvertToUSD { get; set; }

        [SlotGameReportValidation]
        public new long? RoundId { get; set; }

        [SlotGameReportValidation]
        public new string Nickname { get; set; }

        [SlotGameReportValidation]
        public new string Username { get; set; }
    }
}