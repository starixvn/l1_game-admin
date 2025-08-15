using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.BuyFreeSpins.Requests;
using GamesAdmin.Site.Features.BuyFreeSpins.ViewModels;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.BuyFreeSpins
{
    public interface IBuyFreeSpinApi : IBaseAuthorizationApi
    {
        [Get("/buyFreeSpin/all")]
        Task<IEnumerable<BuyFreeSpinSettingViewModel>> GetAll();

        [Get("/buyFreeSpin/{gameId}")]
        Task<BuyFreeSpinSettingViewModel> GetById(int gameId);

        [Post("/buyFreeSpin")]
        Task<bool> Create(CreateBuyFreeSpinRequest model);

        [Put("/buyFreeSpin")]
        Task<bool> Update(UpdateBuyFreeSpinRequest model);

        [Put("/buyFreeSpin/enable")]
        Task<bool> Enable(EnableBuyFreeSpinRequest model);
    }
}