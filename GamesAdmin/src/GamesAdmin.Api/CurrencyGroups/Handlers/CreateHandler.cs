using System;
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
    public class CreateHandler : Handler, IRequestHandler<CreateRequest, bool>
    {
        public CreateHandler(IMapper mapper, IGenericRepository<CurrencyGroupEntity> repository, ICurrencyService currency, ISentryClient sentry)
            : base(mapper, repository, currency, sentry)
        {
        }

        public async Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = Mapper.Map<CurrencyGroupEntity>(request.Model);

                await Repository.InsertOneAsync(entity);

                foreach (var currency in request.Model.Currencies)
                {
                    await Currency.ChangeCurrencyGroup(currency, entity.Id.ToString());
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