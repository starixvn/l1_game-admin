using GamesAdmin.Api._Shared;
using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Database.Entities.Lobby;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Lobby
{
    [Route("/api/lobby")]

    public class LobbyController : BaseAuthorizeController
    {
        private readonly ILobbyService lobbyService;

        public LobbyController(ILobbyService lobbyService)
        {
            this.lobbyService = lobbyService;
        }

        [HttpPost("update")]
        public Task<bool> Update([FromBody] LobbyModel lobbyModel)
        {
            return this.lobbyService.CreateOrUpdate(lobbyModel);
        }

        [HttpGet("get")]
        public Task<LobbyModel> Get(ObjectId Id)
        {
            return this.lobbyService.Get(Id);
        }

        [HttpGet("getAll")]
        public Task<IEnumerable<LobbyModel>> GetAll()
        {
            return this.lobbyService.GetAll();
        }
    }
}
