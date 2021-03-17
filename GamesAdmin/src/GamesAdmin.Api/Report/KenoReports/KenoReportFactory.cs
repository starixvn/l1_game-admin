using System;
using System.Collections.Generic;
using GamesAdmin.Api.Report.KenoReports;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Api.Report
{
    public interface IKenoReportFactory
    {
        IKenoReport GetReportService(Game game);
    }

    public class KenoReportFactory : IKenoReportFactory
    {
        private readonly Dictionary<string, Type> reports = new Dictionary<string, Type>
        {
            { Game.KenoProVietValue, typeof(KenoProVietReport) },
            { Game.KenoMaxValue, typeof(KenoMaxReport) },
            { Game.KenoMax2Value, typeof(KenoMax2Report) },
            { Game.KenoMiniValue, typeof(KenoMiniReport) },
            { Game.KenoMini2Value, typeof(KenoMini2Report) },
            { Game.KenoEastValue, typeof(KenoEastReport) },
            { Game.KenoWestValue, typeof(KenoWestReport) },
            { Game.KenoSouthValue, typeof(KenoSouthReport) },
            { Game.KenoNorthValue, typeof(KenoNorthReport) },
            { Game.KenoProMaxValue, typeof(KenoProMaxReport) }
        };

        private readonly IServiceProvider serviceProvider;

        public KenoReportFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IKenoReport GetReportService(Game game)
        => reports.ContainsKey(game.Value)
            ? (IKenoReport)serviceProvider.GetService(reports[game.Value])
            : default;
    }
}
