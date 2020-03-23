using System;
using MediatR;

namespace GamesAdmin.Api.Games.Requests
{
    public class DeleteRequest : IRequest<bool>
    {
        public DeleteRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
