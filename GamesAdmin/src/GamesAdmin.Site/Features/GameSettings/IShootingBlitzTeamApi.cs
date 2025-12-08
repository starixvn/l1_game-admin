using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IShootingBlitzTeamApi : IBaseAuthorizationApi
    {
        [Get("/shooting_blitz_team")]
        Task<IList<ShootingBlitzTeam>> GetAll();

        [Get("/shooting_blitz_team/{id}")]
        Task<ShootingBlitzTeam> GetById(string id);

        [Post("/shooting_blitz_team")]
        Task<ShootingBlitzTeam> Create([Body] ShootingBlitzTeam team);

        [Put("/shooting_blitz_team/{id}")]
        Task<bool> Update(string id, [Body] ShootingBlitzTeam team);

        [Delete("/shooting_blitz_team/{id}")]
        Task<bool> Delete(string id);
    }
}

