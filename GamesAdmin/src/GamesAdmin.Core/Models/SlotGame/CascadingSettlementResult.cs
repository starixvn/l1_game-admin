using System.Collections.Generic;

namespace GamesAdmin.Core.Models.SlotGame
{
    public class CascadingSettlementResult
    {
        public IEnumerable<CascadingBoard> Boards { get; set; }
    }

    public class CascadingBoard
    {
        public string Name { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public Dictionary<byte, int> WinningSymbols { get; set; }

        public double Multiplier { get; set; }

        public int Scatter { get; set; }
    }
}
