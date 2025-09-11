using AutoMapper;
using GamesAdmin.Api.BuyFreeSpins.Requests;
using GamesAdmin.Api.BuyFreeSpins.ViewModels;
using GamesAdmin.Api.ChatBot;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MediatR;
using Sentry;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.BuyFreeSpins.Handlers
{
    public class GetBuyFreeSpinByGameIdHandler : BuyFreeSpinHandler, IRequestHandler<GetBuyFreeSpinByGameIdRequest, BuyFreeSpinSettingViewModel>
    {
        public GetBuyFreeSpinByGameIdHandler(IMapper mapper, IGenericRepository<GameConfigEntity> repository, ISentryClient sentry, IChatBotService chatBotService) : base(mapper, repository, sentry, chatBotService)
        {
        }

        public async Task<BuyFreeSpinSettingViewModel> Handle(GetBuyFreeSpinByGameIdRequest request, CancellationToken cancellationToken)
        {
            var entity = await Repository.FindOneAsync(entity =>
                entity.GameId == request.GameId &&
                entity.CascadingSlotsConfig.Features != null &&
                entity.CascadingSlotsConfig.Features.BuyFreeSpins != null, entity => new
                {
                    entity.GameId,
                    entity.DisplayName,
                    BuyFreeSpins = entity.CascadingSlotsConfig.Features.BuyFreeSpins
                });

            var result = new BuyFreeSpinSettingViewModel
            {
                Enabled = entity.BuyFreeSpins.Enabled,
                GameId = entity.GameId,
                Multiplier = entity.BuyFreeSpins.Multiplier,
                GameName = entity.DisplayName,
                SiteExclusions = entity.BuyFreeSpins.SiteExclusions
            };
            return result;
        }
    }
}