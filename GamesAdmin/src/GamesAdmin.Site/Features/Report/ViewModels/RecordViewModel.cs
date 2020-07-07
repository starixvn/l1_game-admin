using System;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class RecordViewModel
    {
        public DateTimeOffset Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal BetWin { get; set; }
    }

    public class BigSmallRecordViewModel : RecordViewModel
    {
        public decimal Back { get; set; }

        public bool Select { get; set; }

        public decimal Bet { get; set; }
    }

    public class OddEvenRecordViewModel : RecordViewModel
    {
        public decimal Back { get; set; }

        public bool Select { get; set; }

        public decimal Bet { get; set; }
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
}