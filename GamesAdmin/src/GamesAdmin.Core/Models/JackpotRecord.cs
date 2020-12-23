using System;

namespace GamesAdmin.Core.Models
{
    public class JackpotRecord
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal Pool { get; set; }

        public long RoundId { get; set; }

        public decimal WinningAmount { get; set; }
    }
}