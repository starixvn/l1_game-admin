using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Api.Report
{
    public interface ICascadingSlotReportFactory
    {
        ICascadingSlotReportService GetReportService(Game game);
    }

    public class CascadingSlotReportServiceFactory : ICascadingSlotReportFactory
    {
        private readonly Dictionary<string, Type> reports = new Dictionary<string, Type>
        {
            { Game.TreasureIslandValue, typeof(TreasureIslandReportService) },
        };

        private readonly IServiceProvider serviceProvider;

        public CascadingSlotReportServiceFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ICascadingSlotReportService GetReportService(Game game)
            => reports.ContainsKey(game.Value)
            ? (ICascadingSlotReportService)serviceProvider.GetService(reports[game.Value])
            : default;
    }
}
