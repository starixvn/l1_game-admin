using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroupGames.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<CurrencyGroupGame>>
    {
    }
}