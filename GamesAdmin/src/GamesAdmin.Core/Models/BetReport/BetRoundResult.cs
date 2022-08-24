using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class RoundResult
    {
        public DateTime Time { get; set; }

        public long Number { get; set; }

        public int ExchangeRateId { get; set; }
    }

    public class BigSmallRoundResult : RoundResult
    {
        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }

        public BigSmallSettlementResult SettlementResult { get; set; }
    }

    public class BigSmallSettlementResult
    {
        public bool Big { get; set; }

        public bool Small { get; set; }
    }

    public class OddEvenRoundResult : RoundResult
    {
        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }
    }

    public class SeDieRoundResult : RoundResult
    {
        public IEnumerable<bool> Chips { get; set; }

        public bool Odd { get; set; }

        public bool Even { get; set; }

        public bool TwoRed { get; set; }

        public bool ThreeRed { get; set; }

        public bool FourRed { get; set; }

        public bool ThreeWhite { get; set; }

        public bool FourWhite { get; set; }
    }

    public class FishPrawnCrabRoundResult : RoundResult
    {
        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }
    }

    public class DragonTigerRoundResult : RoundResult
    {
        public DragonTigerCardResult Dragon { get; set; }

        public DragonTigerCardResult Tiger { get; set; }
    }

    public class DragonTigerCardResult
    {
        public byte Number { get; set; }

        public byte Type { get; set; }
    }

    public class BaccaratRoundResult : RoundResult
    {
        public IEnumerable<DragonTigerCardResult> PlayerCards { get; set; }

        public IEnumerable<DragonTigerCardResult> BankerCards { get; set; }

        public BaccaratSettlementResult Result { get; set; }
    }

    public class BaccaratSettlementResult
    {
        public bool Player { get; set; }
        public bool Banker { get; set; }
        public bool Tie { get; set; }
        public bool Big { get; set; }
        public bool Small { get; set; }
        public bool PlayerPair { get; set; }
        public bool BankerPair { get; set; }
        public bool Natural { get; set; }
    }

    public class RouletteRoundResult : RoundResult
    {
        public byte Result { get; set; }

        public RouletteSettlementResult SettlementResult { get; set; }
    }

    public class RouletteSettlementResult
    {
        public IEnumerable<string> StraightUp { get; set; }

        public IEnumerable<string> Split { get; set; }

        public IEnumerable<string> Street { get; set; }

        public IEnumerable<string> Triangle { get; set; }

        public IEnumerable<string> Corner { get; set; }

        public IEnumerable<string> FourNumbers { get; set; }

        public IEnumerable<string> Line { get; set; }

        public IEnumerable<string> Column { get; set; }

        public IEnumerable<string> Dozen { get; set; }

        public IEnumerable<string> RedBlack { get; set; }

        public IEnumerable<string> OddEven { get; set; }

        public IEnumerable<string> BigSmall { get; set; }
    }

    public class VipFishPrawnCrabRoundResult : RoundResult
    {
        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }
    }

    public class SicBoRoundResult : RoundResult
    {
        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class VipSeDieWarRoundResult : RoundResult
    {
        public IEnumerable<bool> PlayerChips { get; set; }

        public IEnumerable<bool> BankerChips { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class LotteryRoundResult : RoundResult
    {
        public IEnumerable<string> Special { get; set; }
        public IEnumerable<string> First { get; set; }
        public IEnumerable<string> Second { get; set; }
        public IEnumerable<string> Third { get; set; }
        public IEnumerable<string> Fourth { get; set; }
        public IEnumerable<string> Fifth { get; set; }
        public IEnumerable<string> Sixth { get; set; }
        public IEnumerable<string> Seventh { get; set; }

        public LotterySettlementResult SettlementResult { get; set; }
    }

    public class LotterySettlementResult
    {
        public IEnumerable<string> FourSets { get; set; }

        public string Special { get; set; }

        public string Head { get; set; }

        public string Tail { get; set; }

        public Dictionary<string, bool> Others { get; set; }

        public IEnumerable<string> TwentySevenSets { get; set; }

        public LotteryOccurrences Occurrences { get; set; }
    }

    public class LotteryOthersSettlementResult
    {
        public bool Odd { get; set; }
        public bool Even { get; set; }
        public bool Big { get; set; }
        public bool Small { get; set; }
    }

    public class LotteryOccurrences
    {
        public Dictionary<string, byte> All { get; set; }
        public Dictionary<string, byte> Seventh { get; set; }
    }

    public class ThreeCardsRoundResult : RoundResult
    {
        public IEnumerable<Card> Result { get; set; }
        public ThreeCardsSettlementResult SettlementResult { get; set; }
    }

    public class ThreeCardsSettlementResult
    {
        public bool Red { get; set; }
        public bool Black { get; set; }
        public bool Diamonds { get; set; }
        public bool Hearts { get; set; }
        public bool Spades { get; set; }
        public bool Clubs { get; set; }
    }

    public class BitcoinMasterRoundResult : RoundResult
    {
        public decimal BettingPrice { get; set; }

        public decimal ResultPrice { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class KenoMatchRoundResult : RoundResult
    {
        public KenoMatchPeriod HalfTime { get; set; }

        public KenoMatchPeriod FullTime { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
    }

    public class KenoMatchPeriod
    {
        public DateTime Time { get; set; }

        public int Total { get; set; }

        public KenoMatchPeriodResult Result { get; set; }

        public KenoMatchPeriodSettlementResult SettlementResult { get; set; }
    }

    public class KenoMatchPeriodResult
    {
        public IEnumerable<int> Blue { get; set; }
        public IEnumerable<int> Red { get; set; }
    }

    public class KenoMatchPeriodSettlementResult
    {
        public bool Blue { get; set; }
        public bool Red { get; set; }
        public bool Over { get; set; }
        public bool Under { get; set; }
        public bool Odd { get; set; }
        public bool Even { get; set; }
    }

    public class KenoPowerRoundResult : RoundResult
    {
        public KenoPowerResult Result { get; set; }
        public KenoPowerSettlementResult SettlementResult { get; set; }
    }

    public class KenoPowerResult
    {
        public IEnumerable<byte> Blue { get; set; }
        public IEnumerable<byte> Red { get; set; }
    }

    public class KenoPowerSettlementResult
    {
        public bool Blue { get; set; }
        public bool Red { get; set; }
        public bool Big { get; set; }
        public bool Small { get; set; }
        public bool Odd { get; set; }
        public bool Even { get; set; }
        public bool BlueBig { get; set; }
        public bool BlueSmall { get; set; }
        public bool RedBig { get; set; }
        public bool RedSmall { get; set; }
    }

    public class HiLoRoundResult : RoundResult
    {
        public DateTime? EndTime { get; set; }

        public decimal Amount { get; set; }

        public decimal Betwin { get; set; }

        public bool Paid { get; set; }

        public string Currency { get; set; }
        public string PartnerId { get; set; }
        public decimal Rate { get; set; }
    }
}