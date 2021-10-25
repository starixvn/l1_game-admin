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
    public class GetByPartnerHandler : Handler, IRequestHandler<GetByPartnerRequest, IEnumerable<CurrencyGroupModel>>
    {
        public GetByPartnerHandler(IMapper mapper, IGenericRepository<CurrencyGroupEntity> repository, ICurrencyService currency, ISentryClient sentry)
            : base(mapper, repository, currency, sentry)
        {
        }

        public async Task<IEnumerable<CurrencyGroupModel>> Handle(GetByPartnerRequest request, CancellationToken cancellationToken)
        {
            var currencies = await Currency.GetByPartner(request.PartnerId);
            var groups = await Repository.FilterByAsync(entity => string.Equals(entity.PartnerId, request.PartnerId));

            return groups.Select(group => TransformToModel(group, currencies)).ToList();
        }
    }
}