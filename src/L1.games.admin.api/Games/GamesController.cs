using System.Collections.Generic;
using L1.games.admin.core.Models;
using Microsoft.AspNetCore.Mvc;

namespace L1.games.admin.api.Games
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        [HttpGet]
        public IList<Game> Get()
           => new List<Game>
            {
                new Game(1, "Cannon", true),
                new Game(2, "Clan Castle", false),
                new Game(3, "Plants and Zoombies", false),
                new Game(4, "Corona", false)
            };
    }
}
