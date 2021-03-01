using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.GlobalConfigs.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GlobalConfigs
{
    public class GlobalConfigHandler :
        IRequestHandler<UpdateSiteExclusionRequest, bool>
        , IRequestHandler<GetGlobalConfigRequest, GlobalConfig>
    {
        private readonly IGlobalConfigService globalConfigService;
        private readonly IGameSettingsService gameSettingService;

        public GlobalConfigHandler(IGlobalConfigService globalConfigService, IGameSettingsService gameSettingService)
        {
            this.globalConfigService = globalConfigService;
            this.gameSettingService = gameSettingService;
        }

        public async Task<bool> Handle(UpdateSiteExclusionRequest request, CancellationToken cancellationToken)
        {
            var result = await globalConfigService.UpdateSiteExclusions(request.Name, request.Exclusions);
            await gameSettingService.UpdateSiteExclusion(request.Name, request.Exclusions);

            return result;
        }

        public Task<GlobalConfig> Handle(GetGlobalConfigRequest request, CancellationToken cancellationToken)
        => globalConfigService.GetGlobalConfigByType(request.Type);
    }
}