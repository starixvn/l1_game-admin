using GamesAdmin.Core.Models.Lobby;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var result = this.lobbyService.GetAll();
            return View("Management");
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