using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class GetByNameRequest : IRequest<AutoRNGGameConfig>
    {
        public GetByNameRequest(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
