using Audit.Mvc;
using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Site.Features._Shared;
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
            result.IsSuperAdmin = HttpContext.HasAdminPermission();

            return Json(result);
        }

        [HttpGet]
        [Route("/lobby/GetLobbyConfig")]
        public async Task<IActionResult> GetLobbyConfig(string id)
        {
            var result = await this.lobbyService.GetLobbyConfig(id);
            result.IsSuperAdmin = HttpContext.HasAdminPermission();

            return Json(result);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateOrUpdate([FromBody] LobbyModel lobbyModel)
        {
            var result = await this.lobbyService.Update(lobbyModel);

            return Json(result);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        [Route("/lobby/delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await this.lobbyService.Delete(id);

            return Json(result);
        }

        [Route("direct-lobby")]
        public IActionResult DirectLobby(string id)
        {
            return View("DirectLobby");
        }

        [Route("direct-lobby/add")]
        [Route("direct-lobby/edit/{id}")]
        public IActionResult CreatOrUpdateDirectLobby(string id)
        {
            return View("DirectLobby");
        }

        [HttpGet]
        [Route("/direct-lobby/GetAll")]
        public async Task<IActionResult> GetAllDirectLobby()
        {
            var result = await this.lobbyService.GetAllDirectLobby();
            result.IsSuperAdmin = HttpContext.HasAdminPermission();

            return Json(result);
        }

        [HttpGet]
        [Route("/direct-lobby/Get")]
        public async Task<IActionResult> GetDirectLobbyConfig(string id)
        {
            var result = await this.lobbyService.GetDirectLobby(id);
            result.IsSuperAdmin = HttpContext.HasAdminPermission();

            return Json(result);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        [Route("/direct-lobby")]
        public async Task<IActionResult> CreateOrUpdateDirectLobby([FromBody] DirectLobbyModel model)
        {
            var result = await this.lobbyService.UpdateDirectLobby(model);

            return Json(result);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        [Route("/direct-lobby/delete")]
        public async Task<IActionResult> DeleteDirectLobby(string id)
        {
            var result = await this.lobbyService.DeleteDirectLobby(id);

            return Json(result);
        }
    }
}