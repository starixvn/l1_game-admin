using System;
using System.Threading.Tasks;
using GamesAdmin.Api.RTPVerify.Requests;
using GamesAdmin.Core.Models;
using Newtonsoft.Json;
using Refit;
using Sentry;

namespace GamesAdmin.Api
{
    public interface IRTPServiceApi
    {
        [Post("/slot-rtp")]
        Task<string> GetSlotRTP([Body] GetSlotRTPRequest request);
    }

    public interface IRTPVerifyService
    {
        public Task<RtpResult> GetSlotRTP(GetSlotRTPRequest request);
    }

    public class RTPVeirfyService : IRTPVerifyService
    {
        private readonly ISentryClient sentryClient;
        private readonly IRTPServiceApi rtpServiceApi;

        public RTPVeirfyService(ISentryClient sentryClient, IRTPServiceApi rtpServiceApi)
        {
            this.sentryClient = sentryClient;
            this.rtpServiceApi = rtpServiceApi;
        }

        public async Task<RtpResult> GetSlotRTP(GetSlotRTPRequest request)
        {
            try
            {
                var result = await rtpServiceApi.GetSlotRTP(request);
                return JsonConvert.DeserializeObject<RtpResult>(result);

            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return default;
            }
        }
    }
}