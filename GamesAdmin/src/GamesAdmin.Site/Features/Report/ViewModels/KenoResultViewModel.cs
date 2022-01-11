using System;
using System.Linq;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Models.RoundResults;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoResultViewModel : BaseRoundResultViewModel
    {
        public KenoResultViewModel(byte[] results, KenoSettlementResult settlementResult, DateTime? time)
        {
            Results = results;
            SettlementResult = settlementResult;
            Time = time;
        }

        public byte[] Results { get; }

        public KenoSettlementResult SettlementResult { get; }

        public DateTime? Time { get; }

        public int Total => Results.Sum(x => x);

        public string BigSmallResult => SettlementResult.Big ? KenoBetChoice.Big.DisplayName : KenoBetChoice.Small.DisplayName;

        public string OddEvenResult => SettlementResult.Odd ? KenoBetChoice.Odd.DisplayName : KenoBetChoice.Even.DisplayName;

        public string FiveElementsResult
            => SettlementResult.Gold ? KenoBetChoice.Gold.DisplayName
                    : SettlementResult.Earth ? KenoBetChoice.Earth.DisplayName
                    : SettlementResult.Fire ? KenoBetChoice.Fire.DisplayName
                    : SettlementResult.Water ? KenoBetChoice.Water.DisplayName
                    : SettlementResult.Wood ? KenoBetChoice.Wood.DisplayName
                    : string.Empty;


        public override string ToString()
        {
            return string.Join("; ", Results);
        }
    }
}

