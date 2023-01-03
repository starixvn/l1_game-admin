using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IGlobalConfigApi : IBaseAuthorizationApi
    {
        [Get("/global_configs/get_exchange_codes")]
        Task<GlobalConfig> GetExchangeCodes();
    }
}