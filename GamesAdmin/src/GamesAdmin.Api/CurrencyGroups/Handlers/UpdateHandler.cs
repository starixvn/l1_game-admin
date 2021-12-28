using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroups.Requests;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.CurrencyGroups.Handlers
{
    public class UpdateHandler : Handler, IRequestHandler<UpdateRequest, bool>
    {
        public UpdateHandler(IMapper mapper, IGenericRepository<CurrencyGroupEntity> repository, ICurrencyService currency, ISentryClient sentry)
            : base(mapper, repository, currency, sentry)
        {
        }

        public async Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Model;
                var update = Builders<CurrencyGroupEntity>.Update
                    .Set(field => field.Name, model.Name)
                    .Set(field => field.Enabled, model.Enabled)
                    .Set(field => field.PartnerId, model.PartnerId);

                await Repository.UpdateAsync(entity => entity.Id.Equals(ObjectId.Parse(model.Id)), update);

                var currencies = await Currency.GetByGroup(request.Model.Id);
                var currenciesNeedRemovingGroup = currencies.Where(currency
                    => !request.Model.Currencies.Any(currencyId => currencyId.Equals(currency.Id)));

                foreach (var currency in request.Model.Currencies)
                {
                    await Currency.ChangeCurrencyGroup(currency, model.Id);
                }

                foreach (var currency in currenciesNeedRemovingGroup)
                {
                    await Currency.ChangeCurrencyGroup(currency.Id, string.Empty);
                }

                return true;
            }
            catch (Exception ex)
            {
                Sentry.CaptureException(ex);

                return false;
            }
        }
    }
}