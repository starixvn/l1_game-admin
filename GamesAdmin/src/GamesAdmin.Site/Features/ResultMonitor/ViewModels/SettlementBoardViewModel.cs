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

        public bool IsError => Board == null;

        public byte[,] FormattedBoard => Board?.Result.ToArray().FormatBoard();

        public string WinningPaylines => string.Join(", ", Board?.Paylines.Where(x => x.Odds > 0).Select(x => x.Payline));

        public byte NumberOfFreeSpins => (byte)(IsError ? 0 : Board.FreeSpins.Odds);

        public byte NumberOfBonus => (byte)(IsError ? 0 : Board.Bonus.Odds);

        public string Jackpot => Board?.JackpotPayline > -1 ? "YES" : "NO";
    }
}