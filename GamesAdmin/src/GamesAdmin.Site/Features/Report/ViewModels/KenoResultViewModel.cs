using System;
using System.Linq;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Models.RoundResults;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoResultViewModel
    {
        public KenoResultViewModel(byte[] results, KenoSettlementResult settlementResult, DateTimeOffset? time)
        {
            Results = results;
            SettlementResult = settlementResult;
            Time = time;
        }

        public byte[] Results { get; }

        public KenoSettlementResult SettlementResult { get; }

        public DateTimeOffset? Time { get; }

        public int Total => Results.Sum(x => x);

        public string BigSmallResult => SettlementResult.Big ? KenoBetChoice.Big.DisplayName : KenoBetChoice.Small.DisplayName;

        public string OddEvenResult => SettlementResult.Odd ? KenoBetChoice.Odd.DisplayName : KenoBetChoice.Even.DisplayName;

        public string FiveElementsResult
        {
            get
            {
                if (SettlementResult.Gold) return KenoBetChoice.Gold.DisplayName;
                if (SettlementResult.Earth) return KenoBetChoice.Earth.DisplayName;
                if (SettlementResult.Fire) return KenoBetChoice.Fire.DisplayName;
                if (SettlementResult.Water) return KenoBetChoice.Water.DisplayName;
                if (SettlementResult.Wood) return KenoBetChoice.Wood.DisplayName;

                return string.Empty;
            }
        }

        public override string ToString()
        {
            return string.Join("; ", Results);
        }
    }
}
