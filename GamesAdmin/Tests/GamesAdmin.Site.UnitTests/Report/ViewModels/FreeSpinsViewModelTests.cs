namespace GamesAdmin.Site.UnitTests.Report.ViewModels
{
    using GamesAdmin.Site.Features.Report.ViewModels;
    using Xunit;

    public class FreeSpinsViewModelTests
    {
        [Theory]
        [InlineData(true, "Extra Free Spins")]
        [InlineData(false, "Free Spins")]
        public void FreeSpinsText_ReturnExpected(bool hasFreeSpins, string expected)
        {
            // arrange
            var viewModel = new FreeSpinsViewModel(null, null, 1, 1, hasFreeSpins, "");

            // assert
            Assert.Equal(expected, viewModel.FreeSpinsText);
        }

        [Theory]
        [InlineData(10, true)]
        [InlineData(0, false)]
        public void HasOdds_ReturnExpected(decimal odds, bool expected)
        {
            // arrange
            var freeSpins = new SlotGameFreeSpinViewModel
            {
                Odds = odds
            };
            var viewModel = new FreeSpinsViewModel(null, freeSpins, 1, 1, true, "");

            // assert
            Assert.Equal(expected, viewModel.HasOdds);
        }
    }
}
