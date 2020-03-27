using AutoMapper;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;
using System;
using System.Threading.Tasks;

namespace GamesAdmin.Api.UM
{
    public interface IUMService 
    {
        Task<bool> AddSchedule(DateTimeOffset start, DateTimeOffset end);
    }

    public class UMService : IUMService
    {
        private readonly IMapper mapper;
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<UMInfoEntity> repository;

        public UMService(
            IMapper mapper, 
            ISentryClient sentryClient, 
            IGenericRepository<UMInfoEntity> repository) 
        {
            this.mapper = mapper;
            this.sentryClient = sentryClient;
            this.repository = repository;
        }

        public async Task<bool> AddSchedule(DateTimeOffset start, DateTimeOffset end)
        {
            try
            {
                await repository.InsertOneAsync(new UMInfoEntity
                {
                    start = start.UtcDateTime,
                    end = end.UtcDateTime,
                    finish = false,
                    time = DateTime.Now
                }); ;

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }
    }
}
