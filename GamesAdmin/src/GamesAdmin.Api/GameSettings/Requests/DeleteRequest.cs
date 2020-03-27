using System;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
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
