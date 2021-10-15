using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Currencies.Requests;
using GamesAdmin.Core.Models.Currencies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Currencies
{
    [Route("api/currency")]
    public class CurrencyController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public CurrencyController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<IEnumerable<CurrencyModel>> GetAll()
            => mediator.Send(new GetAllRequest());

        [HttpPost]
        public Task<bool> Create(CreateCurrencyModel model)
            => mediator.Send(new CreateRequest(model));

        [HttpPut]
        public Task<bool> Update(UpdateCurrencyModel model)
            => mediator.Send(new UpdateRequest(model));
    }
}