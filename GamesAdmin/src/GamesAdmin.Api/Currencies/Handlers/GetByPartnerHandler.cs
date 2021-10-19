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
    public class GetByPartnerHandler : Handler, IRequestHandler<GetByPartnerRequest, IEnumerable<CurrencyModel>>
    {
        public GetByPartnerHandler(IMapper mapper, IGenericRepository<CurrencyEntity> repository) : base(mapper, repository)
        {
        }

        public async Task<IEnumerable<CurrencyModel>> Handle(GetByPartnerRequest request, CancellationToken cancellationToken)
        {
            var entities = await Repository.FilterByAsync(entity => string.Equals(entity.PartnerId, request.Model.PartnerId));

            return entities.Select(entity => Mapper.Map<CurrencyModel>(entity)).ToList();
        }
    }
}