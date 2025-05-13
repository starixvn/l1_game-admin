using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.GameSettings.JackpotSettingsRequests;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings
{
    public class JackpotSettingsHandler :
        IRequestHandler<UpdatePlayerSettingsRequest, UpdateJackpotSettingsResult>,
        IRequestHandler<UpdateBotSettingsRequest, UpdateJackpotSettingsResult>,
        IRequestHandler<UpdateBotRatioSettingsRequest, UpdateJackpotSettingsResult>
    {
        private readonly IGameSettingsService gameService;

        public JackpotSettingsHandler(IGameSettingsService gameService)
        {
            this.gameService = gameService;
        }

        public async Task<UpdateJackpotSettingsResult> Handle(UpdatePlayerSettingsRequest request, CancellationToken cancellationToken)
        {
            var id = await gameService.UpdateJackpotPlayerSettings(request.Settings);

            if (string.IsNullOrWhiteSpace(id))
            {
                return UpdateJackpotSettingsResult.FailedResult();
            }

            return UpdateJackpotSettingsResult.SuccessResult(id);
        }

        public async Task<UpdateJackpotSettingsResult> Handle(UpdateBotSettingsRequest request, CancellationToken cancellationToken)
        {
            var id = await gameService.UpdateJackpotBotSettings(request.Settings);

            if (string.IsNullOrWhiteSpace(id))
            {
                return UpdateJackpotSettingsResult.FailedResult();
            }

            return UpdateJackpotSettingsResult.SuccessResult(id);
        }

        public async Task<UpdateJackpotSettingsResult> Handle(UpdateBotRatioSettingsRequest request, CancellationToken cancellationToken)
        {
            var result = await gameService.UpdateBotRatioSettings(request.Settings);
            if (result)
            {
                return UpdateJackpotSettingsResult.SuccessResult(request.Settings.Id);
            }

            return UpdateJackpotSettingsResult.FailedResult();
        }
    }
}