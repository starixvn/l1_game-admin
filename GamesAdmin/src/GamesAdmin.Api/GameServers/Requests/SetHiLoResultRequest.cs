using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetHiLoResultRequest : IRequest<IEnumerable<Card>>
    {
        public SetHiLoResultRequest(string name, IEnumerable<string> result)
        {
            Name = name;
            Result = result;
        }

        public string Name { get; }

        public IEnumerable<string> Result { get; }
    }
}