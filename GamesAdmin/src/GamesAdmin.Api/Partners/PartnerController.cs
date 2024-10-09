using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Partners.Requests;
using GamesAdmin.Core.Models.Partners;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Partners
{
    [Route("api/partner")]
    public class PartnerController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public PartnerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<IEnumerable<Partner>> GetAll()
           => mediator.Send(new GetAllRequest());

        [HttpPost("add")]
        public Task<bool> Create(Partner partner)
            => mediator.Send(new CreateRequest(partner));

        [HttpPost("delete")]
        public Task<bool> Delete(string partnerId)
          => mediator.Send(new DeleteRequest(partnerId));

        [HttpPost("update")]
        public Task<bool> Update(Partner partner)
           => mediator.Send(new UpdateRequest(partner));

        [HttpGet("{partnerId}")]
        public Task<Partner> GetById(string partnerId)
            => mediator.Send(new GetByIdRequest(partnerId));

        [HttpGet("config")]
        public Task<PartnerConfig> GetConfig(string partnerId, string type)
            => mediator.Send(new GetConfigRequest(partnerId, type));

        [HttpPost("site_exclusion")]
        public Task<bool> UpdateSiteExclusion([FromBody] UpdateSiteExclusionRequest request)
            => mediator.Send(request);

        [HttpPost("super_players")]
        public Task<bool> UpdateSuperPlayers(string partnerId, string[] superPlayers)
            => mediator.Send(new UpdateSuperPlayersRequest(partnerId, superPlayers));
    }
}