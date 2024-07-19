using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Site.Features.Promotions.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Promotions
{
    public interface IPromotionCampaignService
    {
        Task<IEnumerable<PromotionConfigModel>> GetCampaignByStatus(string status);

        Task<PromotionConfigModel> GetCampaignById(string id);

        Task<bool> Create(PromotionConfigModel model);

        Task<OWResult> Award(AwardWinningPlayerRequest request);
    }
    public class PromotionCampaignService  : IPromotionCampaignService
    {
        private readonly IPromotionApi promotionApi;

        public PromotionCampaignService(IPromotionApi promotionApi)
        {
            this.promotionApi = promotionApi;
        }

        public Task<IEnumerable<PromotionConfigModel>> GetCampaignByStatus(string status)
            => promotionApi.GetCampaignByStatus(status);

        public Task<PromotionConfigModel> GetCampaignById(string id)
            => promotionApi.GetCampaignById(id);

        public Task<OWResult> Award(AwardWinningPlayerRequest request)
             => promotionApi.Award(request);

        public Task<bool> Create(PromotionConfigModel model)
        => promotionApi.CreateTournament(model);
    }
}
