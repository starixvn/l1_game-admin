using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.BuyFreeSpins.Requests;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MediatR;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.BuyFreeSpins.Handlers
{
    public class EnableBuyFreeSpinHandler : BuyFreeSpinHandler, IRequestHandler<EnableBuyFreeSpinRequest, bool>
    {
        public EnableBuyFreeSpinHandler(IMapper mapper, IGenericRepository<GameConfigEntity> repository, ISentryClient sentry) : base(mapper, repository, sentry)
        {
        }

        public async Task<bool> Handle(EnableBuyFreeSpinRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var update = Builders<GameConfigEntity>.Update
                    .Set(field => field.CascadingSlotsConfig.Features.BuyFreeSpins.Enabled, request.IsEnabled);

                await Repository.UpdateManyAsync(entity => request.GameIds.Contains(entity.GameId), update);

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