using System;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Database.Entities;
using MediatR;

namespace GamesAdmin.Api.VoidGame
{
    public class VoidGameHandler
        : IRequestHandler<VoidTicketRequest, VoidGameResult>
    {
        private readonly IVoidGameService voidGameService;

        public VoidGameHandler(IVoidGameService voidGameService)
        {
            this.voidGameService = voidGameService;
        }

        public async Task<VoidGameResult> Handle(VoidTicketRequest request, CancellationToken cancellationToken)
        {
            var gameId = Enum.Parse<GameId>(request.GameTypeId.ToString());
            switch (gameId)
            {
                case GameId.Blackjack:
                    return await voidGameService.VoidBlackjackGameTicket<BlackjackRoundEntity, BlackjackBetEntity>(request);
                case GameId.Pontoon: 
                    return await voidGameService.VoidBlackjackGameTicket<PontoonRoundEntity, PontoonBetEntity>(request);
                case GameId.CasinoHoldemPoker:
                case GameId.TexasHoldemPoker: return await voidGameService.VoidHoldemPokerTicket(request);
                default:
                    return  await voidGameService.VoidGameTicket(request);
            }          
        }

    }
}