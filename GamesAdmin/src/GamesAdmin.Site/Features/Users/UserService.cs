using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;
using Sentry;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Users
{
    public interface IUserApi : IBaseAuthorizationApi
    {
        [Get("/users/")]
        Task<IEnumerable<User>> GetAll();

        [Post("/users/add")]
        Task<bool> Add(User user);

        [Post("/users/delete")]
        Task<bool> Delete(User user);

        [Get("/users/{username}")]
        Task<User> Get(string username);

        [Post("/users/signin")]
        Task<User> SignIn(string username, string password);

        [Post("/users/change_password")]
        Task<bool> ChangePassword(string username, string password, string newPassword);

        [Post("/users/createJwtToken")]
        Task<string> CreateJwtToken(string username, string password);
    }

    public interface IUserService
    {
        Task<bool> Create(User user);

        Task<IEnumerable<User>> GetAll();

        Task<bool> Delete(User user);

        Task<User> Get(string username);

        Task<User> SignIn(string username, string password);

        Task<bool> ChangePassword(string username, string password, string newPassword);
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

        public Task<bool> ChangePassword(string username, string password, string newPassword)
        => userApi.ChangePassword(username, password, newPassword);

        public Task<bool> Create(User user)
        => userApi.Add(user);

        public Task<bool> Delete(User user)
        => userApi.Delete(user);

        public Task<User> Get(string username)
        => userApi.Get(username);

        public Task<IEnumerable<User>> GetAll()
        => userApi.GetAll();

        public async Task<User> SignIn(string username, string password)
        {
            try
            {
                return await userApi.SignIn(username, password).ConfigureAwait(false);
            }
            catch (ApiException ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }
    }
}