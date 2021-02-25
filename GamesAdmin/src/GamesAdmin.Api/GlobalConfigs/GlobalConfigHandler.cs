using System.Threading;
using System.Threading.Tasks;
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

        public GlobalConfigHandler(IGlobalConfigService globalConfigService)
        {
            this.globalConfigService = globalConfigService;
        }

        public Task<bool> Handle(UpdateSiteExclusionRequest request, CancellationToken cancellationToken)
       => globalConfigService.UpdateSiteExclusions(request.Name, request.Exclusions);

        public Task<GlobalConfig> Handle(GetGlobalConfigRequest request, CancellationToken cancellationToken)
        => globalConfigService.GetGlobalConfigByType(request.Type);
    }
}