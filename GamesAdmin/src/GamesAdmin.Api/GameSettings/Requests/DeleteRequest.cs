using System;
using MediatR;

namespace GamesAdmin.Api.Games.Requests
{
    public class DeleteRequest : IRequest<bool>
    {
        public DeleteRequest(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
