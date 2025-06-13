using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class MiniRouletteBetHistoryViewModel : BetHistoryViewModel
    {
        public MiniRouletteRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<MiniRouletteBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class MiniRouletteBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public Dictionary<string, decimal> StraightUp { get; set; }

        public Dictionary<string, decimal> Split { get; set; }

        public Dictionary<string, decimal> Street { get; set; }

        public Dictionary<string, decimal> Triangle { get; set; }

        public Dictionary<string, decimal> Corner { get; set; }

        public Dictionary<string, decimal> FourNumbers { get; set; }

        public Dictionary<string, decimal> Line { get; set; }

        public Dictionary<string, decimal> Column { get; set; }

        public Dictionary<string, decimal> Dozen { get; set; }

        public Dictionary<string, decimal> RedBlack { get; set; }

        public Dictionary<string, decimal> OddEven { get; set; }

        public Dictionary<string, decimal> BigSmall { get; set; }
    }
}
