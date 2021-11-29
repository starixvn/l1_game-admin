using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class BetReportBase
    {

    }
    public class BetReport<TRoundResult, TBetRecord> : BetReportBase where TRoundResult : RoundResult where TBetRecord : BetRecord
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

    public class VipBaccaratBetReport : BetReport<BaccaratRoundResult, BaccaratBetRecord>
    {
    }

    public class RouletteBetReport : BetReport<RouletteRoundResult, RouletteBetRecord>
    {
    }

    public class VipSeDieBetReport : BetReport<SeDieRoundResult, VipSeDieBetRecord>
    {
    }

    public class SpecialSeDieBetReport : BetReport<SeDieRoundResult, SpecialSeDieBetRecord>
    {
    }

    public class VipFishPrawnCrabBetReport : BetReport<VipFishPrawnCrabRoundResult, VipFishPrawnCrabBetRecord>
    {
    }

    public class SicBoBetReport : BetReport<SicBoRoundResult, SicBoBetRecord>
    {
    }

    public class VipSeDieWarBetReport : BetReport<VipSeDieWarRoundResult, VipSeDieWarBetRecord>
    {
    }

    public class LotteryBetReport : BetReport<LotteryRoundResult, LotteryBetRecord>
    {
    }

    public class ThreeCardsBetReport : BetReport<ThreeCardsRoundResult, ThreeCardsBetRecord>
    {
    }

    public class BitcoinMasterBetReport : BetReport<BitcoinMasterRoundResult, BitcoinMasterBetRecord>
    {
    }

    public class KenoMatchBetReport : BetReport<KenoMatchRoundResult, KenoMatchBetRecord>
    {
    }

    public class KenoPowerBetReport : BetReport<KenoPowerRoundResult, KenoPowerBetRecord>
    {
    }

    public class CasinoHoldemPokerBetReport : BetReport<CasinoHoldemPokerRoundResult, CasinoHoldemPokerBetRecord>
    {
    }
}