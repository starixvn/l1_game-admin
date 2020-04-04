using GamesAdmin.Core.Models;
using Refit;
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
        Task<bool> AddUsers(User user);
    }

    public interface IUserService 
    {
        Task<bool> Create(User user);

        Task<IEnumerable<User>> GetAll();

        Task<bool> Delete(User user);
    }

    public class UserService : IUserService
    {
        public Task<bool> Create(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
