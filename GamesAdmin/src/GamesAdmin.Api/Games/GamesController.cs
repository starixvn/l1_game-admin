using System.Collections.Generic;
using GamesAdmin.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Games
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService gamesService;

        public GamesController(IGameService gamesService)
        {
            this.gamesService = gamesService;
        }

        [HttpGet]
        public IList<Game> Get()
           => gamesService.Get();

        [HttpGet("{id:length(24)}", Name = "GetGame")]
        public Game Get(string id)
            => gamesService.Get(id);

        [HttpPost]
        public bool Create(Game game)
            => gamesService.Create(game);

        [HttpPut("{id:length(24)}")]
        public bool Update(string id, Game game)
        {
            var book = gamesService.Get(id);

            if (book == null)
            {
                return false;
            }

            gamesService.Update(id, game);

            return true;
        }
    }
}
