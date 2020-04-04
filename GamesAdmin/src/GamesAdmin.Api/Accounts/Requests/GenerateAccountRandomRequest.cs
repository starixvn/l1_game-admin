using GamesAdmin.Api.Accounts.Results;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.Accounts.Requests
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
