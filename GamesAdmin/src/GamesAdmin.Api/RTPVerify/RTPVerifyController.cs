using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.RTPVerify.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.RTPVerify
{
    [Route("api/rtp_verify")]
    public class RTPVerifyController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public RTPVerifyController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("getSlotRtp")]
        public async Task<RtpResult> GetSlotRTP([FromBody] GetSlotRTPRequest request)
        => (await mediator.Send(request));
    }
}