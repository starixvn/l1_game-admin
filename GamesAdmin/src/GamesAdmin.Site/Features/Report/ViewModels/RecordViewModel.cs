using System;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class RecordViewModel : BaseBetViewModel
    {
    }

    public class BigSmallRecordViewModel : RecordViewModel
    {
        public bool Select { get; set; }
    }
    public class OddEvenRecordViewModel : RecordViewModel
    {
        public bool Select { get; set; }
    }

    public class SeDieRecordViewModel : RecordViewModel
    {
        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public decimal ThreeRed { get; set; }

        public decimal FourRed { get; set; }

        public decimal ThreeWhite { get; set; }

        public decimal FourWhite { get; set; }
    }

    public class FishPrawnCrabRecordViewModel : RecordViewModel
    {
        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }
    }
}