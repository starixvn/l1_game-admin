using GamesAdmin.Site.Features.Report.ViewModels;
using System.Collections.Generic;
using Xunit;

namespace GamesAdmin.Site.UnitTests.Report.ViewModels
{
    public class GenerateBoardViewModelTests
    {
        [Theory]
        [InlineData(40, "10")]
        [InlineData(16, "11")]
        [InlineData(25, "12")]
        [InlineData(5, "13")]
        public void GetMultiplierImageName_Always_Return_ImageName(byte multiplier, string expected)
        {
            // act
            var actual = GenerateBoardViewModel.GetMultiplierImageName(multiplier);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetFormatBoard_ReturnExpected()
        {
            var multiplier = new List<byte> { 25, 10, 40 };
            var tumble = new CascadingSlotTumbleViewModel
            {
                Multipliers = multiplier,
                OriginalBoard = "3,0,2,6,6,2,10,1,4,4,10,1,5,2,9,5,5,3,1,1,25,3,7,4,3,8,2,2,4,4"
            };
            var expected = new MultiplierSymbolViewModel[5][];
            expected[0] = new MultiplierSymbolViewModel[6] 
            {
                GetMultiplierSymbol(3),
                GetMultiplierSymbol(2),
                GetMultiplierSymbol(10, "x25"),
                GetMultiplierSymbol(5),
                GetMultiplierSymbol(25, "x10"),
                GetMultiplierSymbol(8) 
            };
            expected[1] = new MultiplierSymbolViewModel[6]
            {
                GetMultiplierSymbol(0),
                GetMultiplierSymbol(10,"x40"),
                GetMultiplierSymbol(1),
                GetMultiplierSymbol(5),
                GetMultiplierSymbol(3),
                GetMultiplierSymbol(2)
            };
            expected[2] = new MultiplierSymbolViewModel[6]   {
                GetMultiplierSymbol(2),
                GetMultiplierSymbol(1),
                GetMultiplierSymbol(5),
                GetMultiplierSymbol(3),
                GetMultiplierSymbol(7),
                GetMultiplierSymbol(2)
            };
            expected[3] = new MultiplierSymbolViewModel[6]   {
                GetMultiplierSymbol(6),
                GetMultiplierSymbol(4),
                GetMultiplierSymbol(2),
                GetMultiplierSymbol(1),
                GetMultiplierSymbol(4),
                GetMultiplierSymbol(4)
            };
            expected[4] = new MultiplierSymbolViewModel[6]   {
                GetMultiplierSymbol(6),
                GetMultiplierSymbol(4),
                GetMultiplierSymbol(9),
                GetMultiplierSymbol(1),
                GetMultiplierSymbol(3),
                GetMultiplierSymbol(4)
            };

            // act 
            var viewModel = new GenerateBoardViewModel(tumble, "");
            var actual = viewModel.GetFormatBoard();

            // assert

            Assert.Equal(expected.ToString(), actual.ToString());
        }


        private static MultiplierSymbolViewModel GetMultiplierSymbol(byte symbol, string multiplier="")
        {
            return new MultiplierSymbolViewModel
            {
                Symbol = symbol,
                Multiplier = multiplier
            };
        }
    }
}
