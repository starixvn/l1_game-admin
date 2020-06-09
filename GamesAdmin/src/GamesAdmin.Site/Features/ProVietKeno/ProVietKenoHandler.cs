using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.ProVietKeno.Requests;
using GamesAdmin.Site.Features.ProVietKeno.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ProVietKeno
{
    public class ProVietKenoHandler :
        IRequestHandler<GetRoundsRequest, IEnumerable<ProVietKenoRoundViewModel>>,
        IRequestHandler<CreateRoundRequest, Unit>,
        IRequestHandler<NotifyRoundResultRequest, Unit>
    {
        private readonly IProVietKenoService provietKenoService;
        private readonly IGameServerService gameServerService;
        private readonly IAppSettings appSettings;

        public ProVietKenoHandler(IProVietKenoService provietKenoService, IGameServerService gameServerService, IAppSettings appSettings)
        {
            this.appSettings = appSettings;
            this.provietKenoService = provietKenoService;
            this.gameServerService = gameServerService;
        }

        public async Task<IEnumerable<ProVietKenoRoundViewModel>> Handle(GetRoundsRequest request, CancellationToken cancellationToken)
        {
            var result = await provietKenoService.GetRoundsAsync(request.Date);

            return result.OrderByDescending(x => x.Time).Select(x => new ProVietKenoRoundViewModel
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

        public async Task<Unit> Handle(CreateRoundRequest request, CancellationToken cancellationToken)
        {
            var result = new List<byte>();
            for (var i = 0; i < request.KenoRound.Result.Count(); i++)
            {
                if (request.KenoRound.Result[i])
                {
                    result.Add((byte)(i + 1));
                }
            }

            var model = new ProVietKenoRound
            {
                Time = request.KenoRound.Date.ToUniversalTime(),
                Number = request.KenoRound.RoundNo.Value,
                Result = result
            };

            await provietKenoService.CreateAsync(model);

            return Unit.Value;
        }

        public async Task<Unit> Handle(NotifyRoundResultRequest request, CancellationToken cancellationToken)
        {
            await gameServerService.NotifyRoundResult(request.KenoRounds);

            return Unit.Value;
        }
    }
}