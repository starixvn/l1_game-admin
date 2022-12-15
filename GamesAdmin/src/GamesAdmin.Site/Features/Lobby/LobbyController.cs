using GamesAdmin.Core.Models.Lobby;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Lobby
{
    [Authorize]
    public class LobbyController : Controller
    {
        private readonly ILobbyService lobbyService;

        public LobbyController(ILobbyService lobbyService)
        {
            this.lobbyService = lobbyService;
        }

        [Route("lobby")]
        [Route("lobby/add")]
        [Route("lobby/edit/{id}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/lobby/GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await this.lobbyService.GetAll();

            return Json(result);
        }

        [HttpGet]
        [Route("/lobby/GetLobbyConfig")]
        public async Task<IActionResult> GetLobbyConfig(string id)
        {
            var result = await this.lobbyService.GetLobbyConfig(id);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromBody] LobbyModel lobbyModel)
        {
            var result = await this.lobbyService.Update(lobbyModel);

            return Json(result);
        }

        [HttpPost]
        [Route("/lobby/delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await this.lobbyService.Delete(id);

            return Json(result);
        }
    }
}