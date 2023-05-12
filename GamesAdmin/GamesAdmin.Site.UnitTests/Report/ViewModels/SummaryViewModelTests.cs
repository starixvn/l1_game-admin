using GamesAdmin.Site.Features.Report.ViewModels;
using Xunit;

namespace GamesAdmin.Site.UnitTests.Report.ViewModels
{
    public class SummaryViewModelTests
    {
        [Theory]
        [InlineData(true, "Free Spins")]
        [InlineData(false, "")]
        public void FreeSpinsLabel_ReturnExpected(bool isFreeSpin, string expected)
        {
            // arrange
            var round = new CascadingSlotRecordViewModel
            {
                FreeSpin = isFreeSpin,
            };
            var viewModel = new SummaryViewModel(round);

            // assert
            Assert.Equal(expected, viewModel.FreeSpinsLabel);
        }


        [Theory]
        [InlineData(true, 20, 0)]
        [InlineData(false, 20, 20)]
        public void TotalAmount_ReturnExpected(bool isFreeSpin, decimal totalAmount, decimal expected)
        {
            // arrange
            var round = new CascadingSlotRecordViewModel
            {
                FreeSpin = isFreeSpin,
                TotalAmount = totalAmount,
            };
            var viewModel = new SummaryViewModel(round);

            // assert
            Assert.Equal(expected, viewModel.TotalAmount);
        }

        [Theory]
        [InlineData(true, "50.00")]
        [InlineData(false, "30.00")]
        public void DisplayWinLoss_ReturnExpected(bool isFreeSpin, string expected)
        {
            // arrange
            var round = new CascadingSlotRecordViewModel
            {
               FreeSpin = isFreeSpin,
               BetWin = 50,
               TotalAmount = 20
            };
            var viewModel = new SummaryViewModel(round);

            // assert
            Assert.Equal(expected, viewModel.DisplayWinLoss);
        }
    }
}
