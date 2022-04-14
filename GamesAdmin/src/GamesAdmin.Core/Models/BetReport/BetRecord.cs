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
}