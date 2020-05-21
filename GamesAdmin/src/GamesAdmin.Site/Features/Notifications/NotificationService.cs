using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.Notifications
{
    public interface INotificationApi : IBaseAuthorizationApi
    {
        [Get("/notifications")]
        Task<IList<Notification>> Get();

        [Post("/notifications/add")]
        Task<bool> Add([Body] Notification notification);
    }

    public interface INotificationService
    {
        public Task<IList<Notification>> Get();

        public Task<bool> Create(Notification notification);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationApi api;

        public NotificationService(INotificationApi api)
        {
            this.api = api;
        }

        public Task<bool> Create(Notification notification)
        => api.Add(notification);

        public Task<IList<Notification>> Get()
        => api.Get();
    }
}
