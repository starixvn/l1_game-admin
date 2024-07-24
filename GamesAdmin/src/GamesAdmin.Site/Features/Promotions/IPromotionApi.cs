using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Promotions.Requests;
using Refit;

namespace GamesAdmin.Site.Features.Promotions
{
    public interface IPromotionApi : IBaseAuthorizationApi
    {
        [Get("/promotion/getByStatus")]
        Task<IEnumerable<PromotionModel>> GetByStatus(string status);

        [Get("/promotion/getByGameId")]
        Task<IEnumerable<PromotionModel>> GetByGameId(GameId gameId);

        [Post("/promotion/create")]
        Task<bool> Create(PromotionModel model);

        [Get("/promotion/getById")]
        Task<PromotionModel> GetById(string id);

        [Post("/promotion/update")]
        Task<bool> Update(PromotionModel model);

        [Post("/promotion/delete")]
        Task<bool> Delete(string id);

        [Get("/promotion/getAnnouncementByStatus")]
        Task<IEnumerable<Notification>> GetAnnouncementByStatus(string status);

        [Post("/promotion/createAnnouncement")]
        Task<bool> CreateAnnouncements(IEnumerable<Notification> models);

        [Get("/promotion/getAnnouncementById")]
        Task<Notification> GetAnnouncementById(string id);

        [Post("/promotion/updateAnnouncement")]
        Task<bool> UpdateAnnouncement(Notification model);

        [Post("/promotion/deleteAnnouncement")]
        Task<bool> DeleteAnnouncement(string id);

        [Get("/promotion/getCampaignByStatus")]
        Task<IEnumerable<PromotionConfigModel>> GetCampaignByStatus(string status);

        [Get("/promotion/getCampaignById")]
        Task<PromotionConfigModel> GetCampaignById(string id);

        [Post("/promotion/award")]
        Task<OWResult> Award(AwardWinningPlayerRequest request);

        [Post("/promotion/createTournament")]
        Task<bool> CreateTournament(PromotionConfigModel model);

        [Get("/promotion/leaderboard")]
        Task<IEnumerable<AwardWinningPlayer>> GetLeaderboard(int promotionId, string playerName, bool isTestCurrency);

        [Post("/promotion/awardTournament")]
        Task<bool> AwardTournament(AwardTournamentRequest request);

        [Post("/promotion/updateTournament")]
        Task<bool> updateTournament(PromotionConfigModel model);
    }
}