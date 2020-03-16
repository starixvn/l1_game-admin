using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Games.Requests
{
    public class GetByIdRequest : IRequest<Game>
    {
        public GetByIdRequest(string id) 
        {
            Id = id;
        }

        public string Id { get; }
    }
}
