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
    public class GetAllHandler : Handler, IRequestHandler<GetAllRequest, IEnumerable<CurrencyGroupModel>>
    {
        public GetAllHandler(IMapper mapper, IGenericRepository<CurrencyGroupEntity> repository, ICurrencyService currency, ISentryClient sentry)
            : base(mapper, repository, currency, sentry)
        {
        }

        public async Task<IEnumerable<CurrencyGroupModel>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            var entities = await Repository.FilterByAsync(entity => true);
            var currencies = await Currency.GetAll();

            return entities.Select(entity => TransformToModel(entity, currencies)).ToList();
        }
    }
}