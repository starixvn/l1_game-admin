using System.Collections.Generic;
using MediatR;
using HiloCard = GamesAdmin.Core.Models.Card;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetHiLoResultRequest : IRequest<IEnumerable<HiloCard>>
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