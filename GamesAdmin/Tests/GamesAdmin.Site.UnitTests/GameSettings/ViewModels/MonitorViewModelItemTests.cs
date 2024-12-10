
namespace GamesAdmin.Site.UnitTests.GameSettings.ViewModels
{
    using GamesAdmin.Site.Features.GameSettings.ViewModels;
    using Core.Enumeration;
    using Xunit;

    public class MonitorViewModelItemTests
    {
        [Theory]
        [InlineData(0,Game.LuckyWheelValue, true)]
        [InlineData((int)GameId.BigSmall, Game.BigSmallValue,  true)]
        [InlineData((int)GameId.OddEven, Game.OddEvenValue, true)]
        [InlineData((int)GameId.Blackjack, Game.BlackJackValue, true)]
        [InlineData((int)GameId.BitcoinMaster, Game.BitcoinMasterValue, true)]
        [InlineData((int)GameId.CasinoHoldemPoker, Game.CasinoHoldemPokerValue, true)]
        [InlineData((int)GameId.ChineseBlackjack, Game.ChineseBlackjackValue, true)]
        [InlineData((int)GameId.FishPrawnCrab, Game.FishPrawnCrabValue, true)]
        [InlineData((int)GameId.HiLo, Game.HiLoValue, true)]
        [InlineData((int)GameId.Pontoon, Game.PontoonValue, true)]
        [InlineData((int)GameId.SpecialSeDie, Game.SpecialSeDieValue, true)]
        [InlineData((int)GameId.TexasHoldemPoker, Game.TexasHoldemPokerValue, true)]
        [InlineData((int)GameId.BigSmallTurbo, Game.BigSmallTurboValue, true)]
        [InlineData((int)GameId.OddEvenTurbo, Game.OddEvenTurboValue, true)]
        [InlineData((int)GameId.SicBo, Game.SicBoValue, false)]
        [InlineData((int)GameId.Baccarat, Game.BaccaratValue, false)]
        [InlineData((int)GameId.DragonTiger, Game.DragonTigerValue, false)]
        [InlineData((int)GameId.Roulette, Game.RouletteValue, false)]
        public void IsUseHistoryUrl_ReturnExpected(int gameId, string name, bool expected)
        {
            // arrange
            var monitorItemVM = new MonitorItemViewModel
            {
                GameId = gameId,
                Name = name,
            };

            // assert
            Assert.Equal(expected, monitorItemVM.IsUseHistoryUrl);
        }
    }
}
