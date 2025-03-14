using System;

namespace GamesAdmin.Core.Models.BetReport
{

    public class CrashGameBetHistory
    {
        public decimal WinAmount { get; set; }

        public decimal Multiplier { get; set; }

        public string Action { get; set; }
        
        public string ClientSeed { get; set; }

        public decimal Amount { get; set; }
        
        public DateTime Time { get; set; }
    }
}
