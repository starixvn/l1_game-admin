using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.Promotions
{
    public interface IPromotionService
    {
        Task<IEnumerable<PromotionModel>> GetByStatus(string status);
        Task<IEnumerable<PromotionModel>> GetByGameId(GameId gameId);
        Task<PromotionModel> GetById(string id);
        Task<IEnumerable<PromotionModel>> GetByIds(IEnumerable<string> ids);
        Task<bool> Create(PromotionModel model);
        Task<bool> Update(PromotionModel model);
        Task<bool> Delete(string id);
        Task<IEnumerable<Notification>> GetAnnouncementByStatus(string status, string type);
        Task<bool> CreateAnnouncements(IEnumerable<Notification> models);
        Task<Notification> GetAnnouncementById(string id);
        Task<bool> UpdateAnnouncement(Notification model);
        Task<bool> DeleteAnnouncement(string id);
    }

    public class PromotionService : IPromotionService
    {
        private readonly IPromotionApi promotionApi;
        private readonly IMapper mapper;

        public PromotionService(IPromotionApi promotionApi, IMapper mapper)
        {
            this.promotionApi = promotionApi;
            this.mapper = mapper;
        }

        public Task<IEnumerable<PromotionModel>> GetByStatus(string status)
        => promotionApi.GetByStatus(status);

        public Task<IEnumerable<PromotionModel>> GetByGameId(GameId gameId)
        => promotionApi.GetByGameId(gameId);

        public Task<bool> Create(PromotionModel model)
        => promotionApi.Create(model);

        public Task<PromotionModel> GetById(string id)
        => promotionApi.GetById(id);

        public Task<IEnumerable<PromotionModel>> GetByIds(IEnumerable<string> ids)
        => promotionApi.GetByIds(ids.ToArray());

        public Task<bool> Update(PromotionModel model)
        => promotionApi.Update(model);

        public Task<bool> Delete(string id)
        => promotionApi.Delete(id);

        public Task<IEnumerable<Notification>> GetAnnouncementByStatus(string status, string type)
        => promotionApi.GetAnnouncementByStatus(status, type);

        public Task<bool> CreateAnnouncements(IEnumerable<Notification> models)
        => promotionApi.CreateAnnouncements(models);

        public Task<Notification> GetAnnouncementById(string id)
        => promotionApi.GetAnnouncementById(id);

        public Task<bool> UpdateAnnouncement(Notification model)
        => promotionApi.UpdateAnnouncement(model);

        public Task<bool> DeleteAnnouncement(string id)
        => promotionApi.DeleteAnnouncement(id);
    }
}
