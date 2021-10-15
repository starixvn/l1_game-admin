using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Core.Models.Partner;
using GamesAdmin.Api.Partner.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Partner
{
    public class PartnerController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public PartnerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<IEnumerable<PartnerInfo>> GetAll()
           => mediator.Send(new GetAllRequest());

        [HttpPost("add")]
        public Task<bool> Create(PartnerInfo partner)
            => mediator.Send(new CreateRequest(partner));

        [HttpPost("delete")]
        public Task<bool> Delete(string partnerId)
          => mediator.Send(new DeleteRequest(partnerId));

        [HttpPost("update")]
        public Task<bool> Get(PartnerInfo partner)
           => mediator.Send(new UpdateRequest(partner));

    }
}
