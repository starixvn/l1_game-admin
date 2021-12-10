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
using Sentry;

namespace GamesAdmin.Api.Currencies.Handlers
{
    public class GetByPartnersHandler : Handler, IRequestHandler<GetByPartnersRequest, IEnumerable<CurrencyModel>>
    {
        public GetByPartnersHandler(IMapper mapper, IGenericRepository<CurrencyEntity> repository, ISentryClient sentry) : base(mapper, repository, sentry)
        {
        }

        public async Task<IEnumerable<CurrencyModel>> Handle(GetByPartnersRequest request, CancellationToken cancellationToken)
        {
            var allEntities = await Repository.FilterByAsync(entity => true);
            var entities = allEntities.AsEnumerable().Where(entity => request.Model.PartnerIds.Any(id => string.Equals(id, entity.PartnerId)));

            return entities.Select(entity => Mapper.Map<CurrencyModel>(entity)).ToList();
        }
    }
}