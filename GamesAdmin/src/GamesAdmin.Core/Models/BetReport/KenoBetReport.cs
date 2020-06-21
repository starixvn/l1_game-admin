using System.Collections.Generic;
using GamesAdmin.Core.Models.RoundResults;

namespace GamesAdmin.Core.Models.BetReport
{
    public class KenoBetReport
    {
        public KenoBetReport(
            KenoRoundResult roundResult,
            IEnumerable<KenoBetRecord> betRecords)
        {
            RoundResult = roundResult;
            BetRecords = betRecords;
        }

        public KenoRoundResult RoundResult { get; }

        public IEnumerable<KenoBetRecord> BetRecords { get; }
    }
}
