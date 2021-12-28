using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.ExchangeRates.Models;
using Refit;

namespace GamesAdmin.Site.Features.ExchangeRates
{
    public interface IExchangeRateApi : IBaseAuthorizationApi
    {
        [Post("/exchange_rate/query")]
        Task<IEnumerable<ExchangeRate>> Query(QueryExchangeRateModel query);
    }
}