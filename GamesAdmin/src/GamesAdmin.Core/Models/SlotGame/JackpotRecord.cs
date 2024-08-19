using System;

namespace GamesAdmin.Core.Models.SlotGame
{
    public class JackpotRecord
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal Pool { get; set; }

        public string Prize { get; set; }

        public long RoundId { get; set; }

        public decimal WinningAmount { get; set; }

        public int ExchangeRateId { get; set; }

        public string Currency { get; set; }

        public string OperatorId { get; set; }

        public decimal Rate { get; set; }
    }
}