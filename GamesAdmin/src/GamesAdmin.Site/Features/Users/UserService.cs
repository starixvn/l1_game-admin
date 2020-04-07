using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using Refit;
using Sentry;

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

        [Get("/users/{username}")]
        Task<User> Get(string username);

        [Post("/users/signin")]
        Task<User> SignIn(string username, string password);
    }

    public interface IUserService
    {
        Task<bool> Create(User user);

        Task<IEnumerable<User>> GetAll();

        Task<bool> Delete(User user);

        Task<User> Get(string username);

        Task<User> SignIn(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly IUserApi userApi;
        private readonly ISentryClient sentryClient;

        public UserService(HttpClient httpClient, ISentryClient sentryClient)
        {
            userApi = RestService.For<IUserApi>(httpClient);
            this.sentryClient = sentryClient;
        }

        public async Task<bool> Create(User user)
        => await userApi.Add(user);

        public async Task<bool> Delete(User user)
        => await userApi.Delete(user);

        public async Task<User> Get(string username)
        => await userApi.Get(username);

        public async Task<IEnumerable<User>> GetAll()
        => await userApi.GetAll();

        public async Task<User> SignIn(string username, string password)
        => await userApi.SignIn(username, password);
    }
}