using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.ExchangeRates.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.ExchangeRates
{
    [Route("/api/exchange_rate")]
    public class ExchangeRateController : BaseAuthorizeController
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ExchangeRateController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpPost("query")]
        public Task<IEnumerable<ExchangeRate>> Query(QueryExchangeRateModel model)
            => mediator.Send(mapper.Map<QueryExchangeRateRequest>(model));
    }
}