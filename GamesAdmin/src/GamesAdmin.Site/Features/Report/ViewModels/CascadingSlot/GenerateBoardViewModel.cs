using GamesAdmin.Core;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class GenerateBoardViewModel
    {
        public CascadingSlotTumbleViewModel Tumble { get; set; }

        public IEnumerable<byte> Multipliers => Tumble.Multipliers;

        public byte[][] Board => RenderedBoard(Tumble.Board);

        public string GameName  {get;set;}

        public static byte[][] RenderedBoard(IEnumerable<byte> board)
        {
            var boardArray = board.ToArray();
            byte[][] result = new byte[CascadingSlotGameConstant.NumberOfRows][];         
            for (var row = 0; row < result.Length; row++)
            {
                result[row] = new byte[CascadingSlotGameConstant.NumberOfColumns];
                for (var column = 0; column < CascadingSlotGameConstant.NumberOfColumns; column++)
                {
                    result[row][column] = boardArray[row + column * CascadingSlotGameConstant.NumberOfRows];
                }
            }

            return result;
        }

        public GenerateBoardViewModel(CascadingSlotTumbleViewModel tumble, string gameName)
        {
            Tumble = tumble;
            GameName = gameName;
        }

        public static string GetMultiplierImageName(byte value)
        {
            if (value <= CascadingSlotGameConstant.MinMultiplierSymbol)
            {
                return "13";
            }
            else
            {
                if (value <= CascadingSlotGameConstant.Multiplier20xSymbol)
                {
                    return "11";
                }

                if (value <= CascadingSlotGameConstant.Multiplier30xSymbol)
                {
                    return "12";
                }

            }
            return "10";
        }

    }
}
