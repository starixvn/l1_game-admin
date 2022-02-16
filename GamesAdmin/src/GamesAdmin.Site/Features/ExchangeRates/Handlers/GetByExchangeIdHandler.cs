using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.ExchangeRates.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.ExchangeRates.Handlers
{
    public class GetByExchangeIdHandler : ExchangeRateHandler, IRequestHandler<GetByExchangeIdRequest, ExchangeRate>
    {
        public GetByExchangeIdHandler(IExchangeRateApi api, IMapper mapper) : base(api, mapper)
        {
        }

        public Task<ExchangeRate> Handle(GetByExchangeIdRequest request, CancellationToken cancellationToken)
            => Api.GetByExchangeId(request.ExchangeId);
    }
}