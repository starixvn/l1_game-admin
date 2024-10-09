using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.GameGroupManagement
{
    public interface IGameGroupManagementApi : IBaseAuthorizationApi
    {
        [Get("/gamegroup/all")]
        Task<IEnumerable<GameGroup>> GetAll();

        [Get("/gamegroup/{groupId}")]
        Task<GameGroup> GetByGroupId(string groupId);

        [Get("/gamegroup/getbygameid")]
        Task<GameGroup> GetByGameId(int gameId);

        [Post("/gamegroup")]
        Task<bool> Create(GameGroup model);

        [Put("/gamegroup")]
        Task<bool> Update(GameGroup model);
    }
}