namespace GamesAdmin.Api
{
    using AutoMapper;
    using GamesAdmin.Api.ChatBot;
    using GamesAdmin.Api.Domain.Requests;
    using GamesAdmin.Api.SentryClient;
    using GamesAdmin.Core.Models;
    using GamesAdmin.Database;
    using GamesAdmin.Database.Entities;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IDomainService
    {
        Task<DomainModel> Get();

        Task<bool> CreateOrUpdate(DomainModel model);
    }

    public class DomainService : IDomainService
    {
        private readonly IGenericRepository<DomainEntity> domainRepository;

        private readonly IMapper mapper;

        private readonly ISentryClient sentryClient;

        private readonly IChatBotService chatBotService;

        public DomainService(IGenericRepository<DomainEntity> domainRepository,
           IMapper mapper, ISentryClient sentryClient, IChatBotService chatBotService)
        {
            this.domainRepository = domainRepository;
            this.mapper = mapper;
            this.sentryClient = sentryClient;
            this.chatBotService = chatBotService;
        }

        public async Task<DomainModel> Get()
        {
            try
            {
                var domainEntity = await Task.Run(() => domainRepository.AsQueryable().FirstOrDefault());

                var domainModels = mapper.Map<DomainModel>(domainEntity) ?? new DomainModel();
                return domainModels;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return new DomainModel();
            }
        }

        public async Task<bool> CreateOrUpdate(DomainModel model)
        {
            try
            {
                var existingDomainEntity = await Task.Run(() => domainRepository.AsQueryable().FirstOrDefault());
                var domainEntity = mapper.Map<DomainEntity>(model);
                if (existingDomainEntity == null)
                {
                    await domainRepository.InsertOneAsync(domainEntity);
                }
                else
                {
                    var changedSetting = new SettingChangedModel($"Domains", existingDomainEntity.Value, model.Value);
                    await chatBotService.NotifyAdminUpdatedSettings(string.Empty, "Domain's Setting ", string.Empty, new List<SettingChangedModel> { changedSetting });
                    var update = Builders<DomainEntity>.Update
                                .Set(field => field.Value, model.Value);
                    await domainRepository.UpdateAsync(entity => entity.Id.Equals(ObjectId.Parse(model.Id)), update );
                }


                return true;

            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }
    }
}
