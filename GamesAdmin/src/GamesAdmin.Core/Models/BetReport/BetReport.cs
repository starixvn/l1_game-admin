using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class BetReport<TRoundResult, TBetRecord> where TRoundResult : RoundResult where TBetRecord : BetRecord
    {
        public TRoundResult RoundResult { get; set; }
        public IEnumerable<TBetRecord> Records { get; set; }
    }

    public class BigSmallBetReport : BetReport<BigSmallRoundResult, BigSmallBetRecord>
    {
    }

    public class OddEvenBetReport : BetReport<OddEvenRoundResult, OddEvenBetRecord>
    {
    }

    public class SeDieBetReport : BetReport<SeDieRoundResult, SeDieBetRecord>
    {
    }

    public class FishPrawnCrabBetReport : BetReport<FishPrawnCrabRoundResult, FishPrawnCrabBetRecord>
    {
    }

    public class DragonTigerBetReport : BetReport<DragonTigerRoundResult, DragonTigerBetRecord>
    {
    }

    public class BaccaratBetReport : BetReport<BaccaratRoundResult, BaccaratBetRecord>
    {
    }
}