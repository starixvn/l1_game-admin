using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class RouletteBetHistoryViewModel : BetHistoryViewModel
    {
        public RouletteRoundResultViewModel RoundResult { get; set; }

        public  IEnumerable<RouletteBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class RouletteBetHistoryRecordViewModel : BetHistoryRecordViewModel
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