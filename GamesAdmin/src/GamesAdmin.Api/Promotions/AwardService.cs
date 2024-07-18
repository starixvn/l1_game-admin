using System;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Core.OWResults;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refit;
using Sentry;
using Sentry.Protocol;

namespace GamesAdmin.Api.Promotions
{
    [Headers("Authorization: Bearer")]
    public interface IAwardGameApi
    {
        [Post("/Player/Award")]
        Task<AwardApiResult> Award([Body] AwardWinningPlayer request);
    }


    public interface IAwardService
    {
        Task<OWResult> AwardWinningPlayer(AwardWinningPlayer awardWinningPlayer);
    }

    public class AwardService : IAwardService
    {
        private const int AwardFailedErrorCode = 1;
        private readonly IAwardGameApi awardGameApi;
        private readonly ISentryClient sentryClient;

        public AwardService(IAwardGameApi awardGameApi, ISentryClient sentryClient)
        {
            this.awardGameApi = awardGameApi;
            this.sentryClient = sentryClient; 
        }

        public async Task<OWResult> AwardWinningPlayer(AwardWinningPlayer awardWinningPlayer)
        {
            try
            {
                var apiResult = await awardGameApi.Award(awardWinningPlayer);

                if (apiResult.Result.IsSuccessful())
                {
                    sentryClient.CaptureMessage($"Award Winning Player: {Newtonsoft.Json.JsonConvert.SerializeObject(awardWinningPlayer)}", SentryLevel.Fatal);
                    
                }

                return apiResult.Result;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return new OWResult { ErrorCode = AwardFailedErrorCode };
        }
    }
}
