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
            this.result = result.ToList();
        }


        public string name { get; }

        public IEnumerable<byte> result { get; }
    }
}