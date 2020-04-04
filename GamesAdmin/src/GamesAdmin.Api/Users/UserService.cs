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
                    username = user.Username,
                    password = PasswordHelper.HashPassword(user.Password),
                    time = DateTime.Now
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
                await userRepository.DeleteOneAsync(entity => entity.username == user.Username);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public Task<IEnumerable<User>> GetAll()
         => Task.Run(() =>
         {
             try
             {
                 var users = userRepository.AsQueryable().ToList();

                 return users
                     .Select(entity => mapper.Map<User>(entity));
             }
             catch (Exception ex)
             {
                 sentryClient.CaptureException(ex);

                 return Enumerable.Empty<User>();
             }
         });
    }
}
