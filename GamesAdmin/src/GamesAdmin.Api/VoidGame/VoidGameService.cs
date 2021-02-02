using System;
using System.Threading.Tasks;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Database.Entities;
using Refit;
using Sentry;

namespace GamesAdmin.Api.VoidGame
{
    [Headers("Authorization: Bearer")]
    public interface IVoidGameApi
    {
        [Post("/Member/VoidGame")]
        Task<VoidGameResult> VoidGameTicket([Body] VoidTicketRequest request);
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
        private readonly IVoidTicketLogger voidTicketLogger;

        public VoidGameService(ISentryClient sentryClient, IVoidGameApi voidGameApi, ITicketHandler ticketHandler, IVoidTicketLogger voidTicketLogger)
        {
            this.sentryClient = sentryClient;
            this.voidGameApi = voidGameApi;
            this.ticketHandler = ticketHandler;
            this.voidTicketLogger = voidTicketLogger;
        }

        public async Task<VoidGameResult> VoidGameTicket(VoidTicketRequest request)
        {
            try
            {
                var requestTime = DateTimeOffset.UtcNow;
                var result = await voidGameApi.VoidGameTicket(request);

                if (result.IsSuccessful())
                {
                    var moveToRm = await ticketHandler.Move<BigSmallOnesEntity, BigSmallBetEntity>(
                        request.ObCustId,
                        request.SiteId,
                        request.GameRoundId,
                        request.GameTypeId,
                        request.FreeBet);
                }

                await voidTicketLogger.Log(requestTime, request, DateTimeOffset.UtcNow, result);
                sentryClient.CaptureMessage($"Void Ticket For {request.SiteId}_{request.ObCustId}", Sentry.Protocol.SentryLevel.Fatal);

                return result;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return default;
        }
    }
}