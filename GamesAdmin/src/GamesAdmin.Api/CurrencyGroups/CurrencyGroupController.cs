using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.CurrencyGroups.Requests;
using GamesAdmin.Core.Models.Currencies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.CurrencyGroups
{
    [Route("/api/currency_group")]
    public class CurrencyGroupController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public CurrencyGroupController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("all")]
        public Task<IEnumerable<CurrencyGroupModel>> GetAll()
            => mediator.Send(new GetAllRequest());

        [HttpGet("{id}")]
        public Task<CurrencyGroupModel> GetById(string id)
            => mediator.Send(new GetByIdRequest(id));

        [HttpPost("filter")]
        public Task<IEnumerable<CurrencyGroupModel>> GetByGroupIds(GetByIdsModel model)
            => mediator.Send(new GetByIdsRequest(model));

        [HttpGet("partner/{partnerId}")]
        public Task<IEnumerable<CurrencyGroupModel>> GetByPartner(string partnerId)
            => mediator.Send(new GetByPartnerRequest(partnerId));

        [HttpPost]
        public Task<bool> Create(CreateCurrencyGroupModel model)
            => mediator.Send(new CreateRequest(model));

        [HttpPut]
        public Task<bool> Update(UpdateCurrencyGroupModel model)
            => mediator.Send(new UpdateRequest(model));
    }
}