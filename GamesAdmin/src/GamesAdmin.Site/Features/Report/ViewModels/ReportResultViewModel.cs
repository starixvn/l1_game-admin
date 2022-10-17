namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class ReportResultViewModel
    {
        public bool ConvertToUSD { get; set; }
        public string BaseCurrency { get; set; }

        public int BaseCurrencyMultiplier { get; set; }

        public string GameName { get; set; }
    }
}