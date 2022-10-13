using System;
using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Site.Extensions;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BetRecordViewModel : BaseBetViewModel
    {
    }

    public class BigSmallRecordViewModel : BetRecordViewModel
    {
        public decimal Bet { get; set; }
        public decimal Back { get; set; }
        public bool Select { get; set; }

        public decimal BaseBetAmount => ConvertToBase(Bet);

        public decimal BaseBack => ConvertToBase(Back);
    }

    public class OddEvenRecordViewModel : BetRecordViewModel
    {
        public decimal Bet { get; set; }

        public decimal Back { get; set; }

        public bool Select { get; set; }

        public decimal BaseBetAmount => ConvertToBase(Bet);

        public decimal BaseBack => ConvertToBase(Back);
    }

    public class SeDieRecordViewModel : BetRecordViewModel
    {
        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public decimal TwoRed { get; set; }

        public decimal ThreeRed { get; set; }

        public decimal FourRed { get; set; }

        public decimal ThreeWhite { get; set; }

        public decimal FourWhite { get; set; }

        public bool FreeBet { get; set; }
    }

    public class VipSeDieRecordViewModel : BetRecordViewModel
    {
        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public decimal TwoRed { get; set; }

        public decimal ThreeRed { get; set; }

        public decimal FourRed { get; set; }

        public decimal ThreeWhite { get; set; }

        public decimal FourWhite { get; set; }
    }

    public class FishPrawnCrabRecordViewModel : BetRecordViewModel
    {
        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }
    }

    public class DragonTigerRecordViewModel : BetRecordViewModel
    {
        public decimal Dragon { get; set; }

        public decimal Tie { get; set; }

        public decimal Tiger { get; set; }
    }

    public class BaccaratRecordViewModel : BetRecordViewModel
    {
        public decimal Banker { get; set; }

        public decimal BankerPair { get; set; }

        public decimal Player { get; set; }

        public decimal PlayerPair { get; set; }

        public decimal Tie { get; set; }

        public decimal Big { get; set; }

        public decimal Small { get; set; }
    }

    public class RouletteRecordViewModel : BetRecordViewModel
    {
        public decimal TotalBet { get; set; }
    }

    public class BlackjackRecordViewModel : BetRecordViewModel
    {
        public long HandId { get; set; }

        public int ExchangeRateId { get; set; }

        public decimal Bet { get; set; }

        public long? SplitForm { get; set; }

        public long Round { get; set; }

        public IEnumerable<CardViewModel> Cards { get; set; }

        public IEnumerable<string> Actions { get; set; }

        public IEnumerable<CardViewModel> DealerCards { get; set; }

        public bool Ended { get; set; }

        public byte Point { get; set; }

        public byte DealerPoint { get; set; }

        public IEnumerable<byte> SoftPoints { get; set; }
    }

    public class CardViewModel
    {
        public string Rank { get; set; }

        public byte Suit { get; set; }
    }

    public class SpecialSeDieRecordViewModel : BetRecordViewModel
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }

    public class VipFishPrawnCrabRecordViewModel : BetRecordViewModel
    {
        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }

        public decimal AnyTriple { get; set; }

        public decimal Mountain { get; set; }

        public decimal Ocean { get; set; }
    }

    public class SicBoRecordViewModel : BetRecordViewModel
    {
        public decimal TotalBet { get; set; }
    }

    public class VipSeDieWarRecordViewModel : BetRecordViewModel
    {
        public decimal TotalBet { get; set; }
    }

    public class SlotGameRecordViewModel : BetRecordViewModel
    {
        public long RoundId { get; set; }

        public decimal Bet { get; set; }

        public decimal TotalAmount { get; set; }

        public IEnumerable<byte> Paylines { get; set; }

        public bool FreeSpin { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public SlotGameSettlementResultViewModel SettlementResult { get; set; }

        public IEnumerable<SlotGameBonusChestViewModel> BonusChests { get; set; }

        public bool Paid { get; set; }

        public byte[][] GetFormatResults() => Result.ToArray().FormatBoard();

        public decimal TotalBetWin => BetWin + (SettlementResult.Jackpot?.WinningAmount ?? 0);

        public decimal WinLoss => FreeSpin ? TotalBetWin : TotalBetWin - TotalAmount;

        public string Game { get; set; }
        public int ExchangeRateId { get; set; }
        public string IconsFolder => Game?.ToLowerInvariant();
        public string SystemType { get; set; }

        public void ConvertToUSD(bool isConvert, int baseCurrencyMultiplier = 1)
        {
            if (isConvert)
            {
                TotalAmount *= (decimal)Rate / baseCurrencyMultiplier;
                BetWin *= (decimal)Rate / baseCurrencyMultiplier;
                Bet *= (decimal)Rate / baseCurrencyMultiplier;

                if (SettlementResult.Jackpot != null)
                {
                    SettlementResult.Jackpot.WinningAmount *= (decimal)Rate / baseCurrencyMultiplier;
                }
            }
        }
    }

    public class SlotGameBonusChestViewModel
    {
        public decimal Amount { get; set; }

        public bool Picked { get; set; }
    }

    public class SlotGameSettlementResultViewModel
    {
        public IEnumerable<SlotGameWinningLineViewModel> WinningPaylines { get; set; }

        public SlotGameBonusViewModel Bonus { get; set; }

        public SlotGameFreeSpinViewModel FreeSpins { get; set; }

        public SlotGameJackpotViewModel Jackpot { get; set; }
    }

    public class SlotGameJackpotViewModel
    {
        public byte WinningLine { get; set; }

        public decimal WinningAmount { get; set; }
    }

    public class SlotGameWinningLineViewModel
    {
        public byte Payline { get; set; }

        public string Symbol { get; set; }

        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class SlotGameBonusViewModel
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class SlotGameFreeSpinViewModel
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class LotteryRecordViewModel : BetRecordViewModel
    {
        public decimal TotalBet { get; set; }
    }

    public class ThreeCardsRecordViewModel : BetRecordViewModel
    {
        public decimal Red { get; set; }

        public decimal Black { get; set; }

        public decimal Diamonds { get; set; }

        public decimal Hearts { get; set; }

        public decimal Spades { get; set; }

        public decimal Clubs { get; set; }
    }

    public class BitcoinMasterRecordViewModel : BetRecordViewModel
    {
        public decimal Bet { get; set; }
        public decimal Back { get; set; }

        public bool Win { get; set; }
        public string Choice { get; set; }
        public decimal GrossWin => Win ? BetWin + Bet - Back : 0;
    }

    public class KenoMatchRecordViewModel : BetRecordViewModel
    {
        public decimal TotalBet { get; set; }
    }

    public class KenoPowerRecordViewModel : BetRecordViewModel
    {
        public decimal TotalBet { get; set; }
    }

    public class HoldemPokerRecordViewModel : BetRecordViewModel
    {
        public long RoundId { get; set; }

        public string PlayerResult { get; set; }

        public string DealerResult { get; set; }

        public decimal Stake { get; set; }

        public bool Ended { get; set; }

        public string BetHistoryUrl { get; set; }

        public int ExchangeRateId { get; set; }

        public bool IsFold { get; set; }

        public bool? IsSwappedOrHold { get; set; }

        public decimal TotalWin { get; set; }
    }

    public class HiLoRecordViewModel : BetRecordViewModel
    {
        public decimal Amount { get; set; }

        public long RoundId { get; set; }

        public bool Paid { get; set; }

        public int ExchangeRateId { get; set; }
    }

    public class CascadingSlotRecordViewModel : BetRecordViewModel
    {
        public long RoundId { get; set; }

        public int ExchangeRateId { get; set; }

        public decimal TotalAmount { get; set; }

        public bool Paid { get; set; }

        public string Game { get; set; }

        public byte BetLevel { get; set; }

        public decimal CoinValue { get; set; }

        public byte TotalMultiplier { get; set; }

        public byte Multiplier { get; set; }

        public decimal TotalTumbleWin { get; set; }

        public bool FreeSpin { get; set; }

        public string Result { get; set; }

        public decimal WinLoss => FreeSpin ? BetWin : BetWin - TotalAmount;

        public IEnumerable<byte> Board => Result.Split(',').Select(value => byte.Parse(value));

        public bool ReachMaxWin { get; set; }

        public void ConvertToUSD(bool isConvert, int baseCurrencyMultiplier = 1)
        {
            if (!isConvert)
            {
                return;
            }

            TotalAmount *= Rate / baseCurrencyMultiplier;
            BetWin *= Rate / baseCurrencyMultiplier;
            CoinValue *= Rate / baseCurrencyMultiplier;
            TotalTumbleWin *= Rate / baseCurrencyMultiplier;

            if (SettlementResult != null && SettlementResult.Tumbles.Any())
            {
                foreach (var tumble in SettlementResult.Tumbles)
                {
                    tumble.Win *= Rate / baseCurrencyMultiplier;
                    if (tumble.Icons != null)
                    {
                        foreach (var (key, _) in tumble.Icons)
                        {
                            tumble.Icons[key].Win *= Rate / baseCurrencyMultiplier;
                        }
                    }
                }
            }
        }

        public CascadingSlotSettlementResultViewModel SettlementResult { get; set; }
    }

    public class CascadingSlotRoundMultiplierViewModel
    {
        public DateTimeOffset Time { get; set; }

        public long RoundId { get; set; }

        public byte Multiplier { get; set; }

        public byte TotalMultiplier { get; set; }
    }

    public class CascadingSlotSettlementResultViewModel
    {
        public IEnumerable<CascadingSlotTumbleViewModel> Tumbles { get; set; }

        public SlotGameFreeSpinViewModel FreeSpins { get; set; }
    }

    public class CascadingSlotTumbleViewModel
    {
        public Dictionary<byte, CascadingSlotWinningSymbolViewModel> Icons { get; set; }

        public decimal Win { get; set; }

        public string OriginalBoard { get; set; }

        public IEnumerable<byte> Multipliers { get; set; }

        public IEnumerable<byte> Board => OriginalBoard.Split(',').Select(value => byte.Parse(value));
    }

    public class CascadingSlotWinningSymbolViewModel
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }

        public decimal Win { get; set; }
    }

    public class UnbalancedBigSmallRecordViewModel : BetRecordViewModel
    {
        public decimal Big { get; set; }

        public decimal Small { get; set; }
    }

    public class FanTanRecordViewModel : BetRecordViewModel
    {
        public decimal TotalBet { get; set; }
    }

    public class SlotRNGRecordViewModel : BetRecordViewModel
    {
        public decimal TotalAmount { get; set; }
    }
}