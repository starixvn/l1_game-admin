using GamesAdmin.Api.VoidGame.Models;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.OWResults;
using Refit;
using Sentry;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        Task<VoidGameResult> VoidBlackjackTicket(VoidTicketRequest request);

        Task<VoidGameResult> VoidHoldemPokerTicket(VoidTicketRequest request);
    }

    public class VoidGameService : IVoidGameService
    {
        private readonly ISentryClient sentryClient;

        private readonly IVoidGameApi voidGameApi;

        private readonly ITicketHandler ticketHandler;

        private readonly IVoidRunningTicketApiFactory voidRunningTicketApiFactory;

        private readonly Dictionary<GameId, string> gameRound24h = new Dictionary<GameId, string>
        {
            { GameId.Blackjack, "blackjack"},
            { GameId.CasinoHoldemPoker, "casino_holdem_poker"},
            { GameId.TexasHoldemPoker, "texas_holdem_poker"}
        };

        public VoidGameService(ISentryClient sentryClient, IVoidGameApi voidGameApi, ITicketHandler ticketHandler,
            IVoidRunningTicketApiFactory voidRunningTicketApiFactory)
        {
            this.sentryClient = sentryClient;
            this.voidGameApi = voidGameApi;
            this.ticketHandler = ticketHandler;
            this.voidRunningTicketApiFactory = voidRunningTicketApiFactory;
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
                    return NotExistTicket();
                }

                var requestTime = DateTimeOffset.UtcNow;
                return await VoidGame(request);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return default;
        }

        public async Task<VoidGameResult> VoidHoldemPokerTicket(VoidTicketRequest request)
        {
            try
            {
                var ticket = await ticketHandler.ExistHoldemPoker(request.PlayerId,
                    request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.GameTypeId,
                    request.FreeBet);
                var isExistTicket = ticket.Exist();
                if (!isExistTicket)
                {
                    return NotExistTicket();
                }
                var isRunningTicket = !ticket.BetRecords[0].Paid;
                var uid = ticket.BetRecords[0].Uid;
                var apiResult = await VoidGame(request);

                if (apiResult.IsFailed())
                {
                    return apiResult;
                }

                if (isRunningTicket)
                {
                    var isVoid = await VoidRunningTicket(request.GameTypeId, uid);
                    if (!isVoid)
                    {
                        return new VoidGameResult { ErrorCode = 1002, ErrorMessage = "Request void running ticket unsuccessfully." };
                    }
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return default;
        }

        public async Task<VoidGameResult> VoidBlackjackTicket(VoidTicketRequest request)
        {
            try
            {
                var ticket = await ticketHandler.ExistBlackjackTicket(request.PlayerId,
                    request.OperatorId,
                    request.SiteId,
                    request.GameRoundId,
                    request.FreeBet);
                var isExistTicket = ticket.Exist();
                if (!isExistTicket)
                {
                    return NotExistTicket();
                }
                var uid = ticket.BetRecord.Uid;
                VoidGameResult apiResult;
                if (!ticket.IsRunningTicket)
                {
                    apiResult = await VoidGame(request);
                    if (apiResult.IsFailed())
                    {
                        return apiResult;
                    }

                }
                else
                {
                    apiResult = await VoidAllTicket(ticket.HandIds, request);
                    if (apiResult.IsFailed())
                    {
                        return apiResult;
                    }               
                }

                if (ShouldMoveRound(ticket))
                {
                    var movedRound = await ticketHandler.MoveRoundBlackjack(ticket.RoundId);
                    if (!movedRound)
                    {
                        return TicketMovedUnsuccessfully();
                    }               
                }

                if (ticket.IsRunningTicket)
                {
                    var isVoid = await VoidRunningTicket(request.GameTypeId, uid);
                    if (!isVoid)
                    {
                        return new VoidGameResult { ErrorCode = 1002, ErrorMessage = "Request void running ticket unsuccessfully." };
                    }
                  
                }

                apiResult.HandIds = String.Join(", ", ticket.HandIds);
                apiResult.IsRunningTicket = ticket.IsRunningTicket;
                return apiResult;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return default;
        }

        private static bool ShouldMoveRound(BlackjackRecord ticket)
        {
            return ticket.IsRunningTicket || ticket.HandIds.Count == 1;
        }

        private async Task<VoidGameResult> VoidAllTicket(List<long> tickets, VoidTicketRequest request)
        {
            foreach (var ticket in tickets)
            {
                request.GameRoundId = ticket;
                var apiResult = await VoidGame(request);
                if (apiResult.IsFailed())
                {
                    return apiResult;
                }
            }
            return new VoidGameResult();
        }

        private async Task<VoidGameResult> VoidGame(VoidTicketRequest request)
        {
            try
            {
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
                        return TicketMovedUnsuccessfully();
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

        private VoidGameResult TicketMovedUnsuccessfully()
        {
            return new VoidGameResult { ErrorCode = 1000, ErrorMessage = "Tickets was moved unsuccessfully." };
        }

        private VoidGameResult NotExistTicket()
        {
            return new VoidGameResult { ErrorCode = 1001, ErrorMessage = "No such ticket." };
        }

        public async Task<bool> VoidRunningTicket(int gameTypeId, string userIndentify)
        {
            try
            {
                var gameId = Enum.Parse<GameId>(gameTypeId.ToString());
                if (gameRound24h.TryGetValue(gameId, out var serviceName))
                {
                    var voidRunningTicketApi = voidRunningTicketApiFactory.GetVoidRunningTicketApi(serviceName);
                    var result = await voidRunningTicketApi.VoidTicket(new VoidRunningTicketRequest(userIndentify));
                    return result;
                }             
            }
            catch (ApiException apiEx)
            {                        
                sentryClient.CaptureMessage($"Request {apiEx.Uri} error: {apiEx.Message}");                             
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }
            return true;
        }
    }
}
