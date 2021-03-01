using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IGlobalConfigApi : IBaseAuthorizationApi
    {
        [Post("/global_configs/{name}/update_site_exclusions")]
        Task<bool> UpdateSiteExclusions(string name, string[] exclusions);

        [Get("/global_configs/get_site_exclusions")]
        Task<GlobalConfig> GetSiteExclusions();
    }
}