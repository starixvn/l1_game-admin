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

        public VoidGameService(ISentryClient sentryClient, IVoidGameApi voidGameApi, ITicketHandler ticketFinder)
        {
            this.sentryClient = sentryClient;
            this.voidGameApi = voidGameApi;
            this.ticketHandler = ticketFinder;
        }

        public async Task<VoidGameResult> VoidGameTicket(VoidTicketRequest request)
        {
            try
            {
                var result = await voidGameApi.VoidGameTicket(request);

                if (result.IsSuccessful())
                {
                    var tickets = await ticketHandler.Find<BigSmallOnesEntity, BigSmallBetEntity>(
                        request.ObCustId, 
                        request.SiteId, 
                        request.GameRoundId, 
                        request.GameTypeId, 
                        request.FreeBet);

                    var moveToRm = await ticketHandler.Remove<BigSmallOnesEntity, BigSmallBetEntity>(
                        request.ObCustId,
                        request.SiteId,
                        request.GameRoundId,
                        request.GameTypeId,
                        request.FreeBet);
                }

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