using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.Report
{
    public class ReportView
    {
        public string RemoteRecordView = "_RemoteRecords";

        private readonly Dictionary<int, string> views = new Dictionary<int, string>
        {
            { (int)GameId.SeDie, "_SeDieRecords" },
            { (int)GameId.MiniSeDie, "_MiniSeDieRecords" },
            { (int)GameId.FishPrawnCrab, "_FishPrawnCrabRecords" },
            { (int)GameId.DragonTiger, "_DragonTigerRecords" },
            { (int)GameId.Roulette, "_RouletteRecords" },
            { (int)GameId.VipSeDie, "_VipSeDieRecords" },
            { (int)GameId.SicBo, "_SicBoRecords" },
            { (int)GameId.VipFishPrawnCrab, "_VipFishPrawnCrabRecords" },
            { (int)GameId.Lottery, "_LotteryRecords" },
            { (int)GameId.ThreeCards, "_ThreeCardsRecords" },
            { (int)GameId.KenoMatch1, "_KenoMatchRecords" },
            { (int)GameId.KenoMatch2, "_KenoMatchRecords" },
            { (int)GameId.KenoPower1, "_KenoPowerRecords" },
            { (int)GameId.KenoPower2, "_KenoPowerRecords" },
            { (int)GameId.KenoProMax, "_KenoBetRecords" },
            { (int)GameId.KenoMax, "_KenoBetRecords" },
            { (int)GameId.KenoMax2, "_KenoBetRecords" },
            { (int)GameId.KenoMini, "_KenoBetRecords" },
            { (int)GameId.KenoMini2, "_KenoBetRecords" },
            { (int)GameId.KenoEast, "_KenoBetRecords" },
            { (int)GameId.KenoWest, "_KenoBetRecords" },
            { (int)GameId.KenoSouth, "_KenoBetRecords" },
            { (int)GameId.KenoNorth, "_KenoBetRecords" },
            { (int)GameId.VipSeDieWar, "_VipSeDieWarRecords" },
            { (int)GameId.Baccarat, "_BaccaratRecords" },
            { (int)GameId.VipBaccarat, "_BaccaratRecords" },
            { (int)GameId.BlockchainBaccarat, "_BaccaratRecords" },
            { (int)GameId.MegaKeno, "_KenoBetRecords" },
            { (int)GameId.MegaKeno2, "_KenoBetRecords" },
            { (int)GameId.MiniSeDie2, "_MiniSeDieRecords" },
            { (int)GameId.MiniSeDie3, "_MiniSeDieRecords" },
            { (int)GameId.MiniSeDie4, "_MiniSeDieRecords" },
            { (int)GameId.UnbalancedBigSmall1, "_UnbalancedBigSmallRecords" },
            { (int)GameId.UnbalancedBigSmall2, "_UnbalancedBigSmallRecords" },
            { (int)GameId.UnbalancedBigSmall3, "_UnbalancedBigSmallRecords" },
            { (int)GameId.UnbalancedBigSmall4, "_UnbalancedBigSmallRecords" },
            { (int)GameId.FruitSlot, "_SlotRNGRecords" },
            { (int)GameId.FanTan, "_FanTanRecords" },
            { (int)GameId.WinThreeCards, "_WinThreeCardsRecords" },
            { (int)GameId.DragonTigerFight, "_DragonTigerFightRecords" },
            { (int)GameId.ColorGame, "_ColorGameRecords" },
            { (int)GameId.BandarQiuQiu, "_BandarQiuQiuRecords" },
            { (int)GameId.BacBall1, "_BacBall1Records" },
            { (int)GameId.BacBall2, "_BacBall2Records" },
            { (int)GameId.LuckyNext1, "_NextBallRecords" },
            { (int)GameId.LuckyNext2, "_NextBallRecords" },
            { (int)GameId.FootballDice, "_FootballDiceRecords" },
            { (int)GameId.FootballSlot, "_SlotRNGRecords" },
        };

        public string GetView(int game)
        {
            return views.ContainsKey(game) ? views[game] : RemoteRecordView;
        }
    }
}