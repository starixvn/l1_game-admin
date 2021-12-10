using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroups.Requests;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using MediatR;
using Sentry;

namespace GamesAdmin.Api.CurrencyGroups.Handlers
{
    public class GetByGroupIdsHandler : Handler, IRequestHandler<GetByIdsRequest, IEnumerable<CurrencyGroupModel>>
    {
        public GetByGroupIdsHandler(IMapper mapper, IGenericRepository<CurrencyGroupEntity> repository, ICurrencyService currency, ISentryClient sentry)
            : base(mapper, repository, currency, sentry)
        {
        }

        public async Task<IEnumerable<CurrencyGroupModel>> Handle(GetByIdsRequest request, CancellationToken cancellationToken)
        {
            var allEntities = await Repository.FilterByAsync(entity => true);
            var entities = allEntities.Where(entity => request.Model.CurrencyGroups.Any(id => string.Equals(id, entity.Id.ToString()))).AsEnumerable();
            var currencies = await Currency.GetAll();

            return entities.Select(entity => TransformToModel(entity, currencies)).ToList();
        }
    }
}