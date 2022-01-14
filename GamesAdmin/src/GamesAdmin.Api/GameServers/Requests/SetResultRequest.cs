using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetResultRequest : IRequest<bool>
    {
        public SetResultRequest(string name, IEnumerable<string> result)
        {
            Name = name;
            Result = result;
        }

        public string Name { get; }

        public IEnumerable<string> Result { get; }
    }
}