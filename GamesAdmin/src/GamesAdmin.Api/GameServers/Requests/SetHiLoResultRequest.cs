using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetHiLoResultRequest : IRequest<IEnumerable<PokerCard>>
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