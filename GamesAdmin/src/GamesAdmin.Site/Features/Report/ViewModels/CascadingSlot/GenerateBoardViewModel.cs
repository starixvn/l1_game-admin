using GamesAdmin.Core;
using GamesAdmin.Core.Enumeration;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{

    public class GenerateBoardViewModel
    {
        public GenerateBoardViewModel(CascadingSlotTumbleViewModel tumble, string symbolUrl)
        {
            Tumble = tumble;
            SymbolUrl = symbolUrl;
        }

        public CascadingSlotTumbleViewModel Tumble { get; }

        public string SymbolUrl { get; }

        public string GetMultiplierSymbolCascadingSlot(int value)
        {
            if(IsTreasureIslandGame())
            {
                return GetMultiplierSymbolTreasureIsland(value);
            }
            return GetMultiplierSymbol(value);
        }

        private static string GetMultiplierSymbol(int value)
        {
            if (value <= CascadingSlotGameConstant.MinMultiplierSymbol)
            {
                return "10";
            }
            else
            {
                if (value <= CascadingSlotGameConstant.Multiplier10xSymbol)
                {
                    return "11";
                }

                if (value <= CascadingSlotGameConstant.Multiplier15xSymbol)
                {
                    return "12";
                }

            }
            return "13";
        }

        private static string GetMultiplierSymbolTreasureIsland(int value)
        {
            if (value <= TreasureIslandGameConstant.MinMultiplierSymbol)
            {
                return "13";
            }
            else
            {
                if (value <= TreasureIslandGameConstant.Multiplier20xSymbol)
                {
                    return "11";
                }

                if (value <= TreasureIslandGameConstant.Multiplier30xSymbol)
                {
                    return "12";
                }

            }
            return "10";
        }

        private bool IsTreasureIslandGame()
        {
            var parts = SymbolUrl.Split("/").Reverse().ToList();
            var gameName = parts[1].Trim();
            return Game.TreasureIslandValue == gameName;
        }

        public MultiplierSymbolViewModel[][] GetFormatBoard()
        {
            const byte numberOfRows = CascadingSlotGameConstant.NumberOfRows;
            const byte numberOfCols = CascadingSlotGameConstant.NumberOfColumns;
            MultiplierSymbolViewModel[][] formatResult = new MultiplierSymbolViewModel[numberOfRows][];
            var symbols = Tumble.Board.ToArray();
            var multipliers = Tumble.Multipliers.ToList();
            var multiplierIndex = 0;
            for (var row = 0; row < numberOfRows; row++)
            {
                formatResult[row] = new MultiplierSymbolViewModel[numberOfCols];
                for (var column = 0; column < numberOfCols; column++)
                {
                    var symbol = symbols[row + column * numberOfRows];
                    var xMultiplier = string.Empty;
                    var multiplier = 0;
                    if (symbol >= CascadingSlotGameConstant.MinMultiplierValue)
                    {
                        multiplier = multipliers[multiplierIndex];
                        xMultiplier = $"x{multiplier}";
                        multiplierIndex++;
                    }
                    formatResult[row][column] = new MultiplierSymbolViewModel
                    {
                        Symbol = symbols[row + column * numberOfRows],
                        XMultiplier = xMultiplier,
                        Multiplier = multiplier
                    };
                }
            }

            return formatResult;
        }
    }
}
