using System;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Api.Report
{

    public interface IKenoReportFactory
    {
        IKenoReport GetReportService(GameType game);
    }

    public class KenoReportFactory : IKenoReportFactory
    {
        private readonly IServiceProvider serviceProvider;

        public KenoReportFactory(IServiceProvider serviceProvider) 
        {
            this.serviceProvider = serviceProvider;
        }

        public IKenoReport GetReportService(GameType game)
        {
            switch (game.Value)
            {
                case GameType.KenoProVietValue:
                    return (IKenoReport)serviceProvider.GetService(typeof(KenoProVietReport));

                case GameType.KenoMaxValue:
                    return (IKenoReport)serviceProvider.GetService(typeof(KenoMaxReport));
            }

            return default;
        }
    }
}
