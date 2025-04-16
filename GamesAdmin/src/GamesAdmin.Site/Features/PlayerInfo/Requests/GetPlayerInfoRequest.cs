namespace GamesAdmin.Site.Features.PlayerInfo.Requests
{
    using ViewModels;
    using MediatR;
    public class GetPlayerInfoRequest : IRequest<IndexViewModel>
    {
        public GetPlayerInfoRequest(string playerId, string name)
        {
            PlayerId = playerId;
            Name = name;
        }

        public string PlayerId { get; }

        public string Name { get; }
    }
}
