using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.RTPVerify.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api
{
    public class RTPVerifyHandler : IRequestHandler<GetSlotRTPRequest, RtpResult>
    {
        private readonly IRTPVerifyService rtpVerifyService;

        public RTPVerifyHandler(IRTPVerifyService rtpVerifyService)
        {
            this.rtpVerifyService = rtpVerifyService;
        }

        public Task<RtpResult> Handle(GetSlotRTPRequest request, CancellationToken cancellationToken)
        => rtpVerifyService.GetSlotRTP(request);
    }
}