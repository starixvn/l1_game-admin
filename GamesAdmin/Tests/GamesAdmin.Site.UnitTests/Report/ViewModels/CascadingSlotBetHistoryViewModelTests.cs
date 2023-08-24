namespace GamesAdmin.Site.UnitTests.Report.ViewModels
{
    using System.Collections.Generic;
    using GamesAdmin.Site.Features.Report.ViewModels;
    using Xunit;

    public class CascadingSlotBetHistoryViewModelTests
    {
        [Theory]
        [InlineData(true, "(You reached the Max Win) Win")]
        [InlineData(false, "Win")]
        public void WinLabel_ReturnExpected(bool isReachMax, string expected)
        {
            // arrange
            var round = new CascadingSlotRecordViewModel
            {
                ReachMaxWin = isReachMax,
            };
            var viewModel = new CascadingSlotBetHistoryViewModel
            {
                Round = round,
            };

            // assert
            Assert.Equal(expected, viewModel.WinLabel);
        }

        [Theory]
        [InlineData(0, 20, false)]
        [InlineData(10, 0, false)]
        [InlineData(0, 0, false)]
        [InlineData(10, 10, true)]
        public void HasTotalMultiplier_ReturnExpected(decimal betwin, int multiplier, bool expected)
        {
            // arrange
            var round = new CascadingSlotRecordViewModel
            {
                BetWin = betwin,
                Multiplier = multiplier,
            };
            var viewModel = new CascadingSlotBetHistoryViewModel
            {
                Round = round,
            };

            // assert
            Assert.Equal(expected, viewModel.HasTotalMultiplier);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void IsMultipliers_ReturnExpected(bool anyMultipliers, bool expected)
        {
            // arrange
            var viewModel = new CascadingSlotBetHistoryViewModel
            {
                Multipliers = anyMultipliers ? GetMultipliers() : null,
            };

            // assert
            Assert.Equal(expected, viewModel.IsMultipliers);
        }

        private static IEnumerable<CascadingSlotRoundMultiplierViewModel> GetMultipliers()
        {
            var multipliers = new List<CascadingSlotRoundMultiplierViewModel>
            {
                new CascadingSlotRoundMultiplierViewModel
                {
                    RoundId = 1,
                    Multiplier = 20,
                    TotalMultiplier = 30
                }
            };
            return multipliers;
        }

        [Fact]
        public void SymbolUrl_ReturnExpected()
        {
            // arrange

            var viewModel = new CascadingSlotBetHistoryViewModel
            {
                Game = "Cascading-slot",
                StaticResourceUrl = "resource"
            };
            var expected = "resource/cascading-slot/symbol";

            // assert
            Assert.Equal(expected, viewModel.SymbolUrl);
        }
    }
}
