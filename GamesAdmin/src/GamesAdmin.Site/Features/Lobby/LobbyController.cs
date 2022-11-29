using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.Lobby
{
    [Authorize]
    public class LobbyController : Controller
    {
        public IActionResult Index()
        {
            return View("Management");
        }

        public IActionResult Creat()
        {
            return View("Lobby");
        }

        [Route("[controller]/[action]/{id}")]
        public IActionResult Update([FromRoute] string id)
        {
            return View("Lobby", id);
        }
    }
}