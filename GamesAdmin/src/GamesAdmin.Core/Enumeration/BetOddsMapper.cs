using System.Collections.Generic;
using GamesAdmin.Core.Enumeration.BetChoices;
using GamesAdmin.Core.Enumeration.BetOdds;

namespace GamesAdmin.Core.Enumeration
{
    public static class BetOddsMapper
    {
        public static IEnumerable<Enumeration> Map(Game gameType)
        {
            switch (gameType.Value)
            {
                case Game.BigSmallValue:
                case Game.BigSmallTurboValue:
                case Game.UnbalancedBigSmall1Value:
                case Game.UnbalancedBigSmall2Value:
                case Game.UnbalancedBigSmall3Value:
                case Game.UnbalancedBigSmall4Value:
                    return Enumeration.GetAll<BigSmallBetChoice>();

                case Game.OddEvenValue:
                case Game.OddEvenTurboValue:
                case Game.SpecialSeDieValue:
                    return Enumeration.GetAll<OddEvenBetChoice>();

                case Game.SeDieValue:
                    return Enumeration.GetAll<SeDieBetChoice>();

                case Game.FishPrawnCrabValue:
                    return Enumeration.GetAll<FishPrawnCrabBetOdds>();

                case Game.DragonTigerValue:
                    return Enumeration.GetAll<DragonTigerBetChoice>();

                case Game.KenoProMaxValue:
                case Game.KenoMaxValue:
                case Game.KenoMax2Value:
                case Game.KenoMiniValue:
                case Game.KenoMini2Value:
                case Game.KenoEastValue:
                case Game.KenoWestValue:
                case Game.KenoSouthValue:
                case Game.KenoNorthValue:
                case Game.MegaKenoValue:
                case Game.MegaKeno2Value:
                    return Enumeration.GetAll<KenoBetChoice>();

                case Game.MiniSeDieValue:
                    return Enumeration.GetAll<MiniSeDieBetChoice>();

                case Game.BaccaratValue:
                    return Enumeration.GetAll<BaccaratBetChoice>();

                case Game.RouletteValue:
                    return Enumeration.GetAll<RouletteBetChoice>();

                case Game.VipSeDieValue:
                    return Enumeration.GetAll<VipSeDieBetChoice>();

                case Game.VipFishPrawnCrabValue:
                    return Enumeration.GetAll<VipFishPrawnCrabBetOdds>();

                case Game.BlackJackValue:
                case Game.SoloBlackjackValue:
                    return Enumeration.GetAll<BlackjackBetChoice>();

                case Game.SicBoValue:
                    return Enumeration.GetAll<SicBoBetOdds>();

                case Game.VipBaccaratValue:
                    return Enumeration.GetAll<VipBaccaratBetChoice>();

                case Game.VipSeDieWarValue:
                    return Enumeration.GetAll<VipSeDieWarBetChoice>();

                case Game.LotteryValue:
                    return Enumeration.GetAll<LotteryBetChoice>();

                case Game.ThreeCardsValue:
                    return Enumeration.GetAll<ThreeCardsBetChoice>();

                case Game.BitcoinMasterValue:
                    return Enumeration.GetAll<BitcoinMasterBetChoice>();

                case Game.KenoMatch1Value:
                case Game.KenoMatch2Value:
                    return Enumeration.GetAll<KenoMatchBetOdds>();

                case Game.KenoPower1Value:
                case Game.KenoPower2Value:
                    return Enumeration.GetAll<KenoPowerBetChoice>();

                case Game.HiLoValue:
                    return Enumeration.GetAll<HiLoBetOdds>();

                case Game.TexasHoldemPokerValue:
                    return Enumeration.GetAll<TexasHoldemPokerOdds>();

                case Game.CasinoHoldemPokerValue:
                    return Enumeration.GetAll<CasinoHoldemPokerBetOdds>();

                case Game.BlockchainBaccaratValue:
                    return Enumeration.GetAll<BlockchainBaccaratBetChoice>();

                case Game.MiniSeDie2Value:
                    return Enumeration.GetAll<MiniSeDieBetChoice>();

                case Game.MiniSeDie3Value:
                    return Enumeration.GetAll<MiniSeDieBetChoice>();

                case Game.MiniSeDie4Value:
                    return Enumeration.GetAll<MiniSeDieBetChoice>();

                case Game.FruitSlotValue:
                    return Enumeration.GetAll<FruitSlotOdds>();

                case Game.FanTanValue:
                    return Enumeration.GetAll<FanTanBetChoice>();

                case Game.WinThreeCardsValue:
                    return Enumeration.GetAll<WinThreeCardsBetChoice>();

                case Game.DragonTigerFightValue:
                    return Enumeration.GetAll<DragonTigerFightBetChoice>();

                case Game.ColorGameValue:
                    return Enumeration.GetAll<ColorGameBetOdds>();

                case Game.PontoonValue:
                    return Enumeration.GetAll<PontoonBetChoice>();

                case Game.BandarQiuQiuValue:
                    return Enumeration.GetAll<BandarQiuQiuOdds>();

                case Game.ChineseBlackjackValue:
                    return Enumeration.GetAll<ChineseBlackjackOdds>();

                case Game.FootballDiceValue:
                    return Enumeration.GetAll<FootballDiceBetOdds>();

                case Game.FootballSlotValue:
                    return Enumeration.GetAll<FootballSlotOdds>();

                case Game.Bac81Value:
                case Game.Bac82Value:
                case Game.Bac83Value:
                case Game.Bac84Value:
                case Game.Bac85Value:
                case Game.Bac86Value:
                case Game.Bac87Value:
                case Game.Bac88Value:
                case Game.Bac89Value:
                case Game.Bac90Value:
                case Game.Bac91Value:
                case Game.Bac92Value:
                case Game.Bac97Value:
                case Game.Bac98Value:
                case Game.Bac100Value:
                case Game.Bac101Value:
                    return Enumeration.GetAll<BaccaratXOdds>();

                case Game.SevenUpDownValue:
                    return Enumeration.GetAll<SevenUpDownOdds>();
                default: return default;
            }
        }
    }
}