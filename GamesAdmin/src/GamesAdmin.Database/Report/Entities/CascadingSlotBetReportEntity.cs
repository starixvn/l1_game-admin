using System.Collections.Generic;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Database.Report.Entities
{
    public class CascadingSlotBetReportEntity : SlotGameRoundEntity
    {
        public string Username { get; set; }

        public string Nickname { get; set; }

        public string UserId { get; set; }

        //public double BetLevel { get; set; }

        //public double CoinValue { get; set; }

        //public byte[] BaseSpin { get; set; }

        //public IEnumerable<byte[]> Tumblings { get; set; }

        //public List<Dictionary<byte, int>> WinningSymbols { get; set; }

        //public List<Dictionary<byte, double>> WinningAmounts { get; set; }

        //public List<double> TumblingWins { get; set; }

        //public List<int> Multipliers { get; set; }
    }
}