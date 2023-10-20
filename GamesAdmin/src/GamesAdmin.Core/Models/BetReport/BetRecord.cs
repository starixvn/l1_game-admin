using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public abstract class BetRecord
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal BetWin { get; set; }

        public DateTime Time { get; set; }

        public GameAccount Account { get; set; }

        public string Currency { get; set; }

        public string PartnerId { get; set; }

        public decimal Rate { get; set; }
    }

    public class BigSmallBetRecord : BetRecord
    {
        public bool Select { get; set; }

        public decimal Bet { get; set; }

        public decimal Back { get; set; }
    }

    public class OddEvenBetRecord : BetRecord
    {
        public bool Select { get; set; }

        public decimal Bet { get; set; }

        public decimal Back { get; set; }
    }

    public class SeDieBetRecord : BetRecord
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

    public class VipSeDieBetRecord : BetRecord
    {
        public decimal Odd { get; set; }

        public decimal Even { get; set; }

        public decimal TwoRed { get; set; }

        public decimal ThreeRed { get; set; }

        public decimal FourRed { get; set; }

        public decimal ThreeWhite { get; set; }

        public decimal FourWhite { get; set; }
    }

    public class FishPrawnCrabBetRecord : BetRecord
    {
        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }
    }

    public class DragonTigerBetRecord : BetRecord
    {
        public decimal Dragon { get; set; }

        public decimal Tie { get; set; }

        public decimal Tiger { get; set; }
    }

    public class BaccaratBetRecord : BetRecord
    {
        public decimal Banker { get; set; }

        public decimal BankerPair { get; set; }

        public decimal Player { get; set; }

        public decimal PlayerPair { get; set; }

        public decimal Tie { get; set; }

        public decimal Big { get; set; }

        public decimal Small { get; set; }
    }

    public class RouletteBetRecord : BetRecord
    {
        public decimal TotalBet { get; set; }
    }

    public class BlackjackBetRecord : BetRecord
    {
        public long HandId { get; set; }

        public int ExchangeRateId { get; set; }

        public decimal Bet { get; set; }

        public byte Point { get; set; }

        public long? SplitForm { get; set; }

        public IEnumerable<Card> Cards { get; set; }

        public IEnumerable<string> Actions { get; set; }

        public IEnumerable<Card> DealerCards { get; set; }

        public bool Ended { get; set; }

        public byte DealerPoint { get; set; }

        public IEnumerable<byte> SoftPoints { get; set; }
    }

    public class Card
    {
        public string Rank { get; set; }

        public byte Suit { get; set; }
    }

    public class SpecialSeDieBetRecord : BetRecord
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public decimal Refund { get; set; }
    }

    public class VipFishPrawnCrabBetRecord : BetRecord
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

    public class ColorGameBetRecord : BetRecord
    {
        public decimal Yellow { get; set; }

        public decimal Red { get; set; }

        public decimal Pink { get; set; }

        public decimal White { get; set; }

        public decimal Blue { get; set; }

        public decimal Green { get; set; }

        public decimal Hot { get; set; }

        public decimal Cold { get; set; }

        public double TotalAmount { get; set; }
    }

    public class VipSeDieWarBetRecord : BetRecord
    {
        public decimal TotalBet { get; set; }
    }

    public class SicBoBetRecord : BetRecord
    {
        public decimal TotalBet { get; set; }
    }

    public class SlotGameBetRecord : BetRecord
    {
        public long RoundId { get; set; }

        public decimal Bet { get; set; }

        public decimal TotalAmount { get; set; }

        public IEnumerable<byte> Paylines { get; set; }

        public bool FreeSpin { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public SlotGameSettlementResult SettlementResult { get; set; }

        public IEnumerable<SlotGameBonusChest> BonusChests { get; set; }

        public bool Paid { get; set; }

        public string Partner { get; set; }

        public int ExchangRateId { get; set; }

        public SlotJackpotContribution JackpotContribution { get; set; }
    }

    public class SlotJackpotContribution
    {
        public double ContributionRate { get; set; }

        public double BaseContributionAmount { get; set; }
    }

    public class SlotGameBonusChest
    {
        public decimal Amount { get; set; }

        public bool Picked { get; set; }
    }

    public class SlotGameSettlementResult
    {
        public IEnumerable<SlotGameWinningLine> WinningPaylines { get; set; }

        public SlotGameBonus Bonus { get; set; }

        public SlotGameFreeSpin FreeSpins { get; set; }

        public SlotGameJackpot Jackpot { get; set; }
    }

    public class SlotGameJackpot
    {
        public byte WinningLine { get; set; }

        public decimal WinningAmount { get; set; }
    }

    public class SlotGameWinningLine
    {
        public byte Payline { get; set; }

        public string Symbol { get; set; }

        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class SlotGameBonus
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class SlotGameFreeSpin
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class LotteryBetRecord : BetRecord
    {
        public decimal TotalBet { get; set; }
    }

    public class ThreeCardsBetRecord : BetRecord
    {
        public decimal Red { get; set; }

        public decimal Black { get; set; }

        public decimal Diamonds { get; set; }

        public decimal Hearts { get; set; }

        public decimal Spades { get; set; }

        public decimal Clubs { get; set; }
    }

    public class BitcoinMasterBetRecord : BetRecord
    {
        public decimal Bet { get; set; }

        public decimal Back { get; set; }

        public bool Win { get; set; }

        public string Choice { get; set; }
    }

    public class KenoMatchBetRecord : BetRecord
    {
        public decimal TotalBet { get; set; }
        public decimal Refund { get; set; }
    }

    public class KenoPowerBetRecord : BetRecord
    {
        public decimal Refund { get; set; }
        public decimal TotalBet { get; set; }
    }

    public class HoldemPokerBetRecord : BetRecord
    {
        public long RoundId { get; set; }

        public string PlayerResult { get; set; }

        public string DealerResult { get; set; }

        public decimal Stake { get; set; }

        public bool Ended { get; set; }

        public bool IsFold { get; set; }

        public bool? IsQualified { get; set; }

        public bool? IsSwappedOrHold { get; set; }

        public int ExchangeRateId { get; set; }

        public decimal TotalWin { get; set; }
    }

    public class HiLoBetRecord : BetRecord
    {
        public decimal Amount { get; set; }

        public long RoundId { get; set; }

        public bool Paid { get; set; }

        public int ExchangeRateId { get; set; }
    }

    public class CascadingSlotBetRecord : BetRecord
    {
        public long RoundId { get; set; }

        public decimal Bet { get; set; }

        public decimal TotalAmount { get; set; }

        public byte BetLevel { get; set; }

        public decimal CoinValue { get; set; }

        public double BetMultiplier { get; set; }

        public bool FreeSpin { get; set; }

        public string Result { get; set; }

        public int TotalMultiplier { get; set; }

        public int Multiplier { get; set; }

        public decimal TotalTumbleWin { get; set; }

        public bool Paid { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public int? MemberId { get; set; }

        public string SiteId { get; set; }

        public int PlayerId { get; set; }

        public int ExchangeRateId { get; set; }

        public CascadingSlotSettlementResult SettlementResult { get; set; }

        public bool ReachMaxWin { get; set; }
    }

    public class CascadingSlotSettlementResult
    {
        public IEnumerable<CascadingSlotTumble> Tumbles { get; set; }

        public SlotGameFreeSpin FreeSpins { get; set; }

        public SlotGameFreeSpin AdditionalFreeSpins { get; set; }
    }

    public class CascadingSlotTumble
    {
        public Dictionary<string, CascadingSlotWinningSymbol> Icons { get; set; }

        public decimal Win { get; set; }

        public string OriginalBoard { get; set; }

        public IEnumerable<int> Multipliers { get; set; }
    }

    public class CascadingSlotWinningSymbol
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }

        public decimal Win { get; set; }
    }

    public class CascadingSlotTotalMultiplier
    {
        public int PlayerId { get; set; }

        public bool Ended { get; set; }

        public IEnumerable<CascadingSlotRoundMultiplier> Rounds { get; set; }
    }

    public class CascadingSlotRoundMultiplier
    {
        public DateTime Time { get; set; }

        public long RoundId { get; set; }

        public int TotalMultiplier { get; set; }

        public int Multiplier { get; set; }
    }

    public class UnbalancedBigSmallBetRecord : BetRecord
    {
        public decimal Big { get; set; }
        public decimal Small { get; set; }
    }

    public class SlotRNGBetRecord : BetRecord
    {
        public decimal TotalAmount { get; set; }
    }

    public class FanTanBetRecord : BetRecord
    {
        public decimal TotalBet { get; set; }
    }

    public class WinThreeCardsBetRecord : BetRecord
    {
        public decimal Dragon { get; set; }
        public decimal Phoenix { get; set; }
        public decimal Pair8Plus { get; set; }
        public decimal Straight { get; set; }
        public decimal Flush { get; set; }
        public decimal StraightFlush { get; set; }
        public decimal ThreeKind { get; set; }
        public decimal TotalBet { get; set; }
    }

    public class DragonTigerFightBetRecord : DragonTigerBetRecord
    {
        public decimal BlackDragon { get; set; }
        public decimal RedDragon { get; set; }
        public decimal BlackTiger { get; set; }
        public decimal RedTiger { get; set; }
        public double TotalAmount { get; set; }
    }

    public class BandarQiuQiuBetRecord : BetRecord
    {
        public decimal Spade { get; set; }

        public decimal Heart { get; set; }

        public decimal Club { get; set; }

        public decimal Diamond { get; set; }

        public double TotalAmount { get; set; }
    }
}