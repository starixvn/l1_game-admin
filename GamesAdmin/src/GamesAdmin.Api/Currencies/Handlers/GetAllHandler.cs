using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies.Requests;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using MediatR;

namespace GamesAdmin.Api.Currencies.Handlers
{
    public class GetAllHandler : Handler, IRequestHandler<GetAllRequest, IEnumerable<CurrencyModel>>
    {
        public GetAllHandler(IMapper mapper, IGenericRepository<CurrencyEntity> repository) : base(mapper, repository)
        {
        }

        public async Task<IEnumerable<CurrencyModel>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            var entities = await Repository.FilterByAsync(entity => true);

            return entities.Select(entity => Mapper.Map<CurrencyModel>(entity));
        }
    }
}