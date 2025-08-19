using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.BuyFreeSpins.Requests;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MediatR;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.BuyFreeSpins.Handlers
{
    public class CreateBuyFreeSpinHandler : BuyFreeSpinHandler, IRequestHandler<CreateBuyFreeSpinRequest, bool>
    {
        public CreateBuyFreeSpinHandler(IMapper mapper, IGenericRepository<GameConfigEntity> repository, ISentryClient sentry) : base(mapper, repository, sentry)
        {
        }

        public async Task<bool> Handle(CreateBuyFreeSpinRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var gameConfig = await Repository.FindOneAsync(entity => entity.GameId == request.GameId);

                if (gameConfig == null)
                {
                    return false;
                }

                if (gameConfig?.CascadingSlotsConfig?.Features?.BuyFreeSpins != null)                    
                {
                    return false;
                }

                if (gameConfig?.CascadingSlotsConfig?.Features != null)
                {                    
                    var update = Builders<GameConfigEntity>.Update
                                            .Set(field => field.CascadingSlotsConfig.Features.BuyFreeSpins, new BuyFreeSpinSetting
                                            {
                                                Enabled = request.Enabled,
                                                Multiplier = request.Multiplier,
                                                SiteExclusions = request.SiteExclusions
                                            });

                    await Repository.UpdateAsync(entity => entity.GameId.Equals(request.GameId), update);
                }
                else 
                {

                    var update = Builders<GameConfigEntity>.Update
                                            .Set(field => field.CascadingSlotsConfig.Features, new CascadingSlotFeatures
                                            {
                                                BuyFreeSpins = new BuyFreeSpinSetting
                                                {
                                                    Enabled = request.Enabled,
                                                    Multiplier = request.Multiplier,
                                                    SiteExclusions = request.SiteExclusions
                                                }
                                            });

                    await Repository.UpdateAsync(entity => entity.GameId.Equals(request.GameId), update);
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