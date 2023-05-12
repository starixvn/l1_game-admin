using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CascadingSlotBetHistoryViewModel
    {
        private const string SymbolPrefix = "symbol";

        public CascadingSlotRecordViewModel Round { get; set; }

        public IEnumerable<CascadingSlotRoundMultiplierViewModel> Multipliers { get; set; }

        public bool ConvertToUSD { get; set; }

        public string Game { get; set; }

        public string StaticResourceUrl { get; set; }

        public string GameName => Game?.ToLowerInvariant();

        public IEnumerable<CascadingSlotTumbleViewModel> Tumbles => Round.SettlementResult.Tumbles;

        public bool IsMultipliers => Multipliers != null && Multipliers.Any();

        public string WinLabel => Round.ReachMaxWin ? "(You reached the Max Win) Win" : "Win";

        public bool HasTotalMultiplier => Round.BetWin > 0 && Round.Multiplier > 0;

        public string SymbolUrl => $"{StaticResourceUrl}/{GameName}/{SymbolPrefix}";

        public byte MultiplierValue => Round.FreeSpin? Round.TotalMultiplier : Round.Multiplier;
    }
}
