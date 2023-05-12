using GamesAdmin.Site.Features._Shared.Helpers;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BaseSpinsViewModel
    {
        public BaseSpinsViewModel(CascadingSlotTumbleViewModel tumble, decimal tumbleWin, byte rowIndex, string symbolUrl)
        {
            Tumble = tumble;
            TumbleWin = tumbleWin;
            RowIndex = rowIndex;
            SymbolUrl = symbolUrl;
        }

        public CascadingSlotTumbleViewModel Tumble { get; }

        public Dictionary<byte, CascadingSlotWinningSymbolViewModel> Icons => Tumble.Icons;

        public int Rowspan => Tumble.Icons.Keys.Count;

        public decimal TumbleWin { get; }

        public byte RowIndex { get; }

        public string SymbolUrl { get; }

        public string DisplayTumbleWin => HtmlHelpers.formatNumber(TumbleWin);
    }
}
