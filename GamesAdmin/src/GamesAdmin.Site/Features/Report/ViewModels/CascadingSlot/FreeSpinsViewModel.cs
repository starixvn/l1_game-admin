
namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class FreeSpinsViewModel
    {
        public CascadingSlotTumbleViewModel Tumble { get; set; }

        public SlotGameFreeSpinViewModel FreeSpins { get; set; }

        public string GameName { get; set; }

        public decimal TumbleWin { get; set; }

        public byte RowIndex { get; set; }

        public bool HasFreeSpins { get; set; }

        public string FreeSpinsText => HasFreeSpins ? "Extra Free Spins" : "Free Spins";

        public FreeSpinsViewModel(CascadingSlotTumbleViewModel tumble, 
            SlotGameFreeSpinViewModel freeSpins, 
            byte rowIndex, 
            decimal tumbleWin, 
            string gameName,
            bool hasFreeSpins)
        {
            GameName = gameName;
            Tumble = tumble;
            FreeSpins = freeSpins;
            RowIndex = rowIndex;
            TumbleWin = tumbleWin;
            HasFreeSpins = hasFreeSpins;
        }
    }
}
