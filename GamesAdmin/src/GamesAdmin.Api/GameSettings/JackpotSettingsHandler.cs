using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.GameSettings.JackpotSettingsRequests;
using GamesAdmin.Api.GeneralGameSettings;
using GamesAdmin.Api.Partners;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings
{
    public class JackpotSettingsHandler :
        IRequestHandler<UpdatePlayerSettingsRequest, UpdateJackpotSettingsResult>,
        IRequestHandler<UpdateBotSettingsRequest, UpdateJackpotSettingsResult>,
        IRequestHandler<UpdateBotRatioSettingsRequest, UpdateJackpotSettingsResult>,
        IRequestHandler<GetAllJackpotSettingsRequest, IEnumerable<JackpotSettings>>,
        IRequestHandler<GetJackpotSettingsRequest, JackpotSettings>,
        IRequestHandler<SearchRequest, IEnumerable<JackpotSettings>>
    {   
        private readonly IGameSettingsService gameService;
        private readonly IGeneralGameSettingService generalGameSettingService;
        private readonly IPartnerService partnerService;

        public JackpotSettingsHandler(IGameSettingsService gameService, IGeneralGameSettingService generalGameSettingService, IPartnerService partnerService)
        {
            this.gameService = gameService;
            this.generalGameSettingService = generalGameSettingService;
            this.partnerService = partnerService;
        }

        public async Task<UpdateJackpotSettingsResult> Handle(UpdatePlayerSettingsRequest request, CancellationToken cancellationToken)
        {
            var existedSitesResult = await GetExistedSitesResult(request.Settings);
            if (existedSitesResult != null)
            {
                return existedSitesResult;
            }


            var id = await gameService.UpdateJackpotPlayerSettings(request.Settings);

            if (string.IsNullOrWhiteSpace(id))
            {
                return UpdateJackpotSettingsResult.FailedResult();
            }

            return UpdateJackpotSettingsResult.SuccessResult(id);
        }

        public async Task<UpdateJackpotSettingsResult> Handle(UpdateBotSettingsRequest request, CancellationToken cancellationToken)
        {
            var existedSitesResult = await GetExistedSitesResult(request.Settings);
            if (existedSitesResult != null)
            {
                return existedSitesResult;
            }

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

        public async Task<JackpotSettings> Handle(GetJackpotSettingsRequest request, CancellationToken cancellationToken)
        {
            return await gameService.GetJackpotSettings(request.ConfigId);
        }

        public async Task<IEnumerable<JackpotSettings>> Handle(GetAllJackpotSettingsRequest request, CancellationToken cancellationToken)
        {
            return await gameService.GetAllJackpotSettings();
        }

        public async Task<IEnumerable<JackpotSettings>> Handle(SearchRequest request, CancellationToken cancellationToken)
        {
            return await gameService.GetJackpotSettings(request.GameIds, request.SiteNames ?? Enumerable.Empty<string>());
        }

        private async Task<UpdateJackpotSettingsResult> GetExistedSitesResult(JackpotSettings settings) {
            var existedSites = await gameService.GetExistedSites(settings.Id, settings.GameId, settings.OperatorId, settings.SiteNames);

            if (existedSites.Any())
            {
                var gameName = await generalGameSettingService.GetDisplayName(settings.GameId);
                var operatorName = await partnerService.GetById(settings.OperatorId);
                return UpdateJackpotSettingsResult.ExistedSiteResult(gameName, operatorName.Name, existedSites);
            }

            return null;
        }
    }
}