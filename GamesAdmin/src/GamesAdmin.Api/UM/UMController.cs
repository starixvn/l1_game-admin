using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.UM.Requests;
using GamesAdmin.Core.Models.Um;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.UM
{
    [Route("api/um")]
    public class UMController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public UMController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public Task<bool> Start([FromBody] UMRequest request)
        => mediator.Send(request);

        [HttpPost]
        [Route("add")]
        public Task<bool> Add([FromBody] UmSchedule eventSchedule)
        => mediator.Send(new CreateRequest(eventSchedule));

        [HttpGet]
        [Route("all")]
        public Task<IList<UmSchedule>> GetAll()
        => mediator.Send(new GetAllRequest());

        [HttpGet]
        [Route("unfinished")]
        public Task<IList<UmSchedule>> GetUnfinished()
        => mediator.Send(new GetUnfinishedUmRequest());

        [HttpGet]
        [Route("allUMInfo")]
        public Task<IEnumerable<UmInfo>> GetPartnerUmInfo()
            => mediator.Send(new GetUMInfoRequest());

        [HttpPost]
        [Route("partners")]
        public Task<bool> UpdatePartnerUmInfo(ServerApiUMRequest updateRequest)
            => mediator.Send(updateRequest);

        [HttpPost]
        [Route("partner")]
        public Task<bool> UpdatePartnerUmInfoFromAPI([FromBody] UpdatePartnerUMFromAPIRequest request)
            => mediator.Send(new ServerApiUMRequest(request.IsUM, true, request.StartTime, request.EndTime, new string[] {request.PartnerId}));
    }
}