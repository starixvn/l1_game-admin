using System;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Games.Requests
{
    public class GetByIdRequest : IRequest<GameConfig>
    {
        public GetByIdRequest(Guid id) 
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
