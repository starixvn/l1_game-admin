using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class VietnameseHeroesBetRecord : BetRecord
    {
        public long RoundId { get; set; }

        public decimal Bet { get; set; }

        public decimal TotalAmount { get; set; }

        public IEnumerable<byte> Paylines { get; set; }

        public bool FreeSpin { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public VietnameseHeroesSettlementResult SettlementResult { get; set; }

        public IEnumerable<VietnameseHeroesBonusChest> BonusChests { get; set; }

        public bool Paid { get; set; }
    }

    public class VietnameseHeroesBonusChest
    {
        public decimal Amount { get; set; }

        public bool Picked { get; set; }
    }

    public class VietnameseHeroesSettlementResult
    {
        public IEnumerable<VietnameseHeroesWinningLine> WinningPaylines { get; set; }

        public VietnameseHeroesBonus Bonus { get; set; }

        public VietnameseHeroesFreeSpin FreeSpins { get; set; }

        public VietnameseHeroesJackpot Jackpot { get; set; }
    }

    public class VietnameseHeroesJackpot
    {
        public byte WinningLine { get; set; }

        public decimal WinningAmount { get; set; }
    }

    public class VietnameseHeroesWinningLine
    {
        public byte Payline { get; set; }

        public string Symbol { get; set; }

        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class VietnameseHeroesBonus
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }

    public class VietnameseHeroesFreeSpin
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }
    }
}