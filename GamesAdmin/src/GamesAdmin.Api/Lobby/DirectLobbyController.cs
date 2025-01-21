using GamesAdmin.Api._Shared;
using GamesAdmin.Core.Models.Lobby;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Lobby
{
    [Route("/api/direct-lobby")]

    public class DirectLobbyController : BaseAuthorizeController
    {
        private readonly ILobbyService lobbyService;

        public DirectLobbyController(ILobbyService lobbyService)
        {
            this.lobbyService = lobbyService;
        }

        [HttpPost("update")]
        public Task<string> Update([FromBody] DirectLobbyModel lobbyModel)
        {
            return this.lobbyService.CreateOrUpdateDirectLobby(lobbyModel);
        }

        [HttpPost("delete")]
        public Task<bool> Delete(string id)
        {
            return this.lobbyService.DeleteDirectLobby(id);
        }

        [HttpGet("get")]
        public Task<DirectLobbyModel> Get(string id)
        {
            return this.lobbyService.GetDirectLobby(id);
        }

        [HttpGet("getAll")]
        public Task<IEnumerable<DirectLobbyModel>> GetAll()
        {
            return this.lobbyService.GetAllDirectLobby();
        }
    }
}
