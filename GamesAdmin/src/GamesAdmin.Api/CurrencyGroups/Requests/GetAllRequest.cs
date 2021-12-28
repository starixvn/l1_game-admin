using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroups.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<CurrencyGroupModel>>
    {
    }
}