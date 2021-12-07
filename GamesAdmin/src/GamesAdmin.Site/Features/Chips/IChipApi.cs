using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.Chips;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.Chips
{
    public interface IChipApi : IBaseAuthorizationApi
    {
        [Get("/chip/all")]
        Task<IEnumerable<ChipModel>> GetAll();

        [Get("/chip/{id}")]
        Task<ChipModel> GetById(string id);

        [Post("/chip")]
        Task<bool> Create(ChipModel model);

        [Put("/chip")]
        Task<bool> Update(ChipModel model);
    }
}