using GamesAdmin.Core.Enumeration;
using System;
using System.Collections.Generic;

namespace GamesAdmin.Api.Report
{
    public interface IFourGuardiansReportFactory
    {
        IFourGuardiansReport GetReportService(Game game);
    }

    public class FourGuardiansReportFactory : IFourGuardiansReportFactory
    {
        private readonly Dictionary<string, Type> reports = new Dictionary<string, Type>
        {
            { Game.FourGuardiansValue, typeof(FourGuardiansReport) },
            { Game.HeroesSlotValue, typeof(HeroesSlotReport) },
        };

        private readonly IServiceProvider serviceProvider;

        public FourGuardiansReportFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IFourGuardiansReport GetReportService(Game game)
            => reports.ContainsKey(game.Value)
            ? (IFourGuardiansReport)serviceProvider.GetService(reports[game.Value])
            : default;
    }
}
