using AutoMapper;
using GamesAdmin.Api._Shared.Helpers;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Users
{
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
        private readonly IGenericRepository<AdminUserEntity> userRepository;
        private readonly ISentryClient sentryClient;
        private readonly IMapper mapper;

        public UserService(
            IMapper mapper,
            ISentryClient sentryClient,
            IGenericRepository<AdminUserEntity> userRepository)
        {
            this.mapper = mapper;
            this.sentryClient = sentryClient;
            this.userRepository = userRepository;
        }

        public async Task<bool> Create(User user)
        {
            try
            {
                var entity = new AdminUserEntity
                {
                    Username = user.Username,
                    Password = PasswordHelper.HashPassword(user.Password),
                    Isadmin = user.IsAdmin,
                    Time = DateTime.Now
                };

                await userRepository.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }

            return true;
        }

        public async Task<bool> Delete(User user)
        {
            try
            {
                await userRepository.DeleteOneAsync(entity => entity.Username == user.Username);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<User> Get(string username)
        {
            try
            {
                var userEntity = await userRepository.FindOneAsync(entity => entity.Username.Equals(username));

                if (userEntity != null)
                {
                    return new User(userEntity.Username, string.Empty, userEntity.Isadmin);
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return default;
        }

        public Task<IEnumerable<User>> GetAll()
         => Task.Run(() =>
         {
             try
             {
                 var users = userRepository.AsQueryable().ToList();

                 return users
                     .Select(entity => new User(entity.Username, string.Empty, entity.Isadmin));
             }
             catch (Exception ex)
             {
                 sentryClient.CaptureException(ex);

                 return Enumerable.Empty<User>();
             }
         });

        public async Task<User> SignIn(string username, string password)
        {
            try
            {
                var userEntity = await userRepository.FindOneAsync(entity => entity.Username.Equals(username));

                if (userEntity != null && PasswordHelper.VerifyHashPassword(password, userEntity.Password))
                {
                    return new User(userEntity.Username, string.Empty, userEntity.Isadmin);
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return default;
        }
    }
}
