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
    }
}
