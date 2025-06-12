using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies.Requests;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using MediatR;
using Sentry;
using GamesAdmin.Api.SentryClient;

namespace GamesAdmin.Api.Currencies.Handlers
{
    public class CreateHandler : Handler, IRequestHandler<CreateRequest, bool>
    {
        public CreateHandler(ISentryClient sentryClient, IMapper mapper, IGenericRepository<CurrencyEntity> repository)
            : base(mapper, repository, sentryClient)
        {
        }

        public async Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Model;
                var currency = await Repository.FindOneAsync(entity
                    => string.Equals(entity.PartnerId, model.PartnerId) && entity.Name.ToUpper() == model.Name.ToUpper());

                if (currency == null)
                {
                    var entity = Mapper.Map<CurrencyEntity>(request.Model);

                    entity.IsUncountOnTotal = entity.IsTest && entity.IsUncountOnTotal;
                    entity.Name = entity.Name.ToUpper();

                    await Repository.InsertOneAsync(entity);

                    Sentry.CaptureAddSetting(entity.Name, "New Currency", entity);
                }
            }
            catch (Exception exception)
            {
                Sentry.CaptureException(exception);

                return false;
            }

            return true;
        }
    }
}