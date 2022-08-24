using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Notifications.Models;
using GamesAdmin.Api.Notifications.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.Notifications
{
    public interface INotificationService
    {
        public Task<IList<Notification>> Get();

        public Task<Notification> GetById(string id);

        public Task<bool> Create(AddRequest request);

        public Task<bool> Update(UpdateRequest notification);

        public Task<bool> Delete(string id);

        public Task<bool> AddOrUpdateForGame(UpdateRequest notification);

        public Task<Notification> GetByGameName(string gameName, NotificationDisplayKind displayKind);

        Task<bool> AddDisableMessage(string gameName, Dictionary<string, string> messages);

        Task<IEnumerable<Notification>> GetPromotionAnnouncementByStatus(string status);

        Task<Notification> GetPromotionAnnouncementByPromotionId(string promotionId);

        Task<bool> CreatePromotionAnnouncements(IEnumerable<CreateAnnouncementModel> models);

        Task<Notification> GetPromotionAnnouncementById(string id);

        Task<bool> UpdatePromotionAnnouncement(Notification model);
    }

    public class NotificationService : INotificationService
    {
        private readonly IMapper mapper;
        private readonly IGenericRepository<NotificationEntity> notificationRepository;
        private readonly ISentryClient sentryClient;

        public NotificationService(
            IMapper mapper,
            IGenericRepository<NotificationEntity> notificationRepository,
            ISentryClient sentryClient)
        {
            this.notificationRepository = notificationRepository;
            this.mapper = mapper;
            this.sentryClient = sentryClient;
        }

        public async Task<bool> Create(AddRequest request)
        {
            var notificationEntity = mapper.Map<NotificationEntity>(request);
            await notificationRepository.InsertOneAsync(notificationEntity);

            return true;
        }

        public async Task<IList<Notification>> Get()
        {
            var notifications = (await notificationRepository
                .FilterByAsync(n => n.DisplayKind != byte.Parse(NotificationDisplayKind.DisabledGame.Value) && n.DisplayKind != byte.Parse(NotificationDisplayKind.Promotion.Value)))
                .ToList();

            return notifications.Select(entity => Convert(entity)).ToList();
        }

        public async Task<Notification> GetById(string id)
        {
            var entity = await notificationRepository.FindByIdAsync(id);

            return Convert(entity);
        }

        public async Task<bool> Update(UpdateRequest notification)
        {
            var notificationEntity = mapper.Map<NotificationEntity>(notification);
            await notificationRepository.ReplaceOneAsync(notificationEntity);

            return true;
        }

        public async Task<bool> Delete(string id)
        {
            await notificationRepository.DeleteByIdAsync(id);

            return true;
        }

        public async Task<bool> AddOrUpdateForGame(UpdateRequest notification)
        {
            var update = Builders<NotificationEntity>.Update.Set(field => field.Messages, notification.Messages);

            if (string.IsNullOrWhiteSpace(notification.GameName))
            {
                await notificationRepository.UpdateManyAsync(entity => entity.DisplayKind == byte.Parse(notification.DisplayKind.Value), update);
            }
            else
            {
                var existNotification = await GetByGameName(notification.GameName, notification.DisplayKind);

                if (existNotification == null)
                {
                    await notificationRepository.InsertOneAsync(new NotificationEntity
                    {
                        GameName = notification.GameName,
                        DisplayKind = byte.Parse(notification.DisplayKind.Value),
                        Messages = notification.Messages
                    });
                }
                else
                {
                    await notificationRepository.UpdateAsync(entity => entity.GameName == notification.GameName, update);
                }
            }

            return true;
        }

        public async Task<Notification> GetByGameName(string gameName, NotificationDisplayKind displayKind)
        {
            var entity = await notificationRepository.FindOneAsync(entity => entity.GameName == gameName && entity.DisplayKind == byte.Parse(displayKind.Value));

            return entity == null ? null : Convert(entity);
        }

        public async Task<bool> AddDisableMessage(string gameName, Dictionary<string, string> messages)
        {
            var notificationEntity = new NotificationEntity
            {
                Message = null,
                DisplayKind = byte.Parse(NotificationDisplayKind.DisabledGame.Value),
                GameName = gameName,
                Messages = messages
            };

            await notificationRepository.InsertOneAsync(notificationEntity);
            return true;
        }

        public async Task<IEnumerable<Notification>> GetPromotionAnnouncementByStatus(string status)
        {
            var entities = status.Equals(PromotionStatus.AllValue)
                ? await notificationRepository
                    .FilterByAsync(entity => entity.DisplayKind == byte.Parse(NotificationDisplayKind.Promotion.Value))
                : await notificationRepository
                    .FilterByAsync(entity => entity.DisplayKind == byte.Parse(NotificationDisplayKind.Promotion.Value) && entity.Enabled == status.Equals(PromotionStatus.EnabledValue));
            return entities.Select(a => ConvertToPromotionAnnouncement(a));
        }

        public async Task<Notification> GetPromotionAnnouncementByPromotionId(string promotionId)
        {
            var announcement = await notificationRepository.FindOneAsync(a => a.PromotionId.Equals(promotionId));
            if (announcement != null)
            {
                return ConvertToPromotionAnnouncement(announcement);
            }
            return null;
        }

        public async Task<bool> CreatePromotionAnnouncements(IEnumerable<CreateAnnouncementModel> models)
        {
            var entities = models.Select(model => mapper.Map<NotificationEntity>(model));
            await notificationRepository.InsertManyAsync(entities);
            return true;
        }

        public async Task<Notification> GetPromotionAnnouncementById(string id)
        {
            var announcement = await notificationRepository.FindByIdAsync(id);
            if (announcement != null)
            {
                return ConvertToPromotionAnnouncement(announcement);
            }
            return null;
        }

        public async Task<bool> UpdatePromotionAnnouncement(Notification model)
        {
            var entity = await notificationRepository.FindByIdAsync(model.Id);
            if (entity != null)
            {
                var update = Builders<NotificationEntity>.Update.Set(field => field.Enabled, model.Enabled)
                .Set(field => field.Title, model.Title)
                .Set(field => field.Messages, model.Messages)
                .Set(field => field.StartTime, model.StartTime)
                .Set(field => field.EndTime, model.EndTime)
                .Set(field => field.PromotionId, model.PromotionId)
                .Set(field => field.GameName, model.GameName)
                .Set(field => field.GameId, model.GameId);

                await notificationRepository.UpdateAsync(notification => notification.Id.Equals(ObjectId.Parse(model.Id)), update);

                return true;
            }
            return false;
        }

        private static Notification Convert(NotificationEntity entity)
            => new Notification(
                entity.Id.ToString(),
                entity.Message,
                Enumeration.FromValue<NotificationDisplayKind>(entity.DisplayKind.ToString()),
                entity.Messages);

        private static Notification ConvertToPromotionAnnouncement(NotificationEntity entity)
          => new Notification(
              entity.Id.ToString(),
                      entity.Message,
                      Enumeration.FromValue<NotificationDisplayKind>(entity.DisplayKind.ToString()),
                      entity.Messages,
                      entity.Title,
                      entity.StartTime,
                      entity.EndTime,
                      entity.PromotionId,
                      entity.Enabled,
                      entity.GameId,
                      entity.GameName
              );
    }
}