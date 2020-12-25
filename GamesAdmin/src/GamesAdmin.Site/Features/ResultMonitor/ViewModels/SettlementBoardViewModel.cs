using System.Linq;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Site.Extensions;

namespace GamesAdmin.Site.Features.ResultMonitor.ViewModels
{
    public class SettlementBoardViewModel
    {
        public SettlementBoardViewModel(string name, SettlementBoard board)
        {
            Name = name;
            Board = board;
        }

        public string Name { get; }

        public SettlementBoard Board { get; }

        public byte[,] FormattedBoard => Board.result.ToArray().FormatBoard();

        public string WinningPaylines => string.Join(", ", Board.paylines.Where(x => x.odds > 0).Select(x => x.payline));

        public byte NumberOfFreeSpins => (byte)Board.freeSpins.odds;

        public byte NumberOfBonus => (byte)Board.bonus.odds;

        public string Jackpot => Board.jackpotPayline > -1  ? "YES" : "NO";
    }
}