using System;
using System.Collections.Generic;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.IdentityServer;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    public interface ISlotBoardApiFactory
    {
        ISlotBoardApi SetBoardApi(string gameConfigName);
    }

    public class SlotBoardApiFactory : ISlotBoardApiFactory
    {
        private readonly Dictionary<string, Type> slotServices = new Dictionary<string, Type>
        {
            { Game.FourGuardiansValue, typeof(IFourGuardiansBoardApi) },
            { Game.HeroesSlotValue, typeof(IHeroesSlotBoardApi) },
            { Game.NightClubValue, typeof(INightClubBoardApi) },
            { Game.WayToGoalValue, typeof(IWayToGoalBoardApi) },
            { Game.FortuneCatValue, typeof(IFortuneCatApi) }
        };

        private readonly IServiceProvider serviceProvider;
        private readonly IAppSettings appSettings;

        public SlotBoardApiFactory(IServiceProvider serviceProvider, IAppSettings appSettings)
        {
            this.serviceProvider = serviceProvider;
            this.appSettings = appSettings;
        }

        public ISlotBoardApi SetBoardApi(string gameConfigName)
        {
            if (Enumeration.Contains<Game>(gameConfigName))
            {
                return (ISlotBoardApi)serviceProvider.GetService(slotServices[gameConfigName]);
            }

            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = IdentityServerManager.GetAccessToken
            };

            var api = string.Format(appSettings.ServiceApiTemplate, gameConfigName);

            return RestService.For<ISlotBoardApi>(api, refitSettings);
        }
    }
}