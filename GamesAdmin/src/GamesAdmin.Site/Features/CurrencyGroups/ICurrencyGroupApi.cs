using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.CurrencyGroups.Models;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using Refit;

namespace GamesAdmin.Site.Features.CurrencyGroups
{
    public interface ICurrencyGroupApi : IBaseAuthorizationApi
    {
        [Get("/currency_group/all")]
        Task<IEnumerable<CurrencyGroupModel>> GetAll();

        [Get("/currency_group/{id}")]
        Task<CurrencyGroupModel> GetById(string id);

        [Post("/currency_group/filter")]
        Task<IEnumerable<CurrencyGroupModel>> Query(string ids);

        [Post("/currency_group")]
        Task<bool> Create(CreateCurrencyGroupModel model);

        [Put("/currency_group")]
        Task<bool> Update(UpdateCurrencyGroupModel model);

        [Get("/currency_group/partner/{partnerId}")]
        Task<IEnumerable<CurrencyGroupModel>> GetByPartner(string partnerId);

        [Post("/currency_group/clone")]
        Task<CurrencyGroupModel> Clone(CloneCurrencyGroupModel model);
    }
}