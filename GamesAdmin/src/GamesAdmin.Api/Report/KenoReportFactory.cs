using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Api.Report
{
    public interface IKenoReportFactory
    {
        IKenoReport GetReportService(GameType game);
    }

    public class KenoReportFactory : IKenoReportFactory
    {
        private readonly Dictionary<string, Type> reports = new Dictionary<string, Type>
        {
            { GameType.KenoProVietValue, typeof(KenoProVietReport) },
            { GameType.KenoMaxValue, typeof(KenoMaxReport) }
        };

        private readonly IServiceProvider serviceProvider;

        public KenoReportFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IKenoReport GetReportService(GameType game)
        => reports.ContainsKey(game.Value)
            ? (IKenoReport)serviceProvider.GetService(reports[game.Value])
            : default;
    }
}
