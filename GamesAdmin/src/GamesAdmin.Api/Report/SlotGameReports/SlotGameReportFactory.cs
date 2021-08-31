using GamesAdmin.Core.Enumeration;
using System;
using System.Collections.Generic;

namespace GamesAdmin.Api.Report
{
    public interface ISlotGameReportFactory
    {
        ISlotGameReport GetReportService(Game game);
    }

    public class SlotGameReportFactory : ISlotGameReportFactory
    {
        private readonly Dictionary<string, Type> reports = new Dictionary<string, Type>
        {
            { Game.FourGuardiansValue, typeof(FourGuardiansReport) },
            { Game.HeroesSlotValue, typeof(HeroesSlotReport) },
        };

        private readonly IServiceProvider serviceProvider;

        public SlotGameReportFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ISlotGameReport GetReportService(Game game)
            => reports.ContainsKey(game.Value)
            ? (ISlotGameReport)serviceProvider.GetService(reports[game.Value])
            : default;
    }
}
