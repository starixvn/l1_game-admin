using GamesAdmin.Api._Shared;
using GamesAdmin.Core.Models.Lobby;
using Microsoft.AspNetCore.Mvc;
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
        public Task<string> Update([FromBody] LobbyModel lobbyModel)
        {
            return this.lobbyService.CreateOrUpdate(lobbyModel);
        }

        [HttpPost("delete")]
        public Task<bool> Delete(string id)
        {
            return this.lobbyService.Delete(id);
        }

        [HttpGet("get")]
        public Task<LobbyModel> Get(string id)
        {
            return this.lobbyService.Get(id);
        }

        [HttpGet("getAll")]
        public Task<IEnumerable<LobbyModel>> GetAll()
        {
            return this.lobbyService.GetAll();
        }

        [HttpGet("getRoundTimerGame")]
        public Task<Game[]> GetRoundTimerGame()
        {
            return this.lobbyService.UniqueRoundTimerGameAllMarket();
        }
    }
}
