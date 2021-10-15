using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Api.Currencies.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<CurrencyModel>>
    {

    }
}