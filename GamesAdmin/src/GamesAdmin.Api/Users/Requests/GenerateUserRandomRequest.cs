using GamesAdmin.Api.Users.Results;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.Users.Requests
{
    public class GenerateUserRandomRequest : IRequest<IList<GenerateBotResult>>
    {
        public GenerateUserRandomRequest(string[] names, bool isBot) 
        {
            Names = names;
            IsBot = isBot;
        }

        public string[] Names { get; }

        public bool IsBot { get; }
    }
}
