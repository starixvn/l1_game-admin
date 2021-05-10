using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class GetDisabledMessageRequest : IRequest<Dictionary<string, string>>
    {
        public GetDisabledMessageRequest(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}