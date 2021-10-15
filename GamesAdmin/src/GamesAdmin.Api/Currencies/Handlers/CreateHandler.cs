using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies.Requests;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using MediatR;
using Sentry;

namespace GamesAdmin.Api.Currencies.Handlers
{
    public class CreateHandler : Handler, IRequestHandler<CreateRequest, bool>
    {
        private readonly ISentryClient sentryClient;

        public CreateHandler(ISentryClient sentryClient, IMapper mapper, IGenericRepository<CurrencyEntity> repository)
            : base(mapper, repository)
        {
            this.sentryClient = sentryClient;
        }

        public async Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var currency = await Repository.FindOneAsync(entity
                    => entity.PartnerId.Equals(request.Model.PartnerId) && entity.Name.Equals(request.Model.Name));

                if (currency == null)
                {
                    var entity = Mapper.Map<CurrencyEntity>(request.Model);

                    await Repository.InsertOneAsync(entity);
                }
            }
            catch (Exception exception)
            {
                sentryClient.CaptureException(exception);

                return false;
            }

            return true;
        }
    }
}