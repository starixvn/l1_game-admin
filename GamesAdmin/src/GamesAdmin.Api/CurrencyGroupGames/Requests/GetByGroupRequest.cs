using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroupGames.Requests
{
    public class GetByGroupRequest : IRequest<IEnumerable<CurrencyGroupGame>>
    {
        public GetByGroupRequest(string currencyGroupId)
        {
            CurrencyGroupId = currencyGroupId;
        }

        public string CurrencyGroupId { get; }
    }
}