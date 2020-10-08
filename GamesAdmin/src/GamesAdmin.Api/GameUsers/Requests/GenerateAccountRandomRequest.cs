using GamesAdmin.Api.GameUser.Results;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.GameUser.Requests
{
    public class GenerateAccountRandomRequest : IRequest<IList<GenerateAccountResult>>
    {
        public GenerateAccountRandomRequest(string[] names, bool isBot) 
        {
            Names = names;
            IsBot = isBot;
        }

        public string[] Names { get; }

        public bool IsBot { get; }
    }
}
