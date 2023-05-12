using GamesAdmin.Site.Features._Shared.Helpers;
namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class FreeSpinsViewModel
    {
        public FreeSpinsViewModel(CascadingSlotTumbleViewModel tumble,
           SlotGameFreeSpinViewModel freeSpins,
           byte rowIndex,
           decimal tumbleWin,
           bool hasFreeSpins,
           string symbolUrl)
        {
            Tumble = tumble;
            FreeSpins = freeSpins;
            RowIndex = rowIndex;
            TumbleWin = tumbleWin;
            HasFreeSpins = hasFreeSpins;
            SymbolUrl = symbolUrl;
        }

        public CascadingSlotTumbleViewModel Tumble { get; }

        public SlotGameFreeSpinViewModel FreeSpins { get; }

        public decimal TumbleWin { get; }

        public byte RowIndex { get; }

        public bool HasFreeSpins { get; }

        public string FreeSpinsText => HasFreeSpins ? "Extra Free Spins" : "Free Spins";

        public bool HasOdds => FreeSpins != null && FreeSpins.Odds > 0;

        public string SymbolUrl { get; }

        public string DisplayTumbleWin => HtmlHelpers.formatNumber(TumbleWin);
    }
}
