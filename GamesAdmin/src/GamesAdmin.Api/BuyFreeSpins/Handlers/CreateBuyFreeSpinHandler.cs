using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared.Helpers;
using GamesAdmin.Api.BuyFreeSpins.Requests;
using GamesAdmin.Api.ChatBot;
using GamesAdmin.Api.SentryClient;
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
        public CreateBuyFreeSpinHandler(IMapper mapper, IGenericRepository<GameConfigEntity> repository, ISentryClient sentry, IChatBotService chatBotService) : base(mapper, repository, sentry, chatBotService)
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

                var oldConfig = gameConfig.CascadingSlotsConfig.Features?.BuyFreeSpins ?? new BuyFreeSpinSetting();

                var newConfig = new BuyFreeSpinSetting
                {
                    Enabled = request.Enabled,
                    Multiplier = request.Multiplier,
                    SiteExclusions = request.SiteExclusions
                };

                var settingChanged = SettingHelper.GetCascadingSlotBuyFreeSpinConfigsChangeModel(oldConfig, newConfig);
                await ChatBotService.NotifyAdminUpdatedSettings(gameConfig.Name, "Buy Free Spin", "Add new", settingChanged);
                if (gameConfig?.CascadingSlotsConfig?.Features != null)
                {
                    var update = Builders<GameConfigEntity>.Update
                                            .Set(field => field.CascadingSlotsConfig.Features.BuyFreeSpins, newConfig);

                    await Repository.UpdateAsync(entity => entity.GameId.Equals(request.GameId), update);
                }
                else
                {

                    var update = Builders<GameConfigEntity>.Update
                                            .Set(field => field.CascadingSlotsConfig.Features, new CascadingSlotFeatures
                                            {
                                                BuyFreeSpins = newConfig
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