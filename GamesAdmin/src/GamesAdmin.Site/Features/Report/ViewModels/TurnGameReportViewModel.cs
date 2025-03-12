using System;
using GamesAdmin.Site.Features.Report.Validators;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class TurnGameReportViewModel : ReportViewModel
    {
        public DateTime FromDate { get; set; } = DateTime.Today;

        public DateTime ToDate { get; set; } = DateTime.Today;

        [TurnGameReportValidation]
        public new long? RoundId { get; set; }

        [TurnGameReportValidation]
        public new string Nickname { get; set; }

        [TurnGameReportValidation]
        public new string Username { get; set; }

        public string GameConfigName { get; set; }

        public string GameDisplayName { get; set; }

        public int GameId { get; set; }
    }
}