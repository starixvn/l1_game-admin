using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Notifications.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.Notifications
{
    public interface INotificationService
    {
        public Task<IList<Notification>> Get();

        public Task<bool> Create(AddRequest notification);
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

        public async Task<bool> Create(AddRequest addRequest)
        {
            var notificationEntity = mapper.Map<NotificationEntity>(addRequest);
            await notificationRepository.InsertOneAsync(notificationEntity);

            return true;
        }

        public async Task<IList<Notification>> Get()
        {
            var notifications = (await notificationRepository
                .FilterByAsync(n => true))
                .ToList();

            return notifications.Select(entity => new Notification(
                entity.Id.ToString(),
                entity.Message,
                Enumeration.FromValue<EventScheduleType>(entity.EventType.ToString()),
                Enumeration.FromValue<NotificationDisplayKind>(entity.DisplayKind.ToString()))).ToList();
        }

    }
}
