using GamesAdmin.Core.OWResults;
using GamesAdmin.Site.Features.VoidGame.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.VoidGame
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class InternalController : ControllerBase
    {
        private readonly IVoidGameApi voidGameApi;

        public InternalController(IVoidGameApi voidGameApi)
        {
            this.voidGameApi = voidGameApi;
        }

        [HttpGet]
        public string Index()
        {
            return "API from admin site";
        }

        [HttpPost("void")]
        public async Task<VoidGameResult> VoidTicket(string playerId, string operatorId, string siteId, long gameRoundId, int gameId)
        {
            const string reason = "Void for incident";
            const bool freeBet = false;
            var request = new VoidTicketRequest(playerId, operatorId, siteId, gameRoundId, gameId, reason, freeBet, false);

            return await voidGameApi.VoidTicket(request);
        }
    }
}