using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.GlobalConfigs.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GlobalConfigs
{
    public class GlobalConfigHandler :
        IRequestHandler<GetGlobalConfigRequest, GlobalConfig>,
        IRequestHandler<UpdateSiteExclusionRequest, bool>,
        IRequestHandler<UpdateSuperPlayersRequest, bool>
    {
        private readonly IGlobalConfigService globalConfigService;
        private readonly IGameSettingsService gameSettingService;

        public GlobalConfigHandler(IGlobalConfigService globalConfigService, IGameSettingsService gameSettingService)
        {
            this.globalConfigService = globalConfigService;
            this.gameSettingService = gameSettingService;
        }

        public Task<GlobalConfig> Handle(GetGlobalConfigRequest request, CancellationToken cancellationToken)
                    => globalConfigService.GetGlobalConfigByType(request.Type);

        public async Task<bool> Handle(UpdateSiteExclusionRequest request, CancellationToken cancellationToken)
        {
            var result = await globalConfigService.UpdateSiteExclusions(request.Name, request.Exclusions);
            await gameSettingService.UpdateSiteExclusion(request.Name, request.Exclusions);

            return result;
        }

        public async Task<bool> Handle(UpdateSuperPlayersRequest request, CancellationToken cancellationToken)
            => await globalConfigService.UpdateSuperPlayers(request.SuperPlayers);
    }
}