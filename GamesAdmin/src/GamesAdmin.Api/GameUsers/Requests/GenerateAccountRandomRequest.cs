using GamesAdmin.Api.GameUser.Results;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.GameUser.Requests
{
    public class GenerateAccountRandomRequest : IRequest<IList<GenerateAccountResult>>
    {
        public GenerateAccountRandomRequest(string[] names) 
        {
            Names = names;
        }

        public string[] Names { get; }
    }
}
