namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class FanTanBetHistoryRowViewModel
    {
        public FanTanSettlementResultViewModel SettlementResult { get; set; }

        public FanTanBetHistoryRecordViewModel Bet { get; set; }

        public bool ConvertToUSD { get; set; }

        public int BaseCurrencyMultiplier { get; set; }
    }
}