using System;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using MediatR;
using Sentry;

namespace GamesAdmin.Api.DailySummarize
{
    public class EndGameInfoHandler : IRequestHandler<RetrieveEndGameInfoRequest, EndGameInfoResult>
    {
        private IEndGameInfoService service;

        public EndGameInfoHandler(IEndGameInfoService service)
        {
            this.service = service;
        }

        public async Task<EndGameInfoResult> Handle(RetrieveEndGameInfoRequest request, CancellationToken cancellationToken)
        {
            EndGameInfoResult result;

            try
            {
                if (request.GameRoundId == 0)
                {
                    result = EndGameInfoError.SB1002;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(request.SiteId))
                    {
                        result = EndGameInfoError.SB1003;
                    }
                    else
                    {
                        if (request.ObCustId == 0)
                        {
                            result = EndGameInfoError.SB1004;
                        }
                        else
                        {
                            result = await service.GetAsync(request);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                result = EndGameInfoError.SB1000;
            }

            return result;
        }
    }
}