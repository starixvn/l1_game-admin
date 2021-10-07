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

        [HttpGet("get_site_exclusions")]
        public Task<GlobalConfig> GetSiteExclusions()
            => mediator.Send(new GetGlobalConfigRequest(GlobalConfigType.SiteExclusion));

        [HttpGet("get_super_players")]
        public Task<GlobalConfig> GetSuperPlayers()
            => mediator.Send(new GetGlobalConfigRequest(GlobalConfigType.SuperPlayers));

        [HttpPost("{name}/update_site_exclusions")]
        public Task<bool> UpdateSiteExclusions(string name, string[] exclusions)
            => mediator.Send(new UpdateSiteExclusionRequest(name, exclusions));

        [HttpPost("update_super_players")]
        public Task<bool> UpdateSuperPlayers(string[] superPlayers)
            => mediator.Send(new UpdateSuperPlayersRequest(superPlayers));
    }
}