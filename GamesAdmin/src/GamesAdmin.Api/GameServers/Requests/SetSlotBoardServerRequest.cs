using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetSlotBoardServerRequest : IRequest<bool>
    {
        public SetSlotBoardServerRequest(string name, IEnumerable<byte> result)
        {
            this.name = name;
            this.results = result.ToList();
        }


        public string name { get; }

        public IEnumerable<byte> results { get; }
    }
}