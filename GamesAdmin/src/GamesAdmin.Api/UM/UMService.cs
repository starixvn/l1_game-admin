using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.Um;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.UM
{
    public interface IUMService
    {
        Task<bool> Process(bool isUM, DateTimeOffset startTime, DateTimeOffset endTime);

        Task<bool> AddSchedule(UmSchedule umSchedule);

        Task<IList<UmSchedule>> GetAll();
    }

    public class UMService : IUMService
    {
        private readonly ISentryClient sentryClient;
        private readonly IMapper mapper;
        private readonly IGenericRepository<UMInfoEntity> umRepository;
        private readonly IGenericRepository<GameConfigEntity> configRepository;
        private readonly IGenericRepository<EventScheduleEntity> scheduleRepository;
        private readonly IGenericRepository<NotificationEntity> notificationRepository;
        private readonly IGenericRepository<UserSessionEntity> usersessionRepository;

        public UMService(
            ISentryClient sentryClient,
            IMapper mapper,
            IGenericRepository<UMInfoEntity> umRepository,
            IGenericRepository<GameConfigEntity> configRepository,
            IGenericRepository<EventScheduleEntity> scheduleRepository,
            IGenericRepository<NotificationEntity> notificationRepository,
            IGenericRepository<UserSessionEntity> usersessionRepository)
        {
            this.sentryClient = sentryClient;
            this.mapper = mapper;
            this.umRepository = umRepository;
            this.configRepository = configRepository;
            this.scheduleRepository = scheduleRepository;
            this.notificationRepository = notificationRepository;
            this.usersessionRepository = usersessionRepository;
        }

        public Task<bool> Process(bool isUM, DateTimeOffset startTime, DateTimeOffset endTime) => isUM ? Start(startTime, endTime) : End();

        public async Task<bool> Start(DateTimeOffset startTime, DateTimeOffset endTime)
        {
            try
            {
                sentryClient.CaptureMessage($"{DateTime.Now} UM Start: {startTime} End: {endTime}");

                await UpdateEnableConfig(false);
                await UpdateUserSessions();

                var umInfo = umRepository.FilterBy(um => !um.Finish).FirstOrDefault();

                if (umInfo == null)
                {
                    await umRepository.InsertOneAsync(new UMInfoEntity
                    {
                        Start = startTime == default ? default : startTime.ToUniversalTime().UtcDateTime,
                        End = endTime == default ? default : endTime.ToUniversalTime().UtcDateTime,
                        Finish = false,
                        Time = DateTime.UtcNow
                    });
                }

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> End()
        {
            try
            {
                sentryClient.CaptureMessage($"{DateTime.Now} UM Finished");

                await UpdateEnableConfig(true);

                var umInfo = umRepository.FilterBy(um => !um.Finish).FirstOrDefault();

                if (umInfo != null)
                {
                    umInfo.Finish = true;

                    await umRepository.ReplaceOneAsync(umInfo);
                }

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        private async Task UpdateEnableConfig(bool enable)
        {
            var update = Builders<GameConfigEntity>.Update.Set(field => field.Enabled, enable);

            await configRepository.UpdateManyAsync(config => true, update);
        }

        private async Task UpdateUserSessions()
        {
            const int sessionExpiredTimeInMinutes = 20;
            var minTime = DateTime.Now.AddMinutes(-sessionExpiredTimeInMinutes);
            var update = Builders<UserSessionEntity>.Update.Set(field => field.RecheckInTime, minTime);
            await usersessionRepository.UpdateManyAsync(ss => minTime < ss.RecheckInTime, update);
        }

        public async Task<bool> AddSchedule(UmSchedule umSchedule)
        {
            var schedule = mapper.Map<EventScheduleEntity>(umSchedule);
            schedule.EventType = byte.Parse(EventScheduleType.UnderMaintenance.Value);

            var notification = new NotificationEntity
            {
                Message = umSchedule.Message,
                DisplayKind = byte.Parse(NotificationDisplayKind.Marquee.Value)
            };

            await scheduleRepository.InsertOneAsync(schedule);
            await notificationRepository.InsertOneAsync(notification);

            return true;
        }

        public async Task<IList<UmSchedule>> GetAll()
        => (await scheduleRepository
            .FilterByAsync(schedule => schedule.EventType == byte.Parse(EventScheduleType.UnderMaintenance.Value)))
            .Select(schedule => mapper.Map<UmSchedule>(schedule))
            .ToList();
    }
}
