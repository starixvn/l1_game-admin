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
    public class GetByOperatorAndCurrencyHandler : Handler, IRequestHandler<GetByOperatorAndCurrencyRequest, CurrencyGroupModel>
    {
        public GetByOperatorAndCurrencyHandler(IMapper mapper, IGenericRepository<CurrencyGroupEntity> repository, ICurrencyService currency, ISentryClient sentry)
            : base(mapper, repository, currency, sentry)
        {
        }

        public async Task<CurrencyGroupModel> Handle(GetByOperatorAndCurrencyRequest request, CancellationToken cancellationToken)
        {
            var currencyModel = (await Currency.GetByPartner(request.OperatorId)).FirstOrDefault(c => c.Name == request.CurrencyName);
            if (!string.IsNullOrEmpty(currencyModel?.GroupId))
            {
                var currencyGroup = await Repository.FindByIdAsync(currencyModel.GroupId);
                if (!string.IsNullOrEmpty(currencyGroup?.Id.ToString()))
                {
                    return new CurrencyGroupModel(currencyGroup.Id.ToString(), currencyGroup.Name, currencyGroup.Enabled, currencyGroup.PartnerId, currencyGroup.FractionalOddsFormat, Enumerable.Empty<CurrencyModel>());
                }
            }
            return null;
        }
    }
}