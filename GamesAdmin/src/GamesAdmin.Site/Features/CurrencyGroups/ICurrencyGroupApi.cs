using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.CurrencyGroups.Models;
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
        Task<IEnumerable<CurrencyGroupModel>> Query(QueryCurrencyGroupsModel model);

        [Post("/currency")]
        Task<bool> Create(CreateCurrencyGroupModel model);

        [Put("/currency")]
        Task<bool> Update(UpdateCurrencyGroupModel model);
    }
}