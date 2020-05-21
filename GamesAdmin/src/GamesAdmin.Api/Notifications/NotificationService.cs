using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Notifications.Requests;
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

        public async Task<bool> Create(AddRequest notification)
        {
            var notificationEntity = mapper.Map<NotificationEntity>(notification);
            await notificationRepository.InsertOneAsync(notificationEntity);

            return true;
        }

        public async Task<IList<Notification>> Get()
        => (await notificationRepository
                .FilterByAsync(n => true))
                .Select(x => mapper.Map<Notification>(x))
                .ToList();
    }
}
