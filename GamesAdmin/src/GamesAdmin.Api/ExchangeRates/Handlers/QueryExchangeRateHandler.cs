using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.ExchangeRates.Requests;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.ExchangeRates;
using MediatR;

namespace GamesAdmin.Api.ExchangeRates.Handlers
{
    public class QueryExchangeRateHandler : ExchangeRateHandler, IRequestHandler<QueryExchangeRateRequest, IEnumerable<ExchangeRate>>
    {
        public QueryExchangeRateHandler(IMapper mapper, IGenericRepository<ExchangeRateEntity> repository) : base(mapper, repository)
        {
        }

        public async Task<IEnumerable<ExchangeRate>> Handle(QueryExchangeRateRequest request, CancellationToken cancellationToken)
        {
            var result = (await Repository.FilterByAsync(entity => entity.Time >= request.From && entity.Time < request.To));

            return Mapper.Map<IEnumerable<ExchangeRate>>(result).AsEnumerable();
        }
    }
}