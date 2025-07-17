using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.Requests
{
    public class GetGuaranteedFeatureRequest : IRequest<GuaranteedFeatureViewModel>
    {
        public GetGuaranteedFeatureRequest(string groupId, int gameId)
        {
            GroupId = groupId;
            GameId = gameId;
        }

        public string GroupId { get; }
        public int GameId { get; }
    }
}