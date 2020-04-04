using GamesAdmin.Core.Models;
using Refit;
using Sentry;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Users
{
    public interface IUserApi 
    {
        [Get("/users/")]
        Task<IEnumerable<User>> GetAll();

        [Post("/users/add")]
        Task<bool> Add(User user);

        [Post("/users/delete")]
        Task<bool> Delete(User user);
    }

    public interface IUserService 
    {
        Task<bool> Create(User user);

        Task<IEnumerable<User>> GetAll();

        Task<bool> Delete(User user);
    }

    public class UserService : IUserService
    {
        private readonly IUserApi userApi;
        private readonly ISentryClient sentryClient;

        public UserService(IUserApi userApi, ISentryClient sentryClient)
        {
            this.userApi = userApi;
            this.sentryClient = sentryClient;
        }

        public Task<bool> Create(User user)
        => userApi.Add(user);

        public Task<bool> Delete(User user)
        => userApi.Delete(user);

        public Task<IEnumerable<User>> GetAll()
        => userApi.GetAll();
    }
}
