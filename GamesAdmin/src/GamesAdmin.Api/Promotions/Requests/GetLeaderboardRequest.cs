using System.Collections.Generic;
using GamesAdmin.Core.Models.Promotion;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class GetLeaderboardRequest : IRequest<IEnumerable<AwardWinningPlayer>>
    {
        public GetLeaderboardRequest(int promotionId, string playerName = "", bool isTestCurrency = false)
        {
            PromotionId = promotionId;
            IsTestCurrency = isTestCurrency;
            PlayerName = playerName;
        }

        public int PromotionId { get; }
        public bool IsTestCurrency { get; }
        public string PlayerName { get; }
    }
}