using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.BuyFreeSpins.Requests;
using GamesAdmin.Api.BuyFreeSpins.ViewModels;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MediatR;
using Sentry;

namespace GamesAdmin.Api.BuyFreeSpins.Handlers
{
    public class GetAllBuyFreeSpinsHandler : BuyFreeSpinHandler, IRequestHandler<GetAllBuyFreeSpinsRequest, IEnumerable<BuyFreeSpinSettingViewModel>>
    {
        public GetAllBuyFreeSpinsHandler(IMapper mapper, IGenericRepository<GameConfigEntity> repository, ISentryClient sentry) : base(mapper, repository, sentry)
        {
        }

        public async Task<IEnumerable<BuyFreeSpinSettingViewModel>> Handle(GetAllBuyFreeSpinsRequest request, CancellationToken cancellationToken)
        {
            var entities = await Repository.FilterByAsync(entity =>
                entity.CascadingSlotsConfig.Features != null &&
                entity.CascadingSlotsConfig.Features.BuyFreeSpins != null, entity => new
                {
                    entity.GameId,
                    entity.DisplayName,
                    BuyFreeSpins = entity.CascadingSlotsConfig.Features.BuyFreeSpins
                });

            var entityList = entities?.ToList();
            if (entityList == null || !entityList.Any())
            {
                return Enumerable.Empty<BuyFreeSpinSettingViewModel>();
            }

            var result = entityList.Select(entity => new BuyFreeSpinSettingViewModel
            {
                Enabled = entity.BuyFreeSpins.Enabled,
                GameId = entity.GameId,
                Multiplier = entity.BuyFreeSpins.Multiplier,
                GameName = entity.DisplayName,
                SiteExclusions = entity.BuyFreeSpins.SiteExclusions
            });
            return result;
        }
    }
}