using GamesAdmin.Core;
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

        public static string GetMultiplierImageName(int value)
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

        public  MultiplierSymbolViewModel[][] GetFormatBoard()
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
                    var multiplier = string.Empty;
                    if (symbol >= CascadingSlotGameConstant.MinMultiplierSymbol)
                    {
                        multiplier =  $"x{multipliers[multiplierIndex]}";
                        multiplierIndex++;
                    }
                    formatResult[row][column] = new MultiplierSymbolViewModel
                    {
                        Symbol = symbols[row + column * numberOfRows],
                        Multiplier = multiplier
                    };                                
                }
            }

            return formatResult;
        }
    }  
}
