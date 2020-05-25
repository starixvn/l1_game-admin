using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.VietlottKeno.Requests;
using GamesAdmin.Site.Features.VietlottKeno.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.VietlottKeno
{
    public class VietlottKenoHandler : IRequestHandler<GetRoundsRequest, IEnumerable<ViewlottKenoRoundViewModel>>
    {
        private readonly IVietlottKenoService vietlottKenoService;
        private readonly IAppSettings appSettings;

        public VietlottKenoHandler(IVietlottKenoService vietlottKenoService, IAppSettings appSettings)
        {
            this.appSettings = appSettings;
            this.vietlottKenoService = vietlottKenoService;
        }

        public async Task<IEnumerable<ViewlottKenoRoundViewModel>> Handle(GetRoundsRequest request, CancellationToken cancellationToken)
        {
            var result = await vietlottKenoService.GetRoundsAsync(request.Date);

            return result.OrderByDescending(x => x.Time).Select(x => new ViewlottKenoRoundViewModel
            {
                Result = x.Result,
                RoundNo = x.Number.ToString("D7"),
                Time = TimeZoneInfo.ConvertTime(x.Time, appSettings.DefaultTimeZone),
                Big = x.Big,
                Even = x.Even,
                Earth = x.Earth,
                Fire = x.Fire,
                Gold = x.Gold,
                Wood = x.Wood,
                Water = x.Water
            });
        }
    }
}