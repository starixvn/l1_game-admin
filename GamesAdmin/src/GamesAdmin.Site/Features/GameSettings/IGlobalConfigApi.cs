using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IGlobalConfigApi : IBaseAuthorizationApi
    {
        [Get("/global_configs/get_site_exclusions")]
        Task<GlobalConfig> GetSiteExclusions();

        [Get("/global_configs/get_super_players")]
        Task<GlobalConfig> GetSuperPlayers();

        [Post("/global_configs/{name}/update_site_exclusions")]
        Task<bool> UpdateSiteExclusions(string name, string[] exclusions);

        [Post("/global_configs/update_super_players")]
        Task<bool> UpdateSuperPlayers(string[] superPlayers);
    }
}