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
        public async Task<IActionResult> GetLobbyConfig()
        {
            var result = await this.lobbyService.GetLobbyConfig(string.Empty);

            return Json(result);
        }


        public IActionResult Create()
        {
            var lobbyEntity = new LobbyModel
            {
                Name = "INH Lobby",
                CurrencyGroupIds = new string[] { "group1", "group2" },
                MiniPanelGameIds = new int[] { 1, 3, 5 },
                Categories = new LobbyCategory[]
                {
                    new LobbyCategory
                    {
                        Name = new CategoryName
                        {
                            EN_US = "All games"
                        },
                        GameSettings = new GameSetting[]
                        {
                            new GameSetting
                            {
                                GameId = 1,
                                BigSize = true,
                                NewTag = false
                            },
                            new GameSetting
                            {
                                GameId = 2,
                                BigSize = true,
                                NewTag = true
                            }
                        }
                    },
                    new LobbyCategory
                    {
                        Name = new CategoryName
                        {
                            EN_US = "Sedie"
                        },
                        GameSettings = new GameSetting[]
                        {
                            new GameSetting
                            {
                                GameId = 2,
                                BigSize = true,
                                NewTag = false
                            },
                            new GameSetting
                            {
                                GameId = 3,
                                BigSize = true,
                                NewTag = true
                            }
                        }
                    }
                }


            };
            this.lobbyService.Update(lobbyEntity);

            return View("Lobby");
        }

        [Route("[controller]/[action]/{id}")]
        public IActionResult Update([FromRoute] string id)
        {
            return View("Lobby", id);
        }
    }
}