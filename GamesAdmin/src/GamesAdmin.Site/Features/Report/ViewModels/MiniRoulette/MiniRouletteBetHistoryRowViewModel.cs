namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class MiniRouletteBetHistoryRowViewModel
    {
        public MiniRouletteSettlementResultViewModel SettlementResult { get; set; }

        public MiniRouletteBetHistoryRecordViewModel Bet { get; set; }

        public bool ConvertToUSD { get; set; }

        public int BaseCurrencyMultiplier { get; set; }
    }
}
