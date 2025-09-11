using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.ChatBot;
using GamesAdmin.Api.Currencies.Requests;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using MediatR;
using Sentry;

namespace GamesAdmin.Api.Currencies.Handlers
{
    public class CreateHandler : Handler, IRequestHandler<CreateRequest, bool>
    {
        private readonly IChatBotService chatBotService;

        public CreateHandler(ISentryClient sentryClient, IMapper mapper, IGenericRepository<CurrencyEntity> repository, IChatBotService chatBotService)
            : base(mapper, repository, sentryClient)
        {
            this.chatBotService = chatBotService;
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

                    NotifyCurrencyCreated(request);
                }
            }
            catch (Exception exception)
            {
                Sentry.CaptureException(exception);

                return false;
            }

            return true;
        }

        private void NotifyCurrencyCreated(CreateRequest request)
        {
            try
            {
                // Send notification asynchronously to avoid blocking
                _ = Task.Run(() => chatBotService.NotifyAdminAddSettings(
                    "ADD",
                    "Currency",
                    request.Model));
            }
            catch (Exception ex)
            {
                // Log the exception but don't throw to avoid breaking the main flow
                Sentry.CaptureException(ex);
            }
        }
    }
}