using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.Requests
{
    public class UpdateGuaranteedFeatureRequest : IRequest<bool>
    {
        public UpdateGuaranteedFeatureRequest(string groupId, int gameId, GuaranteedFeatureViewModel configValue)
        {
            GroupId = groupId;
            GameId = gameId;
            ConfigValue = configValue;
        }

        public string GroupId { get; }
        public int GameId { get; }
        public GuaranteedFeatureViewModel ConfigValue { get; }
    }
} 