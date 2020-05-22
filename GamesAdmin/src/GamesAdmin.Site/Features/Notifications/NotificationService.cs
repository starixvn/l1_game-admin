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

        [Get("/notifications/{id}")]
        Task<Notification> GetById(string id);

        [Post("/notifications/add")]
        Task<bool> Add([Body] Notification notification);

        [Post("/notifications/update")]
        Task<bool> Update([Body] Notification notification);

        [Post("/notifications/delete/id")]
        Task<bool> Delete(string id);
    }

    public interface INotificationService
    {
        public Task<IList<Notification>> Get();

        public Task<bool> Add(Notification notification);

        public Task<Notification> GetById(string id);

        public Task<bool> Update(Notification notification);

        public Task<bool> Delete(string id);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationApi api;

        public NotificationService(INotificationApi api)
        {
            this.api = api;
        }

        public Task<bool> Add(Notification notification)
        => api.Add(notification);

        public Task<IList<Notification>> Get()
        => api.Get();

        public Task<Notification> GetById(string id)
        => api.GetById(id);

        public Task<bool> Update(Notification notification)
        => api.Update(notification);

        public Task<bool> Delete(string id)
        => api.Delete(id);
    }
}
