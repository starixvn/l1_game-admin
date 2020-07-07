using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class RoundResultViewModel
    {
        public long Number { get; set; }

        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }
    }

    public class SeDieRoundResultViewModel
    {
        public long Number { get; set; }

        public IEnumerable<bool> Chips { get; set; }
    }
}