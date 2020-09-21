using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.GameSettings;
using GamesAdmin.Site.Features.ProVietKeno.Requests;
using GamesAdmin.Site.Features.ProVietKeno.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ProVietKeno
{
    public class ProVietKenoHandler :
        IRequestHandler<GetRoundsRequest, IEnumerable<ProVietKenoRoundViewModel>>,
        IRequestHandler<CreateRoundRequest, Unit>,
        IRequestHandler<NotifyRoundResultRequest, Unit>,
        IRequestHandler<SettleRoundRequest, Unit>,
        IRequestHandler<GetByRoundIdRequest, ProVietKenoRound>
    {
        private readonly IProVietKenoService provietKenoService;
        private readonly IAppSettings appSettings;

        public ProVietKenoHandler(
            IProVietKenoService provietKenoService, 
            IAppSettings appSettings)
        {
            this.appSettings = appSettings;
            this.provietKenoService = provietKenoService;
        }

        public async Task<IEnumerable<ProVietKenoRoundViewModel>> Handle(GetRoundsRequest request, CancellationToken cancellationToken)
        {
            var utcDate = TimeZoneInfo.ConvertTimeToUtc(request.Date, appSettings.DefaultTimeZone);
            var result = await provietKenoService.GetRoundsAsync(utcDate);

            return result.OrderByDescending(x => x.Number).Select(x => new ProVietKenoRoundViewModel
            {
                Result = x.Result,
                RoundId = x.Number,
                Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone),
                Big = x.Big,
                Small = x.Small,
                Even = x.Even,
                Odd = x.Odd,
                Earth = x.Earth,
                Fire = x.Fire,
                Gold = x.Gold,
                Wood = x.Wood,
                Water = x.Water,
                Settled = x.Settled
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

            var createModel = new CreateProVietKenoRound 
            {
                Time = request.KenoRound.Date.ToUniversalTime(),
                Number = request.KenoRound.RoundNo.Value,
                Result = result,
            };

            var resultModel = await provietKenoService.CreateAsync(createModel);
            await provietKenoService.NotifyRoundResult(new List<ProVietKenoRound> { resultModel });

            return Unit.Value;
        }

        public async Task<Unit> Handle(NotifyRoundResultRequest request, CancellationToken cancellationToken)
        {
            await provietKenoService.NotifyRoundResult(request.KenoRounds);

            return Unit.Value;
        }

        public async Task<ProVietKenoRound> Handle(GetByRoundIdRequest request, CancellationToken cancellationToken)
        {
            return await provietKenoService.GetByRoundIdAsync(request.RoundId);
        }

        public async Task<Unit> Handle(SettleRoundRequest request, CancellationToken cancellationToken)
        {
            var round = await provietKenoService.GetByRoundIdAsync(request.RoundId);
            await provietKenoService.NotifyRoundResult(new[] { round });

            return Unit.Value;
        }
    }
}