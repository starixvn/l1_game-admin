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
        Task<LobbyModel> Get(string id);

        [Get("/lobby/getAll")]
        Task<IEnumerable<LobbyModel>> GetAll();

        [Post("/lobby/update")]
        Task<string> Update([Body] LobbyModel lobbyModel);

        [Post("/lobby/delete")]
        Task<bool> Delete(string id);

        [Get("/direct-lobby/get")]
        Task<DirectLobbyModel> GetDirectLobby(string id);

        [Get("/direct-lobby/getAll")]
        Task<IEnumerable<DirectLobbyModel>> GetAllDirectLobby();

        [Post("/direct-lobby/update")]
        Task<string> UpdateDirectLobby([Body] DirectLobbyModel lobbyModel);

        [Post("/direct-lobby/delete")]
        Task<bool> DeleteDirectLobby(string id);
    }


}
