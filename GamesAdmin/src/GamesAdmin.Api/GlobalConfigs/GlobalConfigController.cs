using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GlobalConfigs.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.GlobalConfigs
{
    [Route("api/global_configs")]
    public class GlobalConfigController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public GlobalConfigController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("{name}/update_site_exclusions")]
        public Task<bool> UpdateSiteExclusions(string name, string[] exclusions)
          => mediator.Send(new UpdateSiteExclusionRequest(name, exclusions));

        [HttpGet("get_site_exclusions")]
        public Task<GlobalConfig> GetSiteExclusions()
          => mediator.Send(new GetGlobalConfigRequest(GlobalConfigType.SiteExclusion));
    }
}