using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared.Helpers;
using GamesAdmin.Api.BuyFreeSpins.Requests;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MediatR;
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
                var gameConfig = await Repository.FindOneAsync(entity => entity.GameId == request.GameId);

                var oldConfig = gameConfig.CascadingSlotsConfig.Features.BuyFreeSpins;
                var newConfig = new BuyFreeSpinSetting
                {
                    Enabled = request.Enabled,
                    Multiplier = request.Multiplier,
                    SiteExclusions = request.SiteExclusions
                };

                var settingChanged = SettingHelper.GetCascadingSlotBuyFreeSpinConfigsChangeModel(oldConfig, newConfig);
                Sentry.CaptureChangedSettings(gameConfig.Name, "Buy Free Spin", "Update", settingChanged);


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