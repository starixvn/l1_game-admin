using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class GetByNameRequest : IRequest<GameConfig>
    {
        public GetByNameRequest(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
