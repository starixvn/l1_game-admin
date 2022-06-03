using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.Report
{
    public class ReportView
    {
        private readonly Dictionary<GameId, string> views = new Dictionary<GameId, string>
        {
            { GameId.SeDie, "_SeDieRecords" },
            { GameId.MiniSeDie, "_MiniSeDieRecords" },
            { GameId.FishPrawnCrab, "_FishPrawnCrabRecords" },
            { GameId.DragonTiger, "_DragonTigerRecords" },
            { GameId.Baccarat, "_BaccaratRecords" },
            { GameId.Roulette, "_RouletteRecords" },
            { GameId.VipSeDie, "_VipSeDieRecords" },
            { GameId.SicBo, "_SicBoRecords" },
            { GameId.VipFishPrawnCrab, "_VipFishPrawnCrabRecords" },
            { GameId.VipBaccarat, "_VipBaccaratRecords" },
            { GameId.Lottery, "_LotteryRecords" },
            { GameId.ThreeCards, "_ThreeCardsRecords" },
            { GameId.KenoMatch1, "_KenoMatchRecords" },
            { GameId.KenoMatch2, "_KenoMatchRecords" },
            { GameId.KenoPower1, "_KenoPowerRecords" },
            { GameId.KenoPower2, "_KenoPowerRecords" },
            { GameId.KenoProMax, "_KenoBetRecords" },
            { GameId.KenoMax, "_KenoBetRecords" },
            { GameId.KenoMax2, "_KenoBetRecords" },
            { GameId.KenoMini, "_KenoBetRecords" },
            { GameId.KenoMini2, "_KenoBetRecords" },
            { GameId.KenoEast, "_KenoBetRecords" },
            { GameId.KenoWest, "_KenoBetRecords" },
            { GameId.KenoSouth, "_KenoBetRecords" },
            { GameId.KenoNorth, "_KenoBetRecords" },
            { GameId.VipSeDieWar, "_VipSeDieWarRecords" },
            { GameId.BlockchainBaccarat, "_BlockchainBaccaratRecords" },
        };

        public string GetView(GameId game) => views[game];
    }
}