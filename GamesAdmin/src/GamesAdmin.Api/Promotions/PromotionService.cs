using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.ChatBot;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.Promotions
{
    public interface IPromotionService
    {
        Task<IEnumerable<PromotionModel>> GetByStatus(string status);

        Task<IEnumerable<PromotionModel>> GetByGameId(GameId gameId);

        Task<bool> Create(PromotionModel model);

        Task<PromotionModel> GetById(string id);

        Task<bool> Update(PromotionModel model);

        Task<bool> Delete(string id);

        Task<IEnumerable<PromotionModel>> GetByIds(string[] ids);
    }

    public class PromotionService : IPromotionService
    {
        private readonly IGenericRepository<PromotionEntity> promotionRepository;
        private readonly IMapper mapper;
        private readonly IChatBotService chatBotService;
        private readonly INotificationService notificationService;

        public PromotionService(IGenericRepository<PromotionEntity> promotionRepository,
            IChatBotService chatBotService,
            INotificationService notificationService,
            IMapper mapper)
        {
            this.promotionRepository = promotionRepository;
            this.chatBotService = chatBotService;
            this.notificationService = notificationService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PromotionModel>> GetByStatus(string status)
        {
            List<PromotionEntity> entities;
            if (status.Equals(PromotionStatus.AllValue))
            {
                entities = (await promotionRepository.FilterByAsync(entity => !entity.Status.Equals(PromotionStatus.DeletedValue)))
                    .OrderByDescending(promotion => promotion.StartDateTime).ToList();
            }
            else
            {
                entities = (await promotionRepository.FilterByAsync(entity => entity.Status.Equals(status)))
                    .OrderByDescending(promotion => promotion.StartDateTime).ToList();
            }
            var promotions = entities.Select(entity => mapper.Map<PromotionModel>(entity)).ToList();
            for (var index = 0; index < promotions.Count; index++)
            {
                var announcement = await notificationService.GetPromotionAnnouncementByPromotionId(promotions[index].PromotionId);
                promotions[index].HasAnnouncement = announcement != null;
            }
            return promotions;
        }

        public async Task<IEnumerable<PromotionModel>> GetByGameId(GameId gameId)
        {
            var promotions = await promotionRepository
                .FilterByAsync(entity => (!entity.Status.Equals(PromotionStatus.DeletedValue) && entity.GameId == (int)gameId));
            return promotions.Select(entity => mapper.Map<PromotionModel>(entity));
        }

        public async Task<bool> Create(PromotionModel model)
        {
            var entity = mapper.Map<PromotionEntity>(model);
            await promotionRepository.InsertOneAsync(entity);
            await chatBotService.NotifyCreatePromotion(model);
            return true;
        }

        public async Task<PromotionModel> GetById(string id)
        {
            var promotion = await promotionRepository.FindByIdAsync(id);
            return mapper.Map<PromotionModel>(promotion);
        }

        public async Task<bool> Update(PromotionModel model)
        {
            var entity = await promotionRepository.FindByIdAsync(model.PromotionId);
            if (entity != null)
            {
                var update = Builders<PromotionEntity>.Update.Set(field => field.Status, model.Status)
                .Set(field => field.Odds, model.Odds)
                .Set(field => field.StartDateTime, model.StartDateTime)
                .Set(field => field.EndDateTime, model.EndDateTime)
                .Set(field => field.IsDaily, model.IsDaily);

                await promotionRepository.UpdateAsync(promotion => promotion.Id.Equals(ObjectId.Parse(model.PromotionId)), update);

                await chatBotService.NotifyUpdatePromotion(model);
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(string id)
        {
            var entity = await promotionRepository.FindByIdAsync(id);
            if (entity != null)
            {
                var update = Builders<PromotionEntity>.Update.Set(field => field.Status, PromotionStatus.Deleted.Value);
                await promotionRepository.UpdateAsync(promotion => promotion.Id.Equals(ObjectId.Parse(id)), update);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<PromotionModel>> GetByIds(string[] ids)
        {
            if (ids == null || ids.Any(id => string.IsNullOrEmpty(id)))
            {
                return Enumerable.Empty<PromotionModel>();
            }
            var objectIds = ids.Select(id => new ObjectId(id));
            var promotions = await promotionRepository.FilterByAsync(x => objectIds.Contains(x.Id));

            return mapper.Map<IEnumerable<PromotionModel>>(promotions);
        }
    }
}