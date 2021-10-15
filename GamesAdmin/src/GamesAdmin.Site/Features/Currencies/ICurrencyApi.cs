using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Currencies.Models;
using Refit;

namespace GamesAdmin.Site.Features.Currencies
{
    public interface ICurrencyApi : IBaseAuthorizationApi
    {
        [Get("/currency/all")]
        Task<IEnumerable<CurrencyModel>> GetAll();

        [Get("/currency/partner")]
        Task<IEnumerable<CurrencyModel>> GetByPartner([Query] GetCurrencyByPartnerModel model);

        [Post("/currency")]
        Task<bool> Create(CurrencyModel model);

        [Put("/currency")]
        Task<bool> Update(CurrencyModel model);
    }
}