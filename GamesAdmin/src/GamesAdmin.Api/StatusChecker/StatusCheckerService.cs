using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.GeneralGameSettings;
using GamesAdmin.Api.StatusChecker;
using GamesAdmin.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refit;
using Sentry;

namespace GamesAdmin.Api.ServiceStatus
{
    public interface IStatusServiceApi
    {
        [Post("/open-games")]
        Task<string> CheckServiceStatus([Body] CheckOpenGameRequest request);
    }

    public interface IStatusCheckerService
    {
        public Task<StatusGameResult> CheckServiceStatus(CheckServiceStatusRequest request);
    }

    public class StatusCheckerService: IStatusCheckerService
    {
        private readonly ISentryClient sentryClient;
        private readonly IStatusServiceApi statusServiceApi;
        private readonly IGeneralGameSettingService generalGameSettingService;

        public StatusCheckerService(ISentryClient sentryClient, IStatusServiceApi statusServiceApi,
            IGeneralGameSettingService generalGameSettingService)
        {
            this.sentryClient = sentryClient;
            this.statusServiceApi = statusServiceApi;
            this.generalGameSettingService = generalGameSettingService;
        }

        public async Task<StatusGameResult> CheckServiceStatus(CheckServiceStatusRequest request)
        {
            try
            {
                var gameId = await generalGameSettingService.GetGameId(request.GameName);
                var games = new[] { gameId };
                var result = await statusServiceApi.CheckServiceStatus(new CheckOpenGameRequest(request.Player, games));
                if (result.Contains("SSO FAILED !!!"))
                {
                    return  JsonConvert.DeserializeObject<StatusGameResult>(result);
                }
                var gameInfoList = DeserializeGameInfo(result);
                return ExtractGameInfo(gameInfoList);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return default;
            }
        }


        private List<Dictionary<string, dynamic>> DeserializeGameInfo(string result)
        {
            return JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(result);
        }

        private StatusGameResult ExtractGameInfo(List<Dictionary<string, dynamic>> gameInfoList)
        {

            var statusGameResult = default(StatusGameResult);

            if (gameInfoList.Count > 0 && TryGetObject(gameInfoList[0], out var data))
            {
                try
                {
                    statusGameResult = data.ToObject<StatusGameResult>();
                }
                catch (Exception ex)
                {
                    sentryClient.CaptureException(ex);
                }
            }

            return statusGameResult;
        }

        private static bool TryGetObject(Dictionary<string, dynamic> gameInfo, out JObject data)
        {
            data = null;
            var firstKey = gameInfo.Keys.FirstOrDefault();

            if (firstKey != null && gameInfo[firstKey] is JObject obj)
            {
                data = obj;
                return true;
            }

            return false;
        }
    }
}
