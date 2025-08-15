using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.BuyFreeSpins.Requests;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.BuyFreeSpins.Handlers
{
    public class UpdateBuyFreeSpinHandler : BuyFreeSpinHandler, IRequestHandler<UpdateBuyFreeSpinRequest, bool>
    {
        public UpdateBuyFreeSpinHandler(IMapper mapper, IGenericRepository<GameConfigEntity> repository, ISentryClient sentry) : base(mapper, repository, sentry)
        {
        }

        public async Task<bool> Handle(UpdateBuyFreeSpinRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var update = Builders<GameConfigEntity>.Update
                    .Set(field => field.CascadingSlotsConfig.Features.BuyFreeSpins.Enabled, request.Enabled)
                    .Set(field => field.CascadingSlotsConfig.Features.BuyFreeSpins.Multiplier, request.Multiplier)
                    .Set(field => field.CascadingSlotsConfig.Features.BuyFreeSpins.SiteExclusions, request.SiteExclusions);

                await Repository.UpdateAsync(entity => entity.GameId.Equals(request.GameId), update);

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