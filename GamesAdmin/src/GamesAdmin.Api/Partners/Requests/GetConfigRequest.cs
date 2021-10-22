using MediatR;

namespace GamesAdmin.Api.Partners.Requests
{
    public class GetConfigRequest : IRequest<string>
    {
        public GetConfigRequest(string partnerId, string name, int gameId)
        {
            PartnerId = partnerId;
            Name = name;
            GameId = gameId;
        }

        public string PartnerId { get; }
        public string Name { get; }
        public int GameId { get; }
    }
}