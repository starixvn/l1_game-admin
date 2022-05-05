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
    public class GetByCodeHandler : ExchangeRateHandler, IRequestHandler<GetByExchangeIdRequest, ExchangeRate>
    {
        public GetByCodeHandler(IMapper mapper, IGenericRepository<ExchangeRateEntity> repository) : base(mapper, repository)
        {
        }

        public async Task<ExchangeRate> Handle(GetByExchangeIdRequest request, CancellationToken cancellationToken)
        {
            var result = await Repository.FindOneAsync(entity => entity.ExId == request.ExchangeId);

            return Mapper.Map<ExchangeRate>(result);
        }
    }
}