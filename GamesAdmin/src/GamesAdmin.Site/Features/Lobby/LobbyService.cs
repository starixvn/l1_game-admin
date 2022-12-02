using GamesAdmin.Core.Models.Lobby;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Lobby
{
    public interface ILobbyService
    {
        Task<IEnumerable<LobbyModel>> GetAll();

        Task<bool> Update(LobbyModel lobbyModel);

        Task<LobbyModel> Get(ObjectId Id);
    }

    public class LobbyService : ILobbyService
    {
        private readonly ILobbyApi lobbyApi;

        public LobbyService(ILobbyApi lobbyApi)
        {
            this.lobbyApi = lobbyApi;
        }

        public async Task<LobbyModel> Get(ObjectId Id)
        {
            return null;
            //return this.lobbyApi.Get(Id);
        }

        public Task<IEnumerable<LobbyModel>> GetAll()
        {
            return this.lobbyApi.GetAll();
        }

        public Task<bool> Update(LobbyModel lobbyModel)
        {
            // return this.lobbyApi.Update(lobbyEntity);
            return this.lobbyApi.Update(lobbyModel);
        }
    }
}
