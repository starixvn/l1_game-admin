using System.Collections.Generic;

namespace GamesAdmin.Core.Models.SlotGame
{
    public class SettlementBoard
    {
        public IEnumerable<byte> result { get; set; }

        public IEnumerable<Payline> paylines { get; set; }

        public Payline bonus { get; set; }

        public Payline freeSpins { get; set; }

        public int jackpotPayline { get; set; }
    }

    public class Payline 
    { 
        public byte payline { get; set; }

        public string symbol { get; set; }

        public byte count { get; set; }

        public double odds { get; set; }
    }
}
