using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IShootingBlitzRtpApi : IBaseAuthorizationApi
    {
        [Get("/shooting_blitz_rtp")]
        Task<IList<ShootingBlitzRtpConfig>> GetAll();

        [Get("/shooting_blitz_rtp/{id}")]
        Task<ShootingBlitzRtpConfig> GetById(string id);

        [Post("/shooting_blitz_rtp")]
        Task<ShootingBlitzRtpConfig> Create([Body] ShootingBlitzRtpConfig config);

        [Put("/shooting_blitz_rtp/{id}")]
        Task<bool> Update(string id, [Body] ShootingBlitzRtpConfig config);

        [Delete("/shooting_blitz_rtp/{id}")]
        Task<bool> Delete(string id);
    }
}

