using GamesAdmin.Site.Features.Report.Validators;
namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CrashGameReportViewModel : ReportViewModel
    {
        public string GameConfigName { get; set; }

        public string GameDisplayName { get; set; }

        public int GameId { get; set; }
    }
}
