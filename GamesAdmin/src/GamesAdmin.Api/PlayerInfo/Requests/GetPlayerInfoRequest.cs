namespace GamesAdmin.Api.PlayerInfo.Requests
{
    using System.Collections.Generic;
    using GamesAdmin.Core.Models;
    using MediatR;

    public class GetPlayerInfoRequest : IRequest<IEnumerable<PlayerInfoModel>>
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
