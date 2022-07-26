using System;
using System.Threading.Tasks;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Core.OWResults;
using Refit;
using Sentry;

namespace GamesAdmin.Api.VoidGame
{
    [Headers("Authorization: Bearer")]
    public interface IVoidGameApi
    {
        [Post("/Player/VoidGame")]
        Task<VoidGameApiResult> VoidGameTicket([Body] VoidTicketRequest request);
    }

    public interface IVoidGameService
    {
        Task<VoidGameResult> VoidGameTicket(VoidTicketRequest request);
    }

    public class VoidGameService : IVoidGameService
    {
        private readonly ISentryClient sentryClient;
        private readonly IVoidGameApi voidGameApi;
        private readonly ITicketHandler ticketHandler;

        public VoidGameService(ISentryClient sentryClient, IVoidGameApi voidGameApi, ITicketHandler ticketHandler)
        {
            this.sentryClient = sentryClient;
            this.voidGameApi = voidGameApi;
            this.ticketHandler = ticketHandler;
        }

        public async Task<VoidGameResult> VoidGameTicket(VoidTicketRequest request)
        {
            try
            {
                var isExistTicket = await ticketHandler.Exist(request.PlayerId,
                    request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.GameTypeId,
                    request.FreeBet);

                if (!isExistTicket)
                {
                    return new VoidGameResult { ErrorCode = 1001, ErrorMessage = "No such ticket." };
                }

                var requestTime = DateTimeOffset.UtcNow;
                var apiResult = await voidGameApi.VoidGameTicket(request);

                if (apiResult.Result.IsSuccessful())
                {
                    sentryClient.CaptureMessage($"Void Ticket For {request.SiteId}_{request.PlayerId}", Sentry.Protocol.SentryLevel.Fatal);

                    var moved = await ticketHandler.Void(
                        request.PlayerId,
                        request.OperatorId,
                        request.SiteId,
                        request.GameRoundId,
                        request.GameTypeId,
                        request.FreeBet);

                    if (!moved)
                    {
                        return new VoidGameResult { ErrorCode = 1000, ErrorMessage = "Tickets was moved unsuccessfully." };
                    }
                }

                return apiResult.Result;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return default;
        }
    }
}