using System;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Api.VoidGameCloud;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Database.Entities;
using MediatR;

namespace GamesAdmin.Api.VoidGame
{
    public class VoidGameHandler
        : IRequestHandler<VoidTicketRequest, VoidGameResult>
    {
        private readonly IAppSettings appSettings;

        private readonly IVoidGameService voidGameService;

        private readonly IVoidGameCloudService voidGameCloudService;

        public VoidGameHandler(IAppSettings appSettings, IVoidGameService voidGameService, IVoidGameCloudService voidGameCloudService)
        {
            this.appSettings = appSettings;
            this.voidGameService = voidGameService;
            this.voidGameCloudService = voidGameCloudService;
        }

        public async Task<VoidGameResult> Handle(VoidTicketRequest request, CancellationToken cancellationToken)
        {
            var gameId = Enum.Parse<GameId>(request.GameTypeId.ToString());

            if (appSettings.CloudSettings.ShouldCallCloudService(request.GameTypeId))
            {
                return voidGameCloudService.VoidCloudServiceTicket(request);
            }

            switch (gameId)
            {
                case GameId.Blackjack:
                    return await voidGameService.VoidBlackjackGameTicket<BlackjackRoundEntity, BlackjackBetEntity>(request);
                case GameId.Pontoon:
                    return await voidGameService.VoidBlackjackGameTicket<PontoonRoundEntity, PontoonBetEntity>(request);
                case GameId.CasinoHoldemPoker:
                case GameId.TexasHoldemPoker:
                    return voidGameService.VoidHoldemPokerTicket(request);
                default:
                    return  await voidGameService.VoidGameTicket(request);
            }
        }

    }
}