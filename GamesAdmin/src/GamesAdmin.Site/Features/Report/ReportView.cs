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
        };

        public string GetView(GameId game) => views[game];
    }
}