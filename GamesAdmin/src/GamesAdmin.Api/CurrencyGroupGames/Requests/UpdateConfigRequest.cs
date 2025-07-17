using MediatR;

namespace GamesAdmin.Api.CurrencyGroupGames.Requests
{
    public class UpdateConfigRequest : IRequest<bool>
    {
        public UpdateConfigRequest(string groupId, int gameId, object configValue, string type)
        {
            GroupId = groupId;
            GameId = gameId;
            ConfigValue = configValue;
            Type = type;
        }

        public string GroupId { get; }
        public int GameId { get; }
        public object ConfigValue { get; }
        public string Type { get; }
    }
} 