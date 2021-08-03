using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class HeroesSlotBetRecord : BetRecord
    {
        public long RoundId { get; set; }

        public decimal Bet { get; set; }

        public decimal TotalAmount { get; set; }

        public IEnumerable<byte> Paylines { get; set; }

        public bool FreeSpin { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public HeroesSlotSettlementResult SettlementResult { get; set; }

        public IEnumerable<HeroesSlotBonusChest> BonusChests { get; set; }

        public bool Paid { get; set; }
    }

    public class HeroesSlotBonusChest
    {
        public decimal Amount { get; set; }

        public bool Picked { get; set; }
    }

    public class HeroesSlotSettlementResult
    {
        public IEnumerable<HeroesSlotWinningLine> WinningPaylines { get; set; }

        public HeroesSlotBonus Bonus { get; set; }

        public HeroesSlotFreeSpin FreeSpins { get; set; }

        public HeroesSlotJackpot Jackpot { get; set; }
    }

    public class HeroesSlotJackpot
    {
        public byte WinningLine { get; set; }

        public decimal WinningAmount { get; set; }
    }

    public class HeroesSlotWinningLine
    {
        public byte Payline { get; set; }

        public string Symbol { get; set; }

        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class HeroesSlotBonus
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class HeroesSlotFreeSpin
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }
}