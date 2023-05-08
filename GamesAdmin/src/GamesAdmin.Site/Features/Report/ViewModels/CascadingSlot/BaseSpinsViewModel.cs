using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BaseSpinsViewModel
    {
        public CascadingSlotTumbleViewModel Tumble { get; set; }

        public Dictionary<byte, CascadingSlotWinningSymbolViewModel> Icons => Tumble.Icons;

        public int Rowspan => Tumble.Icons.Keys.Count;

        public decimal TumbleWin { get; set; }

        public byte RowIndex { get; set; }

        public string GameName { get; set; }

        public BaseSpinsViewModel(CascadingSlotTumbleViewModel tumble, decimal tumbleWin, byte rowIndex, string gameName)
        {
            Tumble = tumble;
            TumbleWin = tumbleWin;
            RowIndex = rowIndex;
            GameName = gameName;
        }
    }
}
