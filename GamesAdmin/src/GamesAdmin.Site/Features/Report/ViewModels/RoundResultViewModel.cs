using System;
using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public abstract class BaseRoundResultViewModel
    {
        public DateTime Time { get; set; }

        public long Number { get; set; }

        public int ExchangeRateId { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal? ConvertByExchangeRate(decimal? value)
        {
            var exchangeRate = ExchangeRate == 0 ? 1 : ExchangeRate;

            return value != null ? value / exchangeRate : null;
        }
    }

    public class RoundResultViewModel : BaseRoundResultViewModel
    {
        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }

        public int Total => Dice1 + Dice2 + Dice3;

        public bool IsBig => Total > 10;

        public bool IsEven => Total % 2 == 0;
    }

    public class SeDieRoundResultViewModel : BaseRoundResultViewModel
    {
        public long Number { get; set; }

        public IEnumerable<bool> Chips { get; set; }

        public int TotalRed => Chips.Count(x => x);

        public bool Even { get; set; }

        public bool Odd { get; set; }

        public bool TwoRed { get; set; }

        public bool ThreeRed { get; set; }

        public bool FourRed { get; set; }

        public bool ThreeWhite { get; set; }

        public bool FourWhite { get; set; }
    }

    public class FishPrawnCrabRoundResultViewModel : BaseRoundResultViewModel
    {
        public long Number { get; set; }

        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }
    }

    public class DragonTigerRoundResultViewModel : BaseRoundResultViewModel
    {
        public long Number { get; set; }

        public DragonTigerCardViewModel Dragon { get; set; }

        public DragonTigerCardViewModel Tiger { get; set; }
    }

    public class DragonTigerCardViewModel
    {
        public byte Number { get; set; }

        public byte Type { get; set; }
    }

    public class BaccaratRoundResultViewModel : BaseRoundResultViewModel
    {
        public long Number { get; set; }

        public IEnumerable<DragonTigerCardViewModel> PlayerCards { get; set; }

        public IEnumerable<DragonTigerCardViewModel> BankerCards { get; set; }

        public BaccaratSettlementResultViewModel Result { get; set; }
    }

    public class BaccaratSettlementResultViewModel
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

    public class RouletteRoundResultViewModel : BaseRoundResultViewModel
    {
        public long Number { get; set; }

        public byte Result { get; set; }

        public RouletteSettlementResultViewModel SettlementResult { get; set; }
    }

    public class RouletteSettlementResultViewModel
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

    public class VipFishPrawnCrabRoundResultViewModel : BaseRoundResultViewModel
    {
        public long Number { get; set; }

        public IEnumerable<byte> Result { get; set; }
    }

    public class SicBoRoundResultViewModel : BaseRoundResultViewModel
    {
        public long Number { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class VipSeDieWarRoundResultViewModel : BaseRoundResultViewModel
    {
        public long Number { get; set; }

        public IEnumerable<bool> PlayerChips { get; set; }
        public IEnumerable<bool> BankerChips { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class LotteryRoundResultViewModel : BaseRoundResultViewModel
    {
        public long Number { get; set; }

        public IEnumerable<string> Special { get; set; }
        public IEnumerable<string> First { get; set; }
        public IEnumerable<string> Second { get; set; }
        public IEnumerable<string> Third { get; set; }
        public IEnumerable<string> Fourth { get; set; }
        public IEnumerable<string> Fifth { get; set; }
        public IEnumerable<string> Sixth { get; set; }
        public IEnumerable<string> Seventh { get; set; }

        public LotterySettlementResultViewModel SettlementResult { get; set; }
    }

    public class LotterySettlementResultViewModel
    {
        public IEnumerable<string> FourSets { get; set; }

        public string Special { get; set; }

        public string Head { get; set; }

        public string Tail { get; set; }

        public Dictionary<string, bool> Others { get; set; }

        public IEnumerable<string> TwentySevenSets { get; set; }

        public LotteryOccurrencesViewModel Occurrences { get; set; }
    }

    public class LotteryOthersSettlementResultViewModel
    {
        public bool Odd { get; set; }
        public bool Even { get; set; }
        public bool Big { get; set; }
        public bool Small { get; set; }
    }

    public class LotteryOccurrencesViewModel
    {
        public Dictionary<string, byte> All { get; set; }
        public Dictionary<string, byte> Seventh { get; set; }
    }

    public class ThreeCardsRoundResultViewModel : BaseRoundResultViewModel
    {
        public IEnumerable<CardViewModel> Result { get; set; }

        public ThreeCardsSettlementResultViewModel SettlementResult { get; set; }
    }

    public class ThreeCardsSettlementResultViewModel
    {
        public bool Red { get; set; }
        public bool Black { get; set; }
        public bool Diamonds { get; set; }
        public bool Hearts { get; set; }
        public bool Spades { get; set; }
        public bool Clubs { get; set; }
    }

    public class BitcoinMasterRoundResultViewModel : BaseRoundResultViewModel
    {
        public long Number { get; set; }

        public decimal BettingPrice { get; set; }

        public decimal ResultPrice { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }

        public bool IsUp => ResultPrice > BettingPrice;
        public bool IsDown => ResultPrice < BettingPrice;

        public decimal PriceDiff => Math.Abs(ResultPrice - BettingPrice);

        public bool IsEven => Math.Floor(ResultPrice) % 10 == 0;
    }

    public class KenoMatchRoundResultViewModel : BaseRoundResultViewModel
    {
        public long Number { get; set; }

        public KenoMatchPeriodViewModel HalfTime { get; set; }

        public KenoMatchPeriodViewModel FullTime { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
    }

    public class KenoMatchPeriodViewModel
    {
        public DateTime Time { get; set; }

        public int Total { get; set; }

        public KenoMatchPeriodResultViewModel Result { get; set; }

        public KenoMatchPeriodSettlementResultViewModel SettlementResult { get; set; }
    }

    public class KenoMatchPeriodResultViewModel
    {
        public IEnumerable<int> Blue { get; set; }
        public IEnumerable<int> Red { get; set; }
    }

    public class KenoMatchPeriodSettlementResultViewModel
    {
        public bool Blue { get; set; }
        public bool Red { get; set; }
        public bool Over { get; set; }
        public bool Under { get; set; }
        public bool Odd { get; set; }
        public bool Even { get; set; }
    }

    public class KenoPowerRoundResultViewModel : BaseRoundResultViewModel
    {
        public KenoPowerResultViewModel Result { get; set; }

        public KenoPowerSettlementResultViewModel SettlementResult { get; set; }
    }

    public class KenoPowerResultViewModel
    {
        public IEnumerable<byte> Blue { get; set; }
        public IEnumerable<byte> Red { get; set; }
    }

    public class KenoPowerSettlementResultViewModel
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

    public class HiLoRoundResultViewModel : BaseRoundResultViewModel
    {
        public DateTime? EndTime { get; set; }
        public decimal Amount { get; set; }
        public decimal Betwin { get; set; }
        public bool Paid { get; set; }
        public string Currency { get; set; }
        public string PartnerId { get; set; }
        public decimal Rate { get; set; }

        public decimal BaseAmount => Amount * Rate;

        public decimal BaseBetwin => Betwin * Rate;
    }

    public class UnbalancedBigSmallRoundResultViewModel : BaseRoundResultViewModel
    {
        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }

        public int Total => Dice1 + Dice2 + Dice3;

        public UnbalancedBigSmallSettlementResultViewModel SettlementResult { get; set; }
    }

    public class UnbalancedBigSmallSettlementResultViewModel
    {
        public bool Big { get; set; }
        public bool Small { get; set; }
    }

    public class SlotRNGRoundResultViewModel : BaseRoundResultViewModel
    {
        public IEnumerable<string> Symbols { get; set; }
        public byte ResultNumber { get; set; }
        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class FanTanRoundResultViewModel : BaseRoundResultViewModel
    {
        public FanTanResultViewModel Result { get; set; }

        public FanTanSettlementResultViewModel SettlementResult { get; set; }
    }

    public class FanTanResultViewModel
    {
        public byte TotalBeads { get; set; }
        public byte FinalBeads { get; set; }
    }

    public class FanTanSettlementResultViewModel
    {
        public bool Odd { get; set; }
        public bool Even { get; set; }

        public bool Fan1 { get; set; }
        public bool Fan2 { get; set; }
        public bool Fan3 { get; set; }
        public bool Fan4 { get; set; }

        public bool Kwok12 { get; set; }
        public bool Kwok23 { get; set; }
        public bool Kwok34 { get; set; }
        public bool Kwok41 { get; set; }

        public bool Chun123 { get; set; }
        public bool Chun124 { get; set; }
        public bool Chun134 { get; set; }
        public bool Chun234 { get; set; }
    }
}