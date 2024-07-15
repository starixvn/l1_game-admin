using GamesAdmin.Core.Models.Promotion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Promotions
{
    public interface IPromotionCampaignService
    {
        Task<IEnumerable<PromotionConfigModel>> GetCampaignByStatus(string status);
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
    }
}
