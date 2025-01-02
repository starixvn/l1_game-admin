using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.Report
{
    public class ReportView
    {
        public string RemoteRecordView = "_RemoteRecords";

        private readonly Dictionary<GameId, string> views = new Dictionary<GameId, string>
        {
            { GameId.SeDie, "_SeDieRecords" },
            { GameId.MiniSeDie, "_MiniSeDieRecords" },
            { GameId.FishPrawnCrab, "_FishPrawnCrabRecords" },
            { GameId.DragonTiger, "_DragonTigerRecords" },
            { GameId.Roulette, "_RouletteRecords" },
            { GameId.VipSeDie, "_VipSeDieRecords" },
            { GameId.SicBo, "_SicBoRecords" },
            { GameId.VipFishPrawnCrab, "_VipFishPrawnCrabRecords" },
            { GameId.Lottery, "_LotteryRecords" },
            { GameId.ThreeCards, "_ThreeCardsRecords" },
            { GameId.KenoProMax, "_KenoBetRecords" },
            { GameId.KenoMax, "_KenoBetRecords" },
            { GameId.KenoMax2, "_KenoBetRecords" },
            { GameId.VipSeDieWar, "_VipSeDieWarRecords" },
            { GameId.Baccarat, "_BaccaratRecords" },
            { GameId.VipBaccarat, "_BaccaratRecords" },
            { GameId.BlockchainBaccarat, "_BaccaratRecords" },
            { GameId.MiniSeDie2, "_MiniSeDieRecords" },
            { GameId.MiniSeDie3, "_MiniSeDieRecords" },
            { GameId.MiniSeDie4, "_MiniSeDieRecords" },
            { GameId.UnbalancedBigSmall1, "_UnbalancedBigSmallRecords" },
            { GameId.UnbalancedBigSmall2, "_UnbalancedBigSmallRecords" },
            { GameId.UnbalancedBigSmall3, "_UnbalancedBigSmallRecords" },
            { GameId.UnbalancedBigSmall4, "_UnbalancedBigSmallRecords" },
            { GameId.FruitSlot, "_SlotRNGRecords" },
            { GameId.FanTan, "_FanTanRecords" },
            { GameId.WinThreeCards, "_WinThreeCardsRecords" },
            { GameId.DragonTigerFight, "_DragonTigerFightRecords" },
            { GameId.ColorGame, "_ColorGameRecords" },
            { GameId.BandarQiuQiu, "_BandarQiuQiuRecords" },
            { GameId.FootballDice, "_FootballDiceRecords" },
            { GameId.FootballSlot, "_SlotRNGRecords" },
            //xD
            { GameId.BacBall1, "_BacBall1Records" },
            { GameId.BacBall2, "_BacBall2Records" },
            { GameId.LuckyNext1, "_NextBallRecords" },
            { GameId.LuckyNext2, "_NextBallRecords" },
            { GameId.BaccaratNumber, "_BacBall1Records" },
            { GameId.SpeedBaccaratNumber, "_BacBall1Records" },
            { GameId.VIPBaccaratNumber, "_BacBall1Records" },
            //SK
            { GameId.MegaKeno, "_KenoBetRecords" },
            { GameId.MegaKeno2, "_KenoBetRecords" },
            { GameId.KenoMini, "_KenoBetRecords" },
            { GameId.KenoMini2, "_KenoBetRecords" },
            { GameId.KenoEast, "_KenoBetRecords" },
            { GameId.KenoWest, "_KenoBetRecords" },
            { GameId.KenoSouth, "_KenoBetRecords" },
            { GameId.KenoNorth, "_KenoBetRecords" },
            { GameId.KenoSoccer, "_KenoMatchRecords" },
            { GameId.KenoSoccer2, "_KenoMatchRecords" },
            { GameId.KenoPower, "_KenoPowerRecords" },
            { GameId.KenoPower2, "_KenoPowerRecords" },


        };

        public string GetView(GameId game)
        {
            return views.ContainsKey(game) ? views[game] : RemoteRecordView;
        }
    }
}