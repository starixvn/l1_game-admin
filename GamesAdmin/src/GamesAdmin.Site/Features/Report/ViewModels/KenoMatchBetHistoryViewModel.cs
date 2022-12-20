using System;
using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration.BetChoices;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoMatchBetHistoryViewModel : BetHistoryViewModel
    {
        public KenoMatchRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<KenoMatchBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class KenoMatchBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public decimal GetRefund(KenoMatchPeriodSettlementResultViewModel settlementResult)
        {
            var teamChoices = new[] {
                KenoMatchBetChoice.FHBlueValue,
                KenoMatchBetChoice.FHRedValue,
                KenoMatchBetChoice.FTBlueValue,
                KenoMatchBetChoice.FTRedValue,
                KenoMatchBetChoice.LiveFTBlueValue,
                KenoMatchBetChoice.LiveFTRedValue,
            };

            if (teamChoices.Contains(Choice) && !settlementResult.Blue && !settlementResult.Red)
            {
                return Amount;
            }

            return 0;
        }
    }
}