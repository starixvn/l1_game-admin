using System.Threading.Tasks;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.GameRoundResult.Models;

namespace GamesAdmin.Site.Features.GameRoundResult
{
    public interface IGameRoundResultService
    {
        Task<string> GetAccessUrlAsync();
    }

    public class GameRoundResultService : IGameRoundResultService
    {
        private readonly IGameRoundResultApi gameRoundResultApi;
        private readonly ResultPageConfiguration resultPageConfiguration;

        public GameRoundResultService(IGameRoundResultApi gameRoundResultApi, IAppSettings appSettings)
        {
            this.gameRoundResultApi = gameRoundResultApi;
            resultPageConfiguration = appSettings.ResultPageConfiguration;
        }

        public async Task<string> GetAccessUrlAsync()
        {
            var result = await gameRoundResultApi.GetAccessUrl(new GameRoundResultAccessUrlRequest
            {
                Auth = new Auth
                {
                    ClientId = resultPageConfiguration.ClientId,
                    ClientKey = resultPageConfiguration.ClientKey
                },
                Language = resultPageConfiguration.Language
            });

            return result.RedirectUrl;
        }
    }
}