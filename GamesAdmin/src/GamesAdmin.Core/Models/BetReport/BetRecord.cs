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

        public decimal Bet { get; set; }

        public byte Point { get; set; }

        public long? SplitForm { get; set; }

        public IEnumerable<BlackjackCard> Cards { get; set; }

        public IEnumerable<string> Actions { get; set; }

        public IEnumerable<BlackjackCard> DealerCards { get; set; }

        public bool Ended { get; set; }

        public byte DealerPoint { get; set; }

        public IEnumerable<byte> SoftPoints { get; set; }
    }

    public class BlackjackCard
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

    public class FourGuardiansBetRecord : BetRecord
    {
        public long RoundId { get; set; }

        public decimal Bet { get; set; }

        public decimal TotalAmount { get; set; }

        public IEnumerable<byte> Paylines { get; set; }

        public bool FreeSpin { get; set; }

        public IEnumerable<byte> Results { get; set; }

        public FourGuardiansSettlementResult SettlementResult { get; set; }

        public IEnumerable<FourGuardiansBonusChest> BonusChests { get; set; }

        public bool Paid { get; set; }
    }

    public class FourGuardiansBonusChest
    {
        public decimal Amount { get; set; }

        public bool Picked { get; set; }
    }

    public class FourGuardiansSettlementResult
    {
        public IEnumerable<FourGuardiansWinningLine> WinningPaylines { get; set; }

        public FourGuardiansBonus Bonus { get; set; }

        public FourGuardiansFreeSpin FreeSpins { get; set; }

        public FourGuardiansJackpot Jackpot { get; set; }
    }

    public class FourGuardiansJackpot
    {
        public byte WinningLine { get; set; }

        public decimal WinningAmount { get; set; }
    }

    public class FourGuardiansWinningLine
    {
        public byte Payline { get; set; }

        public string Symbol { get; set; }

        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class FourGuardiansBonus
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class FourGuardiansFreeSpin
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }
}