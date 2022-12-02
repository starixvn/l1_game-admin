using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Site.Features._Shared;
using MongoDB.Bson;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Lobby
{
    public interface ILobbyApi : IBaseAuthorizationApi
    {
        [Get("/lobby/get")]
        Task<LobbyModel> Get(ObjectId Id);

        [Get("/lobby/getAll")]
        Task<IEnumerable<LobbyModel>> GetAll();

        [Post("/lobby/update")]
        Task<bool> Update([Body] LobbyModel lobbyModel);
    }


}
