using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Notifications.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

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
    }

    public class NotificationService : INotificationService
    {
        private readonly IMapper mapper;
        private readonly IGenericRepository<NotificationEntity> notificationRepository;

        public NotificationService(
            IMapper mapper,
            IGenericRepository<NotificationEntity> notificationRepository)
        {
            this.notificationRepository = notificationRepository;
            this.mapper = mapper;
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
                .FilterByAsync(n => true))
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

        private static Notification Convert(NotificationEntity entity)
            => new Notification(
                entity.Id.ToString(),
                entity.Message,
                Enumeration.FromValue<NotificationDisplayKind>(entity.DisplayKind.ToString()),
                entity.Messages);

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
    }
}