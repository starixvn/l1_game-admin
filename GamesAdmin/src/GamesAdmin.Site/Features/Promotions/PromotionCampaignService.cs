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

        Task<IEnumerable<AwardWinningPlayer>> GetLeaderboard(GetLeaderboardRequest request);

        Task<IEnumerable<AwardWinningPlayer>> GetTournamentWinningPlayers(GetTournamentWinningPlayersRequest request);

        Task<bool> AwardTournament(AwardTournamentRequest request);

        Task<bool> Update(PromotionConfigModel model, string type);

        Task<bool> Delete(string id);

        Task<bool> AwardTestCurrency(AwardTestCurrencyRequest request);
    }

    public class PromotionCampaignService : IPromotionCampaignService
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

        public Task<IEnumerable<AwardWinningPlayer>> GetLeaderboard(GetLeaderboardRequest request)
             => promotionApi.GetLeaderboard(request.PromotionId, request.PlayerName, request.IsTestCurrency);

        public Task<IEnumerable<AwardWinningPlayer>> GetTournamentWinningPlayers(GetTournamentWinningPlayersRequest request)
             => promotionApi.GetTournamentWinningPlayers(request.PromotionId);

        public Task<bool> AwardTournament(AwardTournamentRequest request)
             => promotionApi.AwardTournament(request);

        public Task<bool> Update(PromotionConfigModel model, string type)
            => promotionApi.UpdateTournament(model, type);

        public Task<bool> Delete(string id)
            => promotionApi.DeleteTournament(id);

        public Task<bool> AwardTestCurrency(AwardTestCurrencyRequest request)
         => promotionApi.AwardTestCurrency(request);
    }
}