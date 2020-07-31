namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class ReportViewModel
    {
        public long? RoundId { get; set; }

        public string Nickname { get; set; }

        public bool ExcludeBot { get; set; }

        public bool ExcludeUUS { get; set; }
    }
}