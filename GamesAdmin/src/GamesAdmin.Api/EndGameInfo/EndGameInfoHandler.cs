using System;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.EndGameInfo;
using MediatR;
using Sentry;

namespace GamesAdmin.Api.DailySummarize
{
    public class EndGameInfoHandler : IRequestHandler<RetrieveEndGameInfoRequest, EndGameInfoResult>
    {
        private readonly IEndGameInfoServiceFactory factory;

        public EndGameInfoHandler(IEndGameInfoServiceFactory factory)
        {
            this.factory = factory;
        }

        public async Task<EndGameInfoResult> Handle(RetrieveEndGameInfoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.GameRoundId == 0)
                {
                    return EndGameInfoError.SB1002;
                }

                if (string.IsNullOrWhiteSpace(request.SiteId))
                {
                    return EndGameInfoError.SB1003;
                }

                if (request.ObCustId == 0)
                {
                    return EndGameInfoError.SB1004;
                }


                if (!Enum.TryParse(request.GameTypeId.ToString(), out GameId gameType))
                {
                    return EndGameInfoError.SB1006;
                }

                var service = factory.GetService(gameType);
                return await service.GetAsync(request);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return EndGameInfoError.SB1000;
            }
        }
    }
}